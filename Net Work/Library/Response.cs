using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    [Serializable]
    public class Response
    {
        public bool success { get; set; }
        public ResponseCodes status { get; set; }
        public string statusText { get; set; }
        public object data { get; set; }
    }
}
