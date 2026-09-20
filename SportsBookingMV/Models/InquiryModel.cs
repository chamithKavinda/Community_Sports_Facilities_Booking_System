using System;

namespace SportsBookingMVC.Models
{
    public class InquiryModel
    {
        public int InquiryID { get; set; }
        public string GuestName { get; set; }
        public string GuestEmail { get; set; }
        public string Message { get; set; }
        public DateTime InquiryDate { get; set; }
    }
}