using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using OSS.Common.Extention;
using OSS.PayCenter.ZFB.Pay;
using OSS.PayCenter.ZFB.Pay.Mos;

namespace OSS.PayCenter.Tests.ZFB
{
    [TestClass]
    public class ZPayTradeTests : ZPayBaseTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            var json = @"{ 'alipay_trade_precreate_response':{
                'code':'10000',
        'msg':'Success',
        'out_trade_no':'6823789339978248',
        'qr_code':'https://qr.alipay.com/bavh4wjlxf12tper3a'
    },
    'sign':'ERITJKEIJKJHKKKKKKKHJEREEEEEEEEEEE'
}";


            var resJsonObj = JObject.Parse(json);
            if (resJsonObj != null)
            {
                var res = resJsonObj["alipay_trade_precreate_response"].ToObject<ZAddPreTradeResp>();

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
