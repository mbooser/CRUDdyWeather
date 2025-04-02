using Microsoft.AspNetCore.Mvc;

namespace CRUDdyWeather.Controllers
{
    public class HomeController1 : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
