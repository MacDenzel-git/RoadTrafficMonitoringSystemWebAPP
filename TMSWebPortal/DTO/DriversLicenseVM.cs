using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMSWebPortal.Models;

namespace TMSWebPortal.DTO
{
    public class DriversLicenseVM:OutputHandler
    {
       public IEnumerable<DriverLicenseDTO> DriverLicenses { get; set; }
    }
}
