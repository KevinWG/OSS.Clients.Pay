using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OSS.PaySdk.Ali.Pay.Mos;
using OSS.PaySdk.Samples.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OSS.PaySdk.Samples.Controllers
{
    public class ScanPayController : BaseController
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ZPay(PayOrderMo order)
        {
            var orderNum = DateTime.Now.ToString("yyyyMMddHHmmss");
            var payReq = new ZAddPreTradeReq
            {
                notify_url = "http://test.pay.osscoder.com/base/ZCallBack",
                body = order.order_name,
                out_trade_no = orderNum,
                total_amount = order.order_price,
                subject = order.order_name
            };
            var res =await zPayApi.AddPrePayTradeAsync(payReq);
            return Json(res);
        }

    }
}
