using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TMSWebPortal.DTO
{
    public class UsersVM:Totals
    {
        public IEnumerable<UserDetailDTO> UserDetailDTOs { get; set; }
 
    }
}
