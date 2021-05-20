using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TMSWebPortal.DTO
{
    public class CrimeMantainainceDTO
    {
        public int CrimeChargeId { get; set; }
        public string CrimeName { get; set; }
        public decimal Charge { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }
}
