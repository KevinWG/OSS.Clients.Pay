using Microsoft.AspNetCore.Mvc;
using OSS.PaySdk.Samples.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OSS.PaySdk.Samples.Controllers
{
    public class HPayController : BaseController
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            //var config = new WxPayCenterConfig();
            //config.SetCertificata = (handler, cert) =>
            //{
            //    handler.ServerCertificateCustomValidationCallback = (msg, c, chain, sslErrors) => true;
            //    handler.ClientCertificates.Add(cert);
            //};
            return View();
        }

        [HttpPost]
        public IActionResult wx_pay(PayOrderMo order)
        {
            return Content("ceshi");
        }
    }

}
