using System.Diagnostics;
using CRUDdyWeather.Enums;
using CRUDdyWeather.Models;
using CRUDdyWeather.Services;
using Microsoft.AspNetCore.Mvc;

namespace CRUDdyWeather.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UrlCaller _urlCaller;

        // Injecting UrlCaller to handle API calls
        public HomeController(ILogger<HomeController> logger, UrlCaller urlCaller)
        {
            _logger = logger;
            _urlCaller = urlCaller;
        }

        // GET: Home/Index
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
        public async Task<IActionResult> SubmitForm(SearchEntity searchEntity)
        {
            if (string.IsNullOrEmpty(searchEntity.Name) || searchEntity.Lat == 0 || searchEntity.Lng == 0 || searchEntity.Ftype == null)
            {
                return BadRequest("Invalid data");
            }

            // Build the URL based on the submitted data
            string url = UrlCaller.UrlBuilder(searchEntity.Lat, searchEntity.Lng, searchEntity.Ftype);

            // Fetch JSON data from the API
            string jsonData = await _urlCaller.FetchJSON(url);

            // Store the data in the SearchEntity model
            searchEntity.DumpJSON = jsonData; // Assuming DumpJSON stores the weather data

            // Optionally, store searchEntity in the database (not shown here)

            // Return the updated model to the view
            return View("Index", searchEntity);  // Pass the updated model back to the main page
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
