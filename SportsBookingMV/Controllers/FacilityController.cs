using Microsoft.AspNetCore.Mvc;
using SportsBookingMVC.Models;
using System.Data.SqlClient;

namespace SportsBookingMVC.Controllers
{
    public class FacilityController : Controller
    {
        private readonly DbHelper _dbHelper;

        public FacilityController(IConfiguration configuration)
        {
            _dbHelper = new DbHelper(configuration);
        }

        // Search action for both Guests and Members
        public IActionResult Search(string searchType, string location)
        {
            List<FacilityModel> facilities = new List<FacilityModel>();

            using (var conn = _dbHelper.GetConnection())
            {
                string query = "SELECT * FROM Facility WHERE 1=1";
                if (!string.IsNullOrEmpty(searchType)) query += " AND FacilityType LIKE @Type";
                if (!string.IsNullOrEmpty(location)) query += " AND Location LIKE @Location";

                using (var cmd = new SqlCommand(query, conn))
                {
                    if (!string.IsNullOrEmpty(searchType)) cmd.Parameters.AddWithValue("@Type", "%" + searchType + "%");
                    if (!string.IsNullOrEmpty(location)) cmd.Parameters.AddWithValue("@Location", "%" + location + "%");

                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            facilities.Add(new FacilityModel
                            {
                                FacilityID = reader.GetInt32(0),
                                FacilityName = reader.GetString(1),
                                FacilityType = reader.GetString(2),
                                Location = reader.GetString(3),
                                HourlyRate = reader.GetDecimal(4),
                                Description = reader.IsDBNull(5) ? "" : reader.GetString(5),
                                IsAvailable = reader.GetBoolean(6)
                            });
                        }
                    }
                }
            }
            return View(facilities);
        }

        // GET: Book Facility (Restricted to logged-in members)
        [HttpGet]
        public IActionResult Book(int id)
        {
            int? memberId = HttpContext.Session.GetInt32("MemberID");
            if (memberId == null)
            {
                TempData["Error"] = "You must log in as a member to book a facility.";
                return RedirectToAction("Login", "Account");
            }

            ViewBag.FacilityID = id;
            return View();
        }

        // POST: Book Facility
        [HttpPost]
        public IActionResult Book(int facilityId, DateTime bookingDate, TimeSpan startTime, TimeSpan endTime)
        {
            int? memberId = HttpContext.Session.GetInt32("MemberID");
            if (memberId == null) return RedirectToAction("Login", "Account");

            using (var conn = _dbHelper.GetConnection())
            {
                string query = "INSERT INTO Booking (MemberID, FacilityID, BookingDate, StartTime, EndTime, Status) VALUES (@MemberID, @FacilityID, @Date, @Start, @End, 'Confirmed')";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MemberID", memberId.Value);
                    cmd.Parameters.AddWithValue("@FacilityID", facilityId);
                    cmd.Parameters.AddWithValue("@Date", bookingDate);
                    cmd.Parameters.AddWithValue("@Start", startTime);
                    cmd.Parameters.AddWithValue("@End", endTime);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            TempData["Success"] = "Facility booked successfully!";
            return RedirectToAction("Search");
        }

        // View Reviews for a Facility (Available to Guests & Members)
        public IActionResult Reviews(int facilityId)
        {
            ViewBag.FacilityID = facilityId;
            return View();
        }

        [HttpPost]
        public IActionResult AddReview(int facilityId, int rating, string comments)
        {
            int? memberId = HttpContext.Session.GetInt32("MemberID");
            if (memberId == null)
            {
                TempData["Error"] = "Please log in to submit a review.";
                return RedirectToAction("Login", "Account");
            }

            using (var conn = _dbHelper.GetConnection())
            {
                string query = "INSERT INTO Review (FacilityID, MemberID, Rating, Comments) VALUES (@FacID, @MemID, @Rating, @Comments)";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@FacID", facilityId);
                    cmd.Parameters.AddWithValue("@MemID", memberId.Value);
                    cmd.Parameters.AddWithValue("@Rating", rating);
                    cmd.Parameters.AddWithValue("@Comments", comments);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            TempData["Success"] = "Review submitted successfully!";
            return RedirectToAction("Search");
        }
    }
}