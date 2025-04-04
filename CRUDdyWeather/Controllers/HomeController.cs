using System.Diagnostics;
using CRUDdyWeather.Enums;
using CRUDdyWeather.Models;
using CRUDdyWeather.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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

            // Store the raw JSON data in the SearchEntity
            searchEntity.DumpJSON = jsonData;

            // Return the ViewModel to the view for rendering
            return Json(searchEntity);
        }

        private void ParseWeatherData(WeatherViewModel viewModel, string jsonData)
        {
            switch (viewModel.Ftype)
            {
                default:
                    viewModel.CurrentWeather.Temperature = UrlCaller.parseResponse(jsonData, viewModel.Ftype, "temperature_2m");
                    viewModel.CurrentWeather.WindSpeed = UrlCaller.parseResponse(jsonData, viewModel.Ftype, "wind_speed_10m");
                    viewModel.CurrentWeather.WeatherDescription = UrlCaller.ParseWeather_Code(UrlCaller.parseResponse(jsonData, viewModel.Ftype, "weather_code"));
                    viewModel.CurrentWeather.Humidity = UrlCaller.parseResponse(jsonData, viewModel.Ftype, "relative_humidity_2m");
                    Console.WriteLine("Parsed Current - Temp = "+viewModel.CurrentWeather.Temperature+" Wind Speed = "+viewModel.CurrentWeather.WindSpeed+" Weather = "+viewModel.CurrentWeather.WeatherDescription+" Humidity = "+viewModel.CurrentWeather.Humidity);
                    break;
            }
            if (viewModel.Ftype == ForcastType.Current)
            {
                // Parse the current weather data from JSON
                viewModel.CurrentWeather = JsonConvert.DeserializeObject<CurrentWeather>(jsonData);
            }
            else if (viewModel.Ftype == ForcastType.Daily)
            {
                // Parse the daily forecast data from JSON
                viewModel.DailyForecasts = JsonConvert.DeserializeObject<List<DailyForecast>>(jsonData);
            }
            else if (viewModel.Ftype == ForcastType.Hourly)
            {
                // Parse the hourly forecast data from JSON
                viewModel.HourlyForecasts = JsonConvert.DeserializeObject<List<HourlyForecast>>(jsonData);
            }
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
