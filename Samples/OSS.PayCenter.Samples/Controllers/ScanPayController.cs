using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OSS.Common.Extention;
using OSS.PayCenter.Samples.Models;
using OSS.PayCenter.ZFB.Pay;
using OSS.PayCenter.ZFB.Pay.Mos;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OSS.PayCenter.Samples.Controllers
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
            string orderNum = DateTime.Now.ToString("yyyyMMddHHmmss");

            var payReq = new ZAddPreTradeReq("http://test.pay.osscoder.com/base/ZCallBack");

            payReq.body = order.order_name;
            payReq.out_trade_no = orderNum;
            payReq.total_amount = order.order_price;
            payReq.subject = order.order_name;

            var res =await zPayApi.AddPreTradeAsync(payReq);
            return Json(res);
        }

    }
}
