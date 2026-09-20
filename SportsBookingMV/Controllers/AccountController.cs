using Microsoft.AspNetCore.Mvc;
using SportsBookingMVC.Models;
using System.Data.SqlClient;

namespace SportsBookingMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly DbHelper _dbHelper;

        public AccountController(IConfiguration configuration)
        {
            _dbHelper = new DbHelper(configuration);
        }

        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public IActionResult Register(string fullName, string email, string password, string contact, string address, string sports)
        {
            using (var conn = _dbHelper.GetConnection())
            {
                string query = "INSERT INTO Members (FullName, Email, Password, ContactNumber, Address, PreferredSports) VALUES (@Name, @Email, @Pass, @Contact, @Address, @Sports)";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Name", fullName);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Pass", password);
                    cmd.Parameters.AddWithValue("@Contact", contact);
                    cmd.Parameters.AddWithValue("@Address", address);
                    cmd.Parameters.AddWithValue("@Sports", sports);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            using (var conn = _dbHelper.GetConnection())
            {
                string query = "SELECT MemberID, FullName FROM Members WHERE Email=@Email AND Password=@Pass";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Pass", password);
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            HttpContext.Session.SetInt32("MemberID", reader.GetInt32(0));
                            HttpContext.Session.SetString("MemberName", reader.GetString(1));
                            return RedirectToAction("Index", "Home");
                        }
                    }
                }
            }
            ViewBag.Error = "Invalid Login Credentials";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}