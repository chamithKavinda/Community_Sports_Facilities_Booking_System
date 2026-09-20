using Microsoft.AspNetCore.Mvc;
using SportsBookingMVC.Models;
using System.Data.SqlClient;

namespace SportsBookingMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly DbHelper _dbHelper;

        public HomeController(IConfiguration configuration)
        {
            _dbHelper = new DbHelper(configuration);
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SendInquiry(string guestName, string guestEmail, string message)
        {
            using (var conn = _dbHelper.GetConnection())
            {
                string query = "INSERT INTO Inquiry (GuestName, GuestEmail, Message) VALUES (@Name, @Email, @Msg)";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Name", guestName);
                    cmd.Parameters.AddWithValue("@Email", guestEmail);
                    cmd.Parameters.AddWithValue("@Msg", message);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            TempData["SuccessMessage"] = "Inquiry sent successfully!";
            return RedirectToAction("Index");
        }
    }
}