using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TMSWebPortal.DTO
{
    public class Totals
    {
        public int CrimesCount { get; set; }
        public int Licenses { get; set; }
        public int VehiclesRegistered { get; set; }
        public int CrimesToday { get; set; }
    }
}
