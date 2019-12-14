using System.Collections.Concurrent;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OSS.Clients.Pay.WX;
using OSS.Clients.Pay.WX.Pay;
using OSS.Clients.Pay.WX.Pay.Mos;
using OSS.Common.Resp;

namespace OSS.Clients.Pay.Tests.WX
{
    [TestClass]
    public class WXPayTradeTests
    {
        // 声明配置
        private static WXPayCenterConfig config= new WXPayCenterConfig()
        {
            AppId = "wx2428e34e0e7dc6ef",
            MchId = "1233410002",
            Key = "e10adc3849ba56abbe56e056f20f883e",
            //AppSecret = "51c56b886b5be869567dd389b3e5d1d6",

            CertPassword = "1233410002",
            CertPath = "cert/apiclient_cert.p12"
        };
        //  调用示例
        private static WXPayTradeApi m_Api=new WXPayTradeApi(config);
        

        [TestMethod]
        public async Task AddUniOrderAsyncTest()
        {
            var order = new WXAddPayUniOrderReq();

            order.device_info = "WEB";
            order.body = "测试商品支付";
            order.openid = "sdfvsfdbf345678888fhngfbsdfbsdfb";

            order.out_trade_no = "2017022423560123";
            order.trade_type = "JSAPI";
            order.total_fee = 100;

            order.spbill_create_ip = "127.0.0.1";
           

            var res =await m_Api.AddUniOrderAsync(order);
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

            //WXPayQueryRedResp t = new WXPayQueryRedResp();
            //t.RespXml = resultXml;
            //t.FromResContent(dics);
        }


    }
}
