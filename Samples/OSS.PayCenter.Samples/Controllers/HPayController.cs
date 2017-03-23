using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OSS.PayCenter.Samples.Controllers
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
        public IActionResult wx_pay(HPayMo order)
        {
            return Content("ceshi");
        }
    }


    public class HPayMo
    {
        public string openid { get; set; }
        public string order_name { get; set; }
        public decimal order_price { get; set; }
        public int pay_channel { get; set; }
    }
}
