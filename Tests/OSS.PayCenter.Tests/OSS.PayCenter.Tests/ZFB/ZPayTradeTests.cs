﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using OSS.Clients.Pay.Ali;
using OSS.Clients.Pay.Ali.Helpers;
using OSS.Clients.Pay.Ali.Pay;
using OSS.Clients.Pay.Ali.Pay.Mos;
using OSS.Common.BasicMos.Resp;
using OSS.Common.Extention;
using OSS.Tools.Log;

namespace OSS.Clients.Pay.Tests.ZFB
{
    [TestClass]
    public class ZPayTradeTests : ZPayBaseTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            var json =
                "{\"alipay_trade_precreate_response\":{\"code\":\"10000\",\"msg\":\"Success\",\"out_trade_no\":\"20170328125923\",\"qr_code\":\"https:\\/\\/qr.alipay.com\\/bax05555mjgtykjbuvi80078\"},\"sign\":\"Ym5gUE1alOl5wpMmCSr1Jnqw5O8Fb4+4Vi7E3FGnE2q0mNw/IH318a54G5xB9TMvdX4fRc9b+n11LQ8MmgvRo4ybObcFQX4Rg61LGeoKpc2szxj5YGQJbddRghKu06Cyz9d10BKjE4nPQ3JxCLl+WuoA+Vo+fHvHKuXisNlsZn2TOaORBhvrexSd6TpSx07RXkMoI1mH8KMS/uh74kL2x7urBKbu2yTpxJzgj5r2s64dcJvZKPOEDvVybvB6J5I7PTilFPMrDINc970KiWGEZNFIOp2DJ/AYqbsokvEIhuC00Q6MGYVs2hYVPZDOXF2uYO7hTrLO0YYhtAmiJlgp9Q==\"}";

            var resJsonObj = JObject.Parse(json);
            if (resJsonObj == null) return;

            var res = resJsonObj["alipay_trade_precreate_response"].ToObject<ZAddPreTradeResp>();
            if (!res.IsSuccess()) return;

            var sign = resJsonObj["sign"].ToString();
            var signContent = GetCehckSignContent("alipay_trade_precreate_response", json);

            CheckSign(signContent, sign, res);
            CheckSign(signContent, sign, res);
            CheckSign(signContent, sign, res);
        }

        private static string GetCehckSignContent(string respColumnName, string contentStr)
        {
            int startIndex = contentStr.IndexOf(respColumnName, StringComparison.Ordinal) + respColumnName.Length + 2;
            int endIndex = contentStr.LastIndexOf(',');
            var signContent = contentStr.Substring(startIndex, endIndex - startIndex);
            return signContent;
        }

        private readonly ZPayRsaHelper m_RsaAssist = new ZPayRsaHelper(config.AppPrivateKey, config.AppPublicKey,
         
            config.Charset);

        protected void CheckSign<T>(string signContent, string sign, T t)
            where T : Resp, new()
        {
            try
            {
                var checkSignRes = m_RsaAssist.CheckSign(signContent, sign);
                if (!checkSignRes)
                {
                    if (!string.IsNullOrEmpty(signContent) &&
                        signContent.Contains("\\/"))
                    {
                        signContent = signContent.Replace("\\/", "/");
                        // 如果验签不通过，转义字符后再次验签
                        checkSignRes = m_RsaAssist.CheckSign(signContent, sign);
                    }

                    if (checkSignRes) return;

                    t.ret = (int) RespTypes.SignError;
                    t.msg = "当前签名非法！";
                }

            }
            catch (Exception e)
            {
                t.ret = (int) RespTypes.InnerError;
                t.msg = "解密签名过程中出错，详情请查看日志";
                LogHelper.Info($"解密签名过程中出错，解密内容：{signContent}, 待验证签名：{sign},   错误信息：{e.Message}",
                    "CheckSign", ZPayConfigProvider.ModuleName);
#if DEBUG
                throw e;
#endif
            }
        }

        private ZPayTradeApi m_Api = new ZPayTradeApi(config);

        [TestMethod]
        public void AddPreTradeTest()
        {
            var payReq = new ZAddPreTradeReq()
            {
                out_trade_no = "20170328125923",
                body = "测试商品",
                subject = "测试",
                total_amount = 0.01M,

                notify_url = "http://你的接收地址"
            };
            var res = m_Api.AddPrePayTradeAsync(payReq).WaitResult();
            var result = res.IsSuccess();

        }
    }
}
