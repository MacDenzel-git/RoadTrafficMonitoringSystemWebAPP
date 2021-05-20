 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMSWebPortal.Controllers;

namespace TMSWebPortal.DTO
{
    public class RecoverPasswordDTO : ResultHandler
    {
       public string Username { get; set; }
        public string Otp { get; set; }
    }
}
