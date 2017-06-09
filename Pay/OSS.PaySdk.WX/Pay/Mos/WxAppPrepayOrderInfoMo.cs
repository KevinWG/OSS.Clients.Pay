using System;
using System.Collections.Generic;
using OSS.Common.Extention;

namespace OSS.PaySdk.Wx.Pay.Mos
{
    /// <summary>
    /// 返回给App端的，直接可用的预付单数据
    /// </summary>
    public class WxAppPrepayOrderInfoMo
    {
        /// <summary>
        /// 使用统一下单接口的返回值，创建一个App立即可用的 <see cref="WxAppPrepayOrderInfoMo"/> 类的示例
        /// </summary>
        /// <param name="t"></param>
        /// <param name="wxapi"></param>
        public WxAppPrepayOrderInfoMo(WxAddPayUniOrderResp t, WxPayTradeApi wxapi)
        {
            appid = t.appid;
            partnerid = t.mch_id;
            prepayid = t.prepay_id;
            noncestr = t.nonce_str;
            timestamp = DateTime.Now.ToUtcSeconds().ToString();
            var dic = new SortedDictionary<string, object>()
                {
                    {"appid",appid},
                    {"partnerid",partnerid},
                    {"prepayid",prepayid},
                    {"noncestr",noncestr},
                    {"package",package},
                    {"timestamp",timestamp},
                };
            sign = wxapi.GetSign(dic);
        }

        public string appid { get; private set; }
        /// <summary>
        /// 也就是mchid
        /// </summary>
        public string partnerid { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public string prepayid { get; private set; }
        public string noncestr { get; private set; }
        public string timestamp { get; private set; } = DateTime.Now.ToUtcSeconds().ToString();

        /// <summary>
        /// 常量 "Sign=WXPay"
        /// </summary>
        public string package { get; private set; } = "Sign=WXPay";

        public string sign { get; private set; }

    }
}
