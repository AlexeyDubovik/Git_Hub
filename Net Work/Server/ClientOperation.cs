using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class ClientOperation
    {
        TcpClient tcpClient;
        Thread threadClient;
        public static event Action<string> onRun;
        BinaryFormatter bf = new BinaryFormatter();
        public ClientOperation(object client)
        {
            tcpClient = new TcpClient();
            if (client is Socket)
                tcpClient.Client = (client as Socket);
            else
                tcpClient = (client as TcpClient);
            threadClient = new Thread(RunBin);
            threadClient.Start();
        } 
        void RunBin()
        {
            try
            {
                Library.Request req = (Library.Request)bf.Deserialize(tcpClient.GetStream());
                onRun?.Invoke((String)req.data);
                Library.Response response = new Library.Response();
                switch (req.command)
                {
                    case Library.Commands.Message:
                        Answer ans = new Answer((String)req.data);
                        ans.buildResponse(ref response);
                        break;
                    default:
                        response.success = false;
                        response.status = Library.ResponseCodes.Error;
                        response.statusText = "Command not Found";
                        break;
                }
                bf.Serialize(tcpClient.GetStream(), response);
                Close();
            }
            catch (Exception ex)
            {
                onRun?.Invoke("Err" + ex.Message);
            }
        }
        void Close()
        {
            tcpClient.Close();
            ClientConnect.clients.Remove(this);
        }
    }
}
