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
        public static ClientConnect getInstance(int _port)
        {
            if (ClientConnect._instance == null)
            {
                _instance = new ClientConnect();
                _instance.port = _port;
            }
            return _instance;
        }
        TcpListener tcpListener;
        Thread threadListener;
        public void Connect(CancellationToken cts)
        {
            _cts = cts;
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
            Task.Run(() => MyListener(), _cts);
            //threadListener = new Thread(MyListener);
            //threadListener.Start(threadListener);
        }
        public void Disconnect()
        {
            tcpListener.Stop();
        }
        public static List<ClientOperation> clients = new List<ClientOperation>();
        //public void MyListener(object obj)
        //{
        //    if (obj == null)
        //        return;
        //    onError?.Invoke(" Listener Start ");
        //    tcpListener.Start();
        //    //Task t = new Task(new Action(() => tcpListener.AcceptTcpClient()));
        //    while (!_cts.IsCancellationRequested)
        //    {
        //        //if (t.Status == TaskStatus.WaitingForActivation)
        //        //    t.Start();
        //        clients.Add(new ClientOperation(tcpListener.AcceptTcpClient()));
        //        onAccess?.Invoke("New Client must start");
        //    }
        //    Disconnect();
        //}
        public async void MyListener()
        {
            onError?.Invoke(" Listener Start ");
            tcpListener.Start();
            while (!_cts.IsCancellationRequested)
            {
                var listner = await tcpListener.AcceptSocketAsync();
                if (listner != null)
                clients.Add(new ClientOperation(listner));
                onAccess?.Invoke("New Client must start");
            }
            Disconnect();
        }
    }
}