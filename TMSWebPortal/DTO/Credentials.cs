using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TMSWebPortal.DTO
{
    public class Credentials
    {
        public int CredentialId { get; set; }
        public long PersonalDetailsId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }

        public virtual PersonalDetails Personal { get; set; }
    }
}
