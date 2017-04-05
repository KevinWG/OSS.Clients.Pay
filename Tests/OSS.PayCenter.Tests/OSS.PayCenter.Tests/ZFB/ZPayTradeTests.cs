using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using OSS.Common.ComModels;
using OSS.Common.ComModels.Enums;
using OSS.Common.Extention;
using OSS.Common.Modules;
using OSS.Common.Modules.LogModule;
using OSS.PayCenter.ZFB.Pay;
using OSS.PayCenter.ZFB.Pay.Mos;
using OSS.PayCenter.ZFB.SysTools;

namespace OSS.PayCenter.Tests.ZFB
{
    [TestClass]
    public class ZPayTradeTests : ZPayBaseTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            var json = "{\"alipay_trade_precreate_response\":{\"code\":\"10000\",\"msg\":\"Success\",\"out_trade_no\":\"20170328125923\",\"qr_code\":\"https:\\/\\/qr.alipay.com\\/bax05555mjgtykjbuvi80078\"},\"sign\":\"Ym5gUE1alOl5wpMmCSr1Jnqw5O8Fb4+4Vi7E3FGnE2q0mNw/IH318a54G5xB9TMvdX4fRc9b+n11LQ8MmgvRo4ybObcFQX4Rg61LGeoKpc2szxj5YGQJbddRghKu06Cyz9d10BKjE4nPQ3JxCLl+WuoA+Vo+fHvHKuXisNlsZn2TOaORBhvrexSd6TpSx07RXkMoI1mH8KMS/uh74kL2x7urBKbu2yTpxJzgj5r2s64dcJvZKPOEDvVybvB6J5I7PTilFPMrDINc970KiWGEZNFIOp2DJ/AYqbsokvEIhuC00Q6MGYVs2hYVPZDOXF2uYO7hTrLO0YYhtAmiJlgp9Q==\"}";
            
            var resJsonObj = JObject.Parse(json);
            if (resJsonObj != null)
            {
                var res = resJsonObj["alipay_trade_precreate_response"].ToObject<ZAddPreTradeResp>();
       
                if (res.IsSuccess)
                {
                    var sign = resJsonObj["sign"].ToString();
                    var signContent = GetCehckSignContent("alipay_trade_precreate_response", json);

                    CheckSign(signContent, sign, res);
                }
            }
        }

        private static string GetCehckSignContent(string respColumnName, string contentStr)
        {
            int startIndex = contentStr.IndexOf(respColumnName, StringComparison.Ordinal) + respColumnName.Length + 2;
            int endIndex = contentStr.LastIndexOf(',');
            var signContent = contentStr.Substring(startIndex, endIndex - startIndex);
            return signContent;
        }
        protected void CheckSign<T>(string signContent, string sign, T t, string signType = null)
        where T : ResultMo, new()
        {
            try
            {
                if (string.IsNullOrEmpty(signType))
                    signType = config.SignType;

                var checkSignRes = ZPayRsaAssist.RSACheckContent(signContent, sign, config.AppPublicKey,
                    config.Charset, config.SignType);
                if (!checkSignRes)
                {
                    if (!string.IsNullOrEmpty(signContent) &&
                        signContent.Contains("\\/"))
                    {
                        signContent = signContent.Replace("\\/", "/");
                        // 如果验签不通过，转义字符后再次验签
                        checkSignRes = ZPayRsaAssist.RSACheckContent(signContent, sign,
                            config.AppPublicKey, config.Charset, signType);
                    }

                    if (checkSignRes) return;

                    t.Ret = (int)ResultTypes.UnAuthorize;
                    t.Message = "当前签名非法！";
                }

            }
            catch (Exception e)
            {
                t.Ret = (int)ResultTypes.InnerError;
                t.Message = "解密签名过程中出错，详情请查看日志";
                LogUtil.Info($"解密签名过程中出错，解密内容：{signContent}, 待验证签名：{sign}, 签名类型：{signType},  错误信息：{e.Message}",
                    "CheckSign", ModuleNames.PayCenter);
#if DEBUG
                throw e;
#endif
            }
        }
        private ZPayTradeApi m_Api=new ZPayTradeApi(config);

        [TestMethod]
        public void AddPreTradeTest()
        {
            var payReq = new ZAddPreTradeReq("http://pay.sample.osscoder.com");

            payReq.out_trade_no = "20170328125923";
            payReq.body = "测试商品";
            payReq.subject = "测试";
            payReq.total_amount = 0.01M;

            var res= m_Api.AddPreTrade(payReq).WaitResult();
           var result=  res.IsSuccess;

        }
    }
}
