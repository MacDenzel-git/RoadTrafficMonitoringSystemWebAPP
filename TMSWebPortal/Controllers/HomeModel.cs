using System.Collections;
using System.Collections.Generic;
using TMSWebPortal.DTO;

namespace TMSWebPortal.Controllers
{
    public class HomeModel
    {
        public int roleid { get; set; }
        public string  username { get; set; }
        public IEnumerable<Crimes> Crimes { get; set; }
        public string CrimeName { get; set; }
    }
}