using Microsoft.AspNetCore.Mvc;

namespace OSS.PaySdk.Samples.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}