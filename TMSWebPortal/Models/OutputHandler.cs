using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TMSWebPortal.Models
{
    public class OutputHandler
    {
        public bool IsErrorOccured { get; set; }
        public string Message { get; set; }
        public object Result { get; set; }
        public int NumberOfCrimes { get; set; }
        public string Identifier { get; set; }
    }
}
