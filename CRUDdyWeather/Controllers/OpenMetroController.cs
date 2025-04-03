using Microsoft.AspNetCore.Mvc;

namespace CRUDdyWeather.Controllers
{
    public class OpenMetroController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
