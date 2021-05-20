using System;

namespace TMSWebPortal.DTO
{
    public class VehicleInsuranceDTO
    {
        public long VehicleId { get; set; }
        public string RegistrationNumber { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateEffective { get; set; }
        public string IssuedBy { get; set; }
    }
}
