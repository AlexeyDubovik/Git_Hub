using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Forbidden_Worlds
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
