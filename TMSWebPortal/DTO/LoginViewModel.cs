 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMSWebPortal.Controllers;

namespace TMSWebPortal.DTO
{
    
        public class LoginViewModel : ResultHandler
        {
            public string Username { get; set; }
            public string Password { get; set; }
            public string ConfirmPassword { get; set; } //used for change password

        }
    
}
