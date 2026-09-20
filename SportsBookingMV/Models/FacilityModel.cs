namespace SportsBookingMVC.Models
{
    public class FacilityModel
    {
        public int FacilityID { get; set; }
        public string FacilityName { get; set; }
        public string FacilityType { get; set; }
        public string Location { get; set; }
        public decimal HourlyRate { get; set; }
        public string Description { get; set; }
        public bool IsAvailable { get; set; }
    }
}