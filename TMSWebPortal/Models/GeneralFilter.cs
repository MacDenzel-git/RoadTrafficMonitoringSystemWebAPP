using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMSWebPortal.Controllers;

namespace TMSWebPortal.Models
{
    public class GeneralFilter:HomeModel
    {
        public string RegistrationNumber { get; set; }
        public string  LicenseNumber { get; set; }
    }
}
