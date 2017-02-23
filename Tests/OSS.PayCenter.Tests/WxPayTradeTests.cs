using System;
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
                AppId = "wxd930ea5d5a258f4f",
                MchId = "10000100",
                Key = "192006250b4c09247ec02edce69f6a2d"
            };

        }

        [TestMethod]
        public void TestMethod1()
        {
            var order = new WxAddPayTradeOrderReq();

            order.device_info = "WEB";
            order.body = "测试商品支付";
            order.notify_url = "http://www.osscoder.com";
            order.openid = "sdfvsfdbf345678888fhngfbsdfbsdfb";

            order.out_trade_no = "2017022423560123";
            order.trade_type = "JSAPI";
            order.total_fee = 100;

            var res = m_Api.AddPayTradeOrder(order).WaitResult();
            Assert.IsTrue(res.IsSuccess);
        }
    }
}
