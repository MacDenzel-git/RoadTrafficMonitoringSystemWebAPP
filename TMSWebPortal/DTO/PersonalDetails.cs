using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TMSWebPortal.DTO
{
    public class PersonalDetails
    {
        public long PersonId { get; set; }
        [Display(Name = "Full Name")]
        public string Name { get; set; }
        [Display(Name = "Date of Birth")]
        public DateTime Age { get; set; }
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }
        [Display(Name = "Branch")]
        public int BranchId { get; set; }
        [Display(Name = "Position")]
        public int PositionId { get; set; }
        [Display(Name = "Roles")]
        public int RoleId { get; set; }

        
    }
}
