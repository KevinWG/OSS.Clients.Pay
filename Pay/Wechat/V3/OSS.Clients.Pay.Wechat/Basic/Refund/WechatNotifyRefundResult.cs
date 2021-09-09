
namespace OSS.Clients.Pay.Wechat.Basic
{
    public class NotifySPRefundResult:BaseRefundResult
    {
        /// <summary>   
        ///   服务商户号   string[1,32]
        ///   服务商户号，由微信支付生成并下发   。
        /// </summary>  
        public string sp_mchid { get; set; }

        /// <summary>   
        ///   子商户号   string[1,32]
        ///   子商户的商户号，由微信支付生成并下发。
        /// </summary>  
        public string sub_mchid { get; set; }

    }

    /// <summary>
    ///  通知退款结果
    /// </summary>
    public class WechatNotifyRefundResult: BaseRefundResult
    {
        /// <summary>   
        ///   直连商户号   string[1,32]
        ///   直连商户的商户号，由微信支付生成并下发。
        /// </summary>  
        public string mchid { get; set; }
    }


    public class BaseRefundResult
    {
        /// <summary>   
        ///   商户订单号   string[1,32]
        ///   返回的商户订单号
        /// </summary>  
        public string out_trade_no { get; set; }

        /// <summary>   
        ///   微信支付订单号   string[1,32]
        ///   微信支付订单号
        /// </summary>  
        public string transaction_id { get; set; }

        /// <summary>   
        ///   商户退款单号   string[1,64]
        ///   商户退款单号
        /// </summary>  
        public string out_refund_no { get; set; }

        /// <summary>   
        ///   微信支付退款单号   string[1,32]
        ///   微信退款单号
        /// </summary>  
        public string refund_id { get; set; }

        /// <summary>   
        ///   退款状态   string[1,16]
        ///   退款状态，枚举值：
        /// SUCCESS：退款成功
        /// CLOSE：退款关闭
        /// ABNORMAL：退款异常，退款到银行发现用户的卡作废或者冻结了，导致原路退款银行卡失败，可前往【服务商平台—>交易中心】，手动处理此笔退款
        /// </summary>  
        public string refund_status { get; set; }

        /// <summary>   
        ///   退款成功时间   string[1,64]
        ///   1、退款成功时间，遵循rfc3339标准格式，
        ///         格式为YYYY-MM-DDTHH:mm:ss+TIMEZONE，YYYY-MM-DD表示年月日，T出现在字符串中，表示time元素的开头，HH:mm:ss表示时分秒，
        ///         TIMEZONE表示时区（+08:00表示东八区时间，领先UTC   8小时，即北京时间）。
        ///         例如：2015-05-20T13:29:35+08:00表示，北京时间2015年5月20日13点29分35秒。
        /// 2、当退款状态为退款成功时返回此参数。
        /// </summary>  
        public string success_time { get; set; }

        /// <summary>   
        ///   退款入账账户   string[1,64]
        ///   取当前退款单的退款入账方。
        ///     退回银行卡：{银行名称}{卡类型}{卡尾号}
        ///     退回支付用户零钱:   支付用户零钱
        ///     退还商户:   商户基本账户/商户结算银行账户
        ///     退回支付用户零钱通：支付用户零钱通
        /// </summary>  
        public string user_received_account { get; set; }

        /// <summary>   
        ///   金额信息   object
        ///   金额信息
        /// </summary>  
        public RefundResultAmount amount { get; set; }

    }


    public class RefundResultAmount
    {
        /// <summary>   
        ///   订单金额   int
        ///   订单总金额，单位为分，只能为整数，详见支付金额
        /// </summary>  
        public int total { get; set; }

        /// <summary>   
        ///   退款金额   int
        ///   退款金额，币种的最小单位，只能为整数，不能超过原订单支付金额，如果有使用券，后台会按比例退。
        /// </summary>  
        public int refund { get; set; }

        /// <summary>   
        ///   用户支付金额   int
        ///   用户实际支付金额，单位为分，只能为整数，详见支付金额
        /// </summary>  
        public int payer_total { get; set; }

        /// <summary>   
        ///   用户退款金额   int
        ///   退款给用户的金额，不包含所有优惠券金额
        /// </summary>  
        public int payer_refund { get; set; }
    }
}
