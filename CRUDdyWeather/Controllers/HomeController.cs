using System.Diagnostics;
using CRUDdyWeather.Enums;
using CRUDdyWeather.Models;
using Microsoft.AspNetCore.Mvc;

namespace CRUDdyWeather.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult About()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SubmitForm(string Name, string Email, ForcastType type)
        {
            if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(type.ToString()))
            {
                return BadRequest("Invalid data");
            }

            // Process form data (save to database, etc.)
            return Json(new { success = true, message = "Data submitted successfully!" });
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
