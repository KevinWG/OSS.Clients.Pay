#region Copyright (C) 2021 Kevin (OSS开源实验室) 公众号：osscore

/***************************************************************************
*　　	文件功能描述：微信支付模快 —— 微信支付静态变量
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2021-7-24
*       
*****************************************************************************/

#endregion

namespace OSS.Clients.Pay.Wechat.Helpers
{
    /// <summary>
    /// 微信支付静态变量
    /// </summary>
    public static class WechatConstKeys
    {
        /// <summary>
        /// 请求Id
        /// </summary>
        public const string RequestID = "Request-ID";

        /// <summary>
        /// 随机码
        /// </summary>
        public const string WechatpayNonce = "Wechatpay-Nonce";

        /// <summary>
        /// 签名
        /// </summary>
        public const string WechatpaySignature = "Wechatpay-Signature";

        /// <summary>
        /// 时间戳
        /// </summary>
        public const string WechatpayTimestamp = "Wechatpay-Timestamp";
        
        /// <summary>
        ///  序列号
        /// </summary>
        public const string WechatpaySerial = "Wechatpay-Serial";
    }
}
