using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OSS.Common.ComModels;
using OSS.PaySdk.Ali.Pay;
using OSS.PaySdk.Ali.Pay.Mos;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OSS.PaySdk.Samples.Controllers
{
    public class AliPayController : BaseController
    {
        private static string returnUrl = "你的域名/alipay/return_url";
        private static string receiveUrl = "你的域名/alipay/receive";

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult return_url()
        {
            return Content("支付成功");
        }

        private static readonly ZPayTradeApi _api = new ZPayTradeApi();

        [HttpPost]
        public IActionResult receive(ZPayCallBackResp pay)
        {
            var dics = Request.Form.ToDictionary(f => f.Key, f => f.Value.ToString());
            var res = _api.CheckCallBackSign(dics);
            if (res.IsSuccess())
            {
                // do something with res
            }
            return Content("success");
        }


        [HttpPost]
        public async Task<IActionResult> GetOfficialPayInfo([FromBody] ZAddOfficialTradeReq order)
        {
            var orderRes = await _api.GetOfficialTradeAsync(order);
            return Json(orderRes);
        }

        [HttpPost]
        public IActionResult GetPagePayInfo(string orderId)
        {
            var order = new ZAddPageTradeReq()
            {
                out_trade_no = orderId,
                total_amount = 0.01M,
                subject = "测试页面订单",
                passback_params = "test",
                return_url = returnUrl,
                notify_url = receiveUrl
            };
            var orderRes = _api.GetPageTradeContent(order);
            return Json(orderRes);
        }

        [HttpPost]
        public IActionResult GetWapPayInfo(string orderId)
        {
            var order = new ZAddWapTradeReq()
            {
                out_trade_no = orderId,
                total_amount = 0.01M,
                subject = "测试页面订单",

                return_url = returnUrl,
                notify_url = receiveUrl
            };
            var orderRes = _api.GetWapTradeContent(order);
            return Json(orderRes);
        }

        [HttpPost]
        public async Task<IActionResult> GetScanPayInfo(string orderId)
        {
            var order = new ZAddPreTradeReq()
            {
                out_trade_no = orderId,
                total_amount = 0.01M,
                subject = "测试扫码订单",
                notify_url = receiveUrl
            };
            
            var orderRes = await _api.AddPrePayTradeAsync(order);
            return Json(orderRes);
        }


        private static readonly ZPayRefundApi _refundApi = new ZPayRefundApi();
        /// <summary>
        /// 退款接口
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> refund([FromBody]ZPayRefundReq req)
        {
            var refundRes = await _refundApi.RefunPayAsync(req);

            return Json(refundRes);
        }

    }
}
