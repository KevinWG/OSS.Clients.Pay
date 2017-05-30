using System.Collections.Concurrent;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OSS.Common.ComModels;
using OSS.Common.Extention;
using OSS.PaySdk.Wx;
using OSS.PaySdk.Wx.Pay;
using OSS.PaySdk.Wx.Pay.Mos;

namespace OSS.PaySdk.Tests.WX
{
    [TestClass]
    public class WxPayTradeTests
    {
        // 声明配置
        private static WxPayCenterConfig config= new WxPayCenterConfig()
        {
            AppSource = "11",
            AppId = "wx2428e34e0e7dc6ef",
            MchId = "1233410002",
            Key = "e10adc3849ba56abbe56e056f20f883e",
            AppSecret = "51c56b886b5be869567dd389b3e5d1d6",

            CertPassword = "1233410002",
            CertPath = "cert/apiclient_cert.p12",
            NotifyUrl = "http://www.osscoder.com",
            // 设置证书附加请求方式
            SetCertificata = (handler, cert) =>
            {
                handler.ServerCertificateCustomValidationCallback = (msg, c, chain, sslErrors) => true;
                handler.ClientCertificates.Add(cert);
            }
        };
        //  调用示例
        private static WxPayTradeApi m_Api=new WxPayTradeApi(config);
        

        static WxPayTradeTests()
        {
            WxPayBaseApi.DefaultConfig = new WxPayCenterConfig()
            {
                AppSource = "11",
                AppId = "wx2428e34e0e7dc6ef",
                MchId = "1233410002",
                Key = "e10adc3849ba56abbe56e056f20f883e",
                AppSecret = "51c56b886b5be869567dd389b3e5d1d6",

                CertPassword = "1233410002",
                CertPath = "cert/apiclient_cert.p12",
                NotifyUrl = "http://www.osscoder.com",
                // 设置证书方式
                SetCertificata = (handler, cert) =>
                {
                    handler.ServerCertificateCustomValidationCallback = (msg, c, chain, sslErrors) => true;
                    handler.ClientCertificates.Add(cert);
                }
            };

        }

        [TestMethod]
        public void AddUniOrderAsyncTest()
        {
            var order = new WxAddPayUniOrderReq();

            order.device_info = "WEB";
            order.body = "测试商品支付";
            order.openid = "sdfvsfdbf345678888fhngfbsdfbsdfb";

            order.out_trade_no = "2017022423560123";
            order.trade_type = "JSAPI";
            order.total_fee = 100;

            var res = m_Api.AddUniOrderAsync(order).WaitResult();
            Assert.IsTrue(res.IsSuccess());
        }




        [TestMethod]
        public void DicTest()
        {
            ConcurrentDictionary<string,string> dics=new ConcurrentDictionary<string, string>();
            dics["key"] = "111";
        }


        [TestMethod]
        public void RedListTest()
        {
            string res = @"<xml>
<return_code><![CDATA[SUCCESS]]></return_code>
<return_msg><![CDATA[OK]]></return_msg>
<result_code><![CDATA[SUCCESS]]></result_code>
<err_code><![CDATA[SUCCESS]]></err_code>
<err_code_des><![CDATA[OK]]></err_code_des>
<mch_billno><![CDATA[9010080799701411170000046603]]></mch_billno>
<mch_id><![CDATA[11475856]]></mch_id>
<detail_id><![CDATA[10000417012016080830956240040]]></detail_id>
<status><![CDATA[RECEIVED]]></status>
<send_type><![CDATA[ACTIVITY]]></send_type>
<hb_type><![CDATA[NORMAL]]></hb_type>
<total_num>1</total_num>
<total_amount>100</total_amount>
<send_time><![CDATA[2016-08-08 21:49:22]]></send_time>
<hblist>
<hbinfo>
<openid><![CDATA[oHkLxtzmyHXX6FW_cAWo_orTSRXs]]></openid>
<amount>100</amount>
<rcv_time><![CDATA[2016-08-08 21:49:46]]></rcv_time>
</hbinfo>
<hbinfo>
<openid><![CDATA[oHkLxtzmyHXX6FW_cAWo_orTSRXs]]></openid>
<amount>100</amount>
<rcv_time><![CDATA[2016-08-08 21:49:46]]></rcv_time>
</hbinfo>
</hblist>
</xml>";
            //XmlDocument resultXml = null;
            //var dics = XmlDicHelper.ChangXmlToDir(res, ref resultXml);

            //WxPayQueryRedResp t = new WxPayQueryRedResp();
            //t.RespXml = resultXml;
            //t.FromResContent(dics);
        }


    }
}
