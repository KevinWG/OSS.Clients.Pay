using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using OSS.Clients.Pay.WX.Pay;
using OSS.Clients.Pay.WX.Pay.Mos;
using OSS.Common.BasicMos.Resp;

namespace OSS.Clients.Pay.Samples.Controllers
{
    public class WXPayController : BaseController
    {
        // GET: /<controller>/
        public IActionResult Index()
        {

            return View();
        }
        

        public WXPayController()
        {
           // 上下文配置设置方式
           // WXPayConfigProvider.SetContextConfig(new WXPayCenterConfig(){AppId = "XXXX"});
        }
        // 声明配置设置方式
        //private static readonly WXPayTradeApi _api = new WXPayTradeApi(new WXPayCenterConfig() { AppId = "XXX" });

        private static readonly string _callBackDomain = "你的当前域名";

        private static readonly WXPayTradeApi _api = new WXPayTradeApi();
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

        private static WXAddPayUniOrderReq GetUniorder(string orderId,string tradeType)
        {
            return new WXAddPayUniOrderReq
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
        private static readonly WXPayRefundApi _refundApi = new WXPayRefundApi();
        public async Task<IActionResult> refund(string orderId)
        {
            var refundRes = await _refundApi.RefundOrderAsync(new WXPayRefundReq()
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
