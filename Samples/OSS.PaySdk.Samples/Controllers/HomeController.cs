using Microsoft.AspNetCore.Mvc;

namespace OSS.Clients.Pay.Samples.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}