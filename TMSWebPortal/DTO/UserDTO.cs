 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TMSWebPortal.Models;

namespace TMSWebPortal.DTO
{
    public class UserDTO:OutputHandler
    {
        [Display(Name="Full Name")]
        public string Name { get; set; }
        [Display(Name = "Date Of Birth")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateOfBirth { get; set; } = DateTime.UtcNow.AddHours(2);
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
        [Display(Name="Email Address")]
        public string EmailAddress { get; set; }
        [Display(Name = "Branch")]
        public long PersonalDetailsId { get; set; }
        [Display(Name="Branch Name")]
        public int BranchId { get; set; }
        [Display(Name = "Position")]

        public int PositionId { get; set; }
        
        public int RoleId { get; set; }
        public string Password { get; set; }
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }
        [Display(Name = "Firstname")]
        public string FirstName { get; set; }
        [Display(Name = "Lastname")]
        public string LastName { get; set; }
        public IEnumerable<Branch> Branches { get; set; }
        public IEnumerable<Roles> Roles { get; set; }
        public IEnumerable<Positions> Positions { get; set; }
        public bool IsOfficerCreation { get; set; }
    }
}
