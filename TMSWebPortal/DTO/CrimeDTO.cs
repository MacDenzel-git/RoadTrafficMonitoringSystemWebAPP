using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMSWebPortal.Models;

namespace TMSWebPortal.DTO
{
    public class CrimeDTO: OutputHandler
    {
        public string VehicleRegistrationNumber { get; set; }
        public string LicenseNumber { get; set; }
        public string  LoggedInUser { get; set; }
        public string CrimeName { get; set; }
        public decimal CrimeCharge { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
