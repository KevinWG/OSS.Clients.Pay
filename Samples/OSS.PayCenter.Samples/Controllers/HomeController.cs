using System.Net.Http;
using Microsoft.AspNetCore.Mvc;

namespace OSS.PayCenter.Samples.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


        public IActionResult Error()
        {
            var handler=new HttpClientHandler();
            return View();
        }



    }
}
