using System;

namespace SportsBookingMVC.Models
{
    public class ReviewModel
    {
        public int ReviewID { get; set; }
        public int FacilityID { get; set; }
        public int MemberID { get; set; }
        public string MemberName { get; set; } // Added to display who wrote it
        public int Rating { get; set; }
        public string Comments { get; set; }
        public DateTime ReviewDate { get; set; }
    }
}