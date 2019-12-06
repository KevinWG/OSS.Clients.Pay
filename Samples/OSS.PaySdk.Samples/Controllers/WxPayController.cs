using Microsoft.AspNetCore.Mvc;
using OSS.Common.ComModels;
using OSS.PaySdk.Wx.Pay;
using OSS.PaySdk.Wx.Pay.Mos;
using System.IO;
using System.Threading.Tasks;
using OSS.Common.Resp;

namespace OSS.PaySdk.Samples.Controllers
{
    public class WxPayController : BaseController
    {
        // GET: /<controller>/
        public IActionResult Index()
        {

            return View();
        }
        

        public WxPayController()
        {
           // 上下文配置设置方式
           // WxPayConfigProvider.SetContextConfig(new WxPayCenterConfig(){AppId = "XXXX"});
        }
        // 声明配置设置方式
        //private static readonly WxPayTradeApi _api = new WxPayTradeApi(new WxPayCenterConfig() { AppId = "XXX" });

        private static readonly string _callBackDomain = "你的当前域名";

        private static readonly WxPayTradeApi _api = new WxPayTradeApi();
        //  获取扫码支付的二维码信息
        public async Task<IActionResult> GetScanPayInfo(string orderId)
        {
            var order = GetUniorder(orderId, "NATIVE");

            var orderRes = await _api.AddUniOrderAsync(order);
            return Json(orderRes);
        }

        // 获取公号内js支付需要的信息唤起支付
        public async Task<IActionResult> GetJsPayInfo(string orderId)
        {
            var order = GetUniorder(orderId, "JSAPI");

            var orderRes = await _api.AddUniOrderAsync(order);
            if (!orderRes.IsSuccess()) return Json(orderRes);

            var jsPara = _api.GetJsClientParaResp(orderRes.prepay_id);
            return Json(jsPara);
        }

        private static WxAddPayUniOrderReq GetUniorder(string orderId,string tradeType)
        {
            return new WxAddPayUniOrderReq
            {
                notify_url = string.Concat(_callBackDomain, "/wxpay/receive"),
                body = "OSSPay-测试商品",
                device_info = "WEB",
                openid = "oldRAw-Wu4eOD5CVPWeWVDOvhRbo",
                out_trade_no = orderId,

                spbill_create_ip = "114.242.25.208",
                total_fee = 1,
                trade_type = tradeType
            };
        }
        //  支付结果接收
        public IActionResult receive()
        {
            string strPayResult;
            using (var streamReader = new StreamReader(Request.Body))
            {
                strPayResult = streamReader.ReadToEnd();
            }
            var wxPayRes = _api.DecryptPayResult(strPayResult);
            //  do something with wxPayRes
            var returnXml = _api.GetCallBackReturnXml(new Resp());
            return Content(returnXml);
        }

        //  退款示例
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
