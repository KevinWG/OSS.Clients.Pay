using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using OSS.PayCenter.ZFB.Pay.Mos;

namespace OSS.PayCenter.Tests.ZFB
{
    [TestClass]
    public class ZPayTradeTests
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


        [TestMethod]
        public void AddPreTradeTest()
        {
         
        }
    }
}
