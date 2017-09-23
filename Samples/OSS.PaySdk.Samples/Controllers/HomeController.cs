using Microsoft.AspNetCore.Mvc;

namespace Univ.CompPay.Sample.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}