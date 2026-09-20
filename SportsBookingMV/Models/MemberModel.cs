namespace SportsBookingMVC.Models
{
    public class MemberModel
    {
        public int MemberID { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ContactNumber { get; set; }
        public string Address { get; set; }
        public string PreferredSports { get; set; }
    }
}