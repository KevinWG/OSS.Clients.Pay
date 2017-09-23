using Microsoft.AspNetCore.Mvc;
using OSS.Common.ComModels;
using OSS.Common.Plugs.LogPlug;
using OSS.PaySdk.Wx.Pay;
using OSS.PaySdk.Wx.Pay.Mos;
using System.IO;
using System.Threading.Tasks;

namespace OSS.PaySdk.Samples.Controllers
{
    public class WxPayController : BaseController
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

        private static readonly string _callBackDomain = "你的当前域名";
        private static readonly WxPayTradeApi _api = new WxPayTradeApi();

        public async Task<IActionResult> GetJsPayInfo(string orderId)
        {
            var order = new WxAddPayUniOrderReq
            {
                notify_url = string.Concat(_callBackDomain, "/wxpay/receive"),
                body = "OSSPay-测试商品",
                device_info = "WEB",
                openid = "oldRAw-Wu4eOD5CVPWeWVDOvhRbo",
                out_trade_no = orderId,

                spbill_create_ip = "114.242.25.208",
                total_fee = 1,
                trade_type = "JSAPI"
            };

            var orderRes = await _api.AddUniOrderAsync(order);
            if (!orderRes.IsSuccess()) return Json(orderRes);

            var jsPara = _api.GetJsClientParaResp(orderRes.prepay_id);
            return Json(jsPara);
        }
        public async Task<IActionResult> GetScanPayInfo(string orderId)
        {
            var order = new WxAddPayUniOrderReq
            {
                notify_url = string.Concat(_callBackDomain, "/wxpay/receive"),
                body = "OSSPay-测试商品",
                device_info = "WEB",
                openid = "oldRAw-Wu4eOD5CVPWeWVDOvhRbo",
                out_trade_no = orderId,

                spbill_create_ip = "114.242.25.208",
                total_fee = 1,
                trade_type = "NATIVE"
            };

            var orderRes = await _api.AddUniOrderAsync(order);
            return Json(orderRes);
        }

        public IActionResult receive()
        {
            string strPayResult;
            using (var streamReader = new StreamReader(Request.Body))
            {
                strPayResult = streamReader.ReadToEnd();
            }
            
            LogUtil.Info("支付结果通知：" + strPayResult);

            var returnXml = _api.GetCallBackReturnXml(new ResultMo());
            return Content(returnXml);
        }

        private static readonly WxPayRefundApi _refundApi = new WxPayRefundApi();
        public async Task<IActionResult> refund(string orderId)
        {
            var refundRes = await _refundApi.RefundOrderAsync(new WxPayRefundReq()
            {
                out_trade_no = orderId,
                total_fee = 1,
                refund_fee = 1,
                out_refund_no = orderId
            });

            return Json(refundRes);
        }
    }

}
