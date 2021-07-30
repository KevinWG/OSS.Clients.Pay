using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using OSS.Clients.Pay.Wechat;
using OSS.Clients.Pay.Wechat.Basic;
using OSS.Common.BasicMos.Resp;
using OSS.Common.Extension;

namespace OSS.Clients.Pay.Samples.Controllers
{
    public class WechatController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }
        
        //  获取扫码支付的二维码信息
        public async Task<IActionResult> GetScanPayInfo(string orderId)
        {
            var nResp = await new NativePayReq()
                {
                    total        = 1,
                    description  = "测试商品",
                    out_trade_no = orderId,
                    notify_url   = "http://你的回调域名/Wechat/ReceivePayResult" // 接收示例在下方
                }
                // .SetContextConfig(new WechatPayConfig(){})   // 可以设置当前上下文的配置信息，方便多租户使用
                //.AddOptionalBodyPara("attach","附加数据")  // 添加可选参数
                .SendAsync();
            return Json(nResp);
        }

        [HttpPost]
        public async Task<IActionResult> ReceivePayResult()
        {
            if (!Request.Headers.TryGetValue("Wechatpay-Nonce", out var nonce)
                || !Request.Headers.TryGetValue("Wechatpay-Signature", out var signature)
                || !Request.Headers.TryGetValue("Wechatpay-Timestamp", out var timestamp)
                || !Request.Headers.TryGetValue("Wechatpay-Serial", out var serial))
            {
                return Content("非合法的微信通知请求!");
            }
            
            string strContent;
            using (var reader = new StreamReader(Request.Body))
            {
                strContent = await reader.ReadToEndAsync();
            }

            var res = await WechatPayHelper.Verify(WechatPayHelper.pay_config, signature.ToString(), serial.ToString(),
                nonce.ToString(), timestamp.ToString().ToInt64(),
                strContent);

            // 签名正确
            if (res.IsSuccess())
            {
                var encryptResult = JsonSerializer.Deserialize<NotifyPayEncryptResult>(strContent);
                // 获取真实的结果
                var result = encryptResult?.resource.DecrytToPayResult(WechatPayHelper.pay_config.api_v3_key);
                
                //  do something big
            }
            return new JsonResult(new {code = "SUCCESS"});
        }


    }

}
