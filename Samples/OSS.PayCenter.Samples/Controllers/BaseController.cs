using System.Linq;
using Microsoft.AspNetCore.Mvc;
using OSS.Common.Plugs.LogPlug;
using OSS.PaySdk.Ali;
using OSS.PaySdk.Ali.Pay;
using OSS.PaySdk.Ali.Pay.Mos;
using OSS.PaySdk.Wx;

namespace OSS.PaySdk.Samples.Controllers
{
    public class BaseController : Controller
    {
        protected static WxPayCenterConfig WxPayConfig { get; set; }=new WxPayCenterConfig()
            {
                AppSource = "11",//  用户自定义填写，主要处理
                AppId = "wx9b46f71cd4597f6e",
                MchId = "1233410002",
                Key = "e10adc3849ba56abbe56e056f20f883e",
                AppSecret = "51c56b886b5be869567dd389b3e5d1d6",

                CertPassword = "1233410002",
                CertPath = "cert/apiclient_cert.p12",
                NotifyUrl = "pay.sample.osscoder.com",
                // 设置证书方式
                SetCertificata = (handler, cert) =>
                {
                    handler.ServerCertificateCustomValidationCallback = (msg, c, chain, sslErrors) => true;
                    handler.ClientCertificates.Add(cert);
                }
            };
        
        private static string privateKey =
       "MIIEowIBAAKCAQEAtZTRAPYO4keKVcH8BazPyjH6se4kllBmaUVAuUN/TetxwCRSo90h+v85MsgFLrFS5AtAS2keomyohve15lbCItNlzUB254ndE8mGfZz21oMSI7qoBJyJyebWam4mEN08zP2SNpvB6MBNGUPDgfFLFuLl1olQxxZWQBv6/8x8VE1+YKtq/avrAK/CCnhxg/eXC9G+ivHp2ZLozYa8mB+iMgOSIi9klzBoVSkcx0nHKLh7J09hLgcnqbW9e5hARaUP5ZX+9+c2nDEF9i/cwfty8/TO91NVypj9uT5ofHZmOw89BG24vfDcJYxbvLEFTM6ts62xBYkSYrIpsaguUCEXcQIDAQABAoIBAQCmwWY3tcDxebXHEAD8zTl+fOu33/XlvSxv3NOV0DDnRCQQysrCBeCg+yZyRlLMELkJCHQhTQZ/L76NRIveXyqmhPFmz57jAC7xbw996FqfoXtVKKQIqCG6M64Ry9pVfnfJ4c0XLW+k6oVjgGHZwditye49WZm/W/oVohyeMhKz73FLhHuVcqSoZDqB3W+KeCxQTsqK6cG7ScNLlHg3A1wzWPoA1vxI8TjI9CJcfzT2AHYrwDPnF1PXMN217GUgdplIRfH01PYNA5y+oo5c4llRMp+vpBesFyEK/Ihxqdy73OHHhxqKz3MVUYSFSp8YbTPFcHPo37EOFvBkL+uRI9RhAoGBAOwskctMHjteVl2fPQRiNuW8zqW0qVpmTp6958QvSRjLcb2358FzQebnlb9gRYTTh4u+qT+2s9H3nN21qzbW//3DZxh6AOxLtK0I942TQw46hYriv5h93p8s0dyOsurZDZQ3h+//dz3TKw9mwufY4v/5gXIvZTF7mbYlPiCaEPidAoGBAMTTCV7uLFCfwDQdrtNk+dbL+YO6cFpp1a3myd8wRuzwB1SmXmoEM+pABFcYl8FPoUtePkvVcyfDr8IdUUdGL9Zk2NfMBgsDoqE1kO47JVu3qCx7HvehlN/UDe9BVa5QsBz4Psz7Gq5jX9pJs2X6jO+NJy2WVMSxztR/XyyYb4/lAoGAARwkg0QUUEsdNMtuyfp8L5A3mGfE8/vPGsfhsvZV7ZvnKPEYUbxs7tzfw0p8iSdFV87JlNcS+UWkoxYbe+J+yX6FmYPZRUMF0lpcb3nlssdroT93jwvoY+8d/V9eT+Q9QBkStnoI4W2Aok3lVJmcV3+gsByw6Q6fpk9+f0C6G7ECgYBzt+vDf15YjIPRYD5AFRb6nXP2aa/SyHSTyKOZb+XTOg/lSecqh4wKRlcG6fOW+P1ItYEEztrkXx2/j8MQl4hakXIX9eD7qsh75WVvWyDMur7KYBzF1J8VtbP3nb8FPHU+owkxNNTsAIsDaN9rrpIb6S5GlCMEfdGvh69qJbk5rQKBgHJpR+5zfzgwE2tMnFWIJp1noMr7KnOyqjebFEvRPkAIP5U46OE7D3A1CSrVRFBkDBY+0xkTg5XFrIX3F9xnL2dR1Ru2QZ0wQ+fBHqLEjoZkERsLqGpxEOqau0BMo1SD5nRPp9z7eL535SsOER5MeDhMfgHfAhSCacJo2DWMnUBw";

        private static string publicKey =
            "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEArEGMX+VF5JE9OJjYU8Pc4UdYfEGN3hQDjOTREXCYaLOC7tzC2IfTa9vvJyxpx0IBknsSR++VJXiW7EddRmSIcesydiC5XvaWOB1vsLJ8xxWPGyB9L6f4/aoUrd8s9dsv0l5U3CyvIT4FzJF3Z8SyV7vfObwrwCVq/ELrN2CtN1J4bgvKFaZglzslNVIKR3HTTpOq3XJvKZ/dFHLQdw7+Un44InoLj21Z5Fu17M+J3rAAbzJD28infsG6NMBrz7WtbrlJyDacOerOJIPjYeEniNXd3FOUwx1qRgoRq+Z9xLYIZvhjPz2k4iopI1pboPPnrjvwJ5uTwqREJchluNlEiQIDAQAB";

        protected static ZPayCenterConfig ZPayConfig { get; set; } = new ZPayCenterConfig()
        {
            AppSource = "1",
            AppId = "2016080300153582",
            AppPrivateKey = privateKey,
            AppPublicKey = publicKey
        };

        protected static ZPayTradeApi zPayApi = new ZPayTradeApi(ZPayConfig);

        [HttpPost]
        public IActionResult ZCallBack(ZPayCallBackResp resp)
        {
            var dics = Request.Form.ToDictionary(f => f.Key, f => f.Value.ToString());
            LogUtil.Info($"回调内容：{string.Join("&",dics)}");

            var res = zPayApi.CheckCallBackSign(dics);
            LogUtil.Info($"支付状态：{res.ret}, 消息：{res.message}");
            
            return Content("success");
        }
    }
}