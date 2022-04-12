using System.Net;
using System.Net.Sockets;

namespace Server
{
    public class ClientConnect
    {
        public event Action<string> onAccess;
        public event Action<string> onError;
        public IPAddress ip = IPAddress.Any;
        public CancellationToken _cts;
        public int port;
        private ClientConnect() { }
        private static ClientConnect _instance;
        public static ClientConnect getInstance()
        {
            if (ClientConnect._instance == null)
            {
                _instance = new ClientConnect();
            }
            return _instance;
        }
        TcpListener tcpListener;
        Thread threadListener;
        public void Connect(CancellationToken cts, int _port)
        {
            _cts = cts;
            port = _port;
            if (tcpListener != null)
            {
                onError?.Invoke(" Socket is bisy");
                return;
            }
            if (threadListener != null)
            {
                onError?.Invoke(" Thread is bisy");
                return;
            }
            tcpListener = new TcpListener(ip, port);
            threadListener = new Thread(MyListener);
            threadListener.Start(threadListener);
        }
        public void Disconnect()
        {
            if (tcpListener != null)
                tcpListener.Stop();
        }
        public static List<ClientOperation> clients = new List<ClientOperation>();
        public void MyListener(object obj)
        {
            if (obj == null)
                return;
            onError?.Invoke(" Listener Start ");
            tcpListener.Start();
            while (!_cts.IsCancellationRequested)
            {
                try
                {
                    clients.Add(new ClientOperation(tcpListener.AcceptTcpClient()));
                    //onAccess?.Invoke("New Client must start");
                }
                catch 
                {
                    return;
                }
            }
        }
    }
}