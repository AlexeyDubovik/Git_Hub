using System.Threading;

namespace Spy
{
    internal class ThreadControl
    {
        public ThreadControl()
        {
            thread = null;
            cts = null;
            _manualEvent = null;
        }
        public Thread thread { get; set; }
        public CancellationTokenSource cts { get; set; }
        public ManualResetEvent _manualEvent { get; set; }
    }
}
