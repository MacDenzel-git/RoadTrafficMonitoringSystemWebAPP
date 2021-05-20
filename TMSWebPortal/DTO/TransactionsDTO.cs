using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TMSWebPortal.DTO
{
    public class TransactionsDTO
    {
 
        public string CrimeName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public string LicenseNumber { get; set; }
        public string VehicleRegistrationNumber { get; set; }
        public decimal CrimeCharge { get; set; }
        public bool Paid { get; set; }
    }
}
