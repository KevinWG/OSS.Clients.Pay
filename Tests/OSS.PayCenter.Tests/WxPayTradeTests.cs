using Microsoft.VisualStudio.TestTools.UnitTesting;
using OSS.Common.Extention;
using OSS.PayCenter.WX;
using OSS.PayCenter.WX.Pay;
using OSS.PayCenter.WX.Pay.Mos;

namespace OSS.PayCenter.Tests
{
    [TestClass]
    public class WxPayTradeTests
    {
        private WxPayTradeApi m_Api=new WxPayTradeApi();
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
                NotifyUrl = "http://www.osscoder.com"
            };

        }

        [TestMethod]
        public void TestMethod1()
        {
            var order = new WxAddPayUniOrderReq();

            order.device_info = "WEB";
            order.body = "测试商品支付";
            order.openid = "sdfvsfdbf345678888fhngfbsdfbsdfb";

            order.out_trade_no = "2017022423560123";
            order.trade_type = "JSAPI";
            order.total_fee = 100;

            var res = m_Api.AddPayUniOrder(order).WaitResult();
            Assert.IsTrue(res.IsSuccess);
        }
    }
}
