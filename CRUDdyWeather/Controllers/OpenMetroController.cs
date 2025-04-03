using Microsoft.AspNetCore.Mvc;
using CRUDdyWeather.Models;

namespace CRUDdyWeather.Controllers
{
    
    public class OpenMetroController : Controller
    {
        private readonly ClientAPI _clientAPI;

        public OpenMetroController()
        {
            _clientAPI = new ClientAPI();
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
