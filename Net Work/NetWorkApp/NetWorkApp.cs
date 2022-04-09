using Client;
using Server;

namespace NetWorkApp
{
    public partial class NetWorkApp : Form
    {
        private ThreadControl TC;
        private string Sand;
        private int port;
        private string ip;
        private bool IsServer;
        public NetWorkApp()
        {
            InitializeComponent();
            TC = new ThreadControl();
            Sand = DateTime.Now.ToString();
        }
        private void PrintText(String txt)
        {
            if (txt == null)
                return;
            Invoke(new Action(() => listBox1.Items.Add("")));
            for (int i = 0; i < txt.Count(); i++)
            {
                Thread.Sleep(5);
                if (txt.ToArray()[i] == '\n')
                    Invoke(new Action(() => listBox1.Items.Add(txt.ToArray()[i])));
                else
                    Invoke(new Action(() => listBox1.Items[listBox1.Items.Count - 1] += txt.ToArray()[i].ToString()));
            }
        }
        private void _NetWork(object Net)
        {
            if (Net == null)
                return;
            int port;
            String ip;
            try
            {
                port = Port_Box.Text.Length == 0 ? 3333 : int.Parse(Port_Box.Text);
                ip = IP_Box.Text.Length == 0 ? "127.0.0.1" : IP_Box.Text;
            }
            catch
            {
                port = 3333;
                ip = "127.0.0.1";
            }
            if (!IsServer)
            {
                ServerConnect server = new ServerConnect();
                server.onError += (msg) => MessageBox.Show(msg); ;
                server.Connect(ip, port);
                server.cmdFirstBin(Sand);
                server.waitResponse((res) =>
                {
                    String r = (String)res.data;
                    PrintText(r.ToString());
                });
            }
            else
            {
                ClientConnect.getInstance(port).Connect(TC.cts.Token);
                ClientConnect.getInstance(port).onError += PrintText;
                ClientConnect.getInstance(port).onAccess += PrintText;
                ClientOperation.onRun += (msg) => { PrintText(msg); };
            }
        }
        private void _Start(bool param = true)
        {
            IsServer = param;
            if (IsServer)
                PrintText("Start by Srever...");
            else if (Sand == null)
                PrintText("Start by Client...");
            TC._manualEvent = new ManualResetEvent(true);
            TC.cts = new CancellationTokenSource();
            TC.thread = new Thread(_NetWork);
            TC.thread.IsBackground = true;
            TC.thread.Start(TC);
        }
        private void _Stop()
        {
            TC.cts?.Cancel();
            TC.thread = null;
            TC.cts = null;
            PrintText("Stopped this shit");
        }
        private void Start_Click(object sender, EventArgs e)
        {
            if (TC.thread == null)
            {
                switch (new Question().ShowDialog())
                {
                    case DialogResult.Cancel:
                        return;
                    case DialogResult.OK:
                        label2.Text = "Server";
                        _Start(true);
                        break;
                    case DialogResult.No:
                        label2.Text = "Client";
                        _Start(false);
                        break;
                }
                Start.Enabled = false;
                ShutDown.Enabled = true;
            }
        }
        private void ShutDown_Click(object sender, EventArgs e)
        {
            ShutDown.Enabled = false;
            Start.Enabled = true;
            _Stop();
        }
        private void Server_FormClosed(object sender, FormClosedEventArgs e)
        {
            TC.cts?.Cancel();
        }
        private void ClearText_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
        }
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && textBox1.Text.Length > 0 && !IsServer)
            {
                Sand = DateTime.Now.ToString() + "\n" + textBox1.Text;
                TC.cts?.Cancel();
                TC.thread = null;
                _Start(false);
                e.SuppressKeyPress = true;
                textBox1.Clear();
            }
        }
    }
}