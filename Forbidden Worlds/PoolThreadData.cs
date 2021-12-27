using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Forbidden_Worlds
{
    internal class PoolThreadData
    {
        public String path { get; set; }
        public CancellationToken Token { get; set; }
    }
}
