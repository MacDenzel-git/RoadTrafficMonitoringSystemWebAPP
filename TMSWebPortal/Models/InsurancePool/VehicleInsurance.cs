using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TMSWebPortal.Models.InsurancePool
{
    public class VehicleInsurance
    {
        public long VehicleId { get; set; }
        public string RegistrationNumber { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateEffective { get; set; }
        public string Message { get; set; }
        public bool IsErrorOccured { get; set; }
        public string Identifier { get; set; }
        public string IssuedBy { get; set; }

    }
}
