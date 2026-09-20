using System;

namespace SportsBookingMVC.Models
{
    public class BookingModel
    {
        public int BookingID { get; set; }
        public int MemberID { get; set; }
        public int FacilityID { get; set; }
        public DateTime BookingDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string Status { get; set; }
    }
}