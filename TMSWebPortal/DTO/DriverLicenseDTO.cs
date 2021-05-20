using System;
using System.ComponentModel.DataAnnotations;
using TMSWebPortal.Models;

namespace TMSWebPortal.DTO
{
    public class DriverLicenseDTO:OutputHandler

    {
     
         public string LicenseNumber { get; set; }
         public string Trn { get; set; }
        public DateTime FirstIssue { get; set; }
        public DateTime DateIssued { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string CountryIssued { get; set; }
        [StringLength(1, ErrorMessage = "Code length can't be more than 1.")]
        public string LicenseCode { get; set; }
        
        public int VehicleRestriction { get; set; }
        [StringLength(2, ErrorMessage = "Driver Restriction length can't be more than 2.")]
        public string DriverRestriction { get; set; }
         
        public int IssueNumber { get; set; }
        public long PersonId { get; set; }
        public DateTime DateIssuedShort => DateTime.Parse(DateIssued.ToString("MM/dd/yyyy"));
        public DateTime ExpiryDateShort => DateTime.Parse(ExpiryDate.ToString("MM/dd/yyyy"));
        public string PersonInfo { get; set; }
    }
}
