#pragma warning disable 8618
namespace OSS.Clients.Pay.Wechat.Basic
{
    /// <summary>
    ///  服务商支付回调解密后结果
    /// </summary>
    public class NotifySPPayResult : BaseNotifyPayResult
    {
        /// <summary>   
        ///   服务商应用ID   string[1,32]
        ///   服务商申请的公众号或移动应用appid。
        /// </summary>  
        public string sp_appid { get; set; }

        /// <summary>   
        ///   服务商户号   string[1,32]
        ///   服务商户号，由微信支付生成并下发
        /// </summary>  
        public string sp_mchid { get; set; }

        /// <summary>   
        ///   子商户应用ID   string[1,32]
        ///   子商户申请的公众号或移动应用appid。
        /// </summary>  
        public string sub_appid { get; set; }

        /// <summary>   
        ///   子商户号   string[1,32]
        ///   子商户的商户号，由微信支付生成并下发。
        /// </summary>  
        public string sub_mchid { get; set; }
    }
}