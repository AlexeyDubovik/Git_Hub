using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;


namespace Client
{
    public class ServerConnect
    {
        public event Action<string> onError;
        TcpClient tcpClient; 
        //Thread threadClient; 
        NetworkStream stream;
        BinaryFormatter bf = new BinaryFormatter();
        public void Connect(String _ip, int port)
        {
            try
            {
                IPAddress ip = IPAddress.Parse(_ip);
                tcpClient = new TcpClient();
                tcpClient.Connect(new IPEndPoint(ip, port));
                stream = tcpClient.GetStream();
            }
            catch (Exception ex)
            {
                onError?.Invoke(ex.Message);
            }
        }
        public void cmdFirstBin(String str)
        {
            Library.Request request = new Library.Request { command = Library.Commands.Message };
            request.data = (string)str;
            try
            {
                bf.Serialize(stream, request);
            }
            catch (Exception ex)
            {
                onError?.Invoke(ex.Message);
            }
        }
        public void waitResponse(Action<Library.Response> onOk)
        {
            try
            {
                Library.Response response = (Library.Response)bf.Deserialize(stream);
                if (response.success)
                {
                    onOk(response);
                }
                else
                {
                    onError?.Invoke(response.statusText);
                }
            }
            catch(Exception ex)
            {
                onError?.Invoke(ex.ToString());
            }
        }
    }
}