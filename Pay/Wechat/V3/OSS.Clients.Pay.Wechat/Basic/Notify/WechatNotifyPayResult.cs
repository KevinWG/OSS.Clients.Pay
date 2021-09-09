namespace OSS.Clients.Pay.Wechat.Basic
{
    /// <summary>
    ///  普通商户支付回调解密后结果
    /// </summary>
    public class WechatNotifyPayResult: WechatPayResultResp
    {
        /// <summary>   
        ///   应用ID   string[1,32]
        ///   直连商户申请的公众号或移动应用appid。
        /// </summary>  
        public string appid { get; set; }

        /// <summary>   
        ///   商户号   string[1,32]
        ///   商户的商户号，由微信支付生成并下发。
        /// </summary>  
        public string mchid { get; set; }
    }

    public class WechatPayResultResp:BaseResp
    {
        /// <summary>   
        ///   商户订单号   string[6,32]
        ///   商户系统内部订单号，只能是数字、大小写字母_-*且在同一个商户号下唯一。
        /// </summary>  
        public string out_trade_no { get; set; }

        /// <summary>   
        ///   微信支付订单号   string[1,32]
        ///   微信支付系统生成的订单号。
        /// </summary>  
        public string transaction_id { get; set; }

        /// <summary>   
        ///   交易类型   string[1,16]
        ///   交易类型，枚举值：JSAPI：公众号支付
        /// NATIVE：扫码支付
        /// APP：APP支付
        /// MICROPAY：付款码支付
        /// MWEB：H5支付
        /// FACEPAY：刷脸支付         
        /// </summary>  
        public string trade_type { get; set; }

        /// <summary>   
        ///   交易状态   string[1,32]
        ///   交易状态，枚举值：SUCCESS：支付成功
        /// REFUND：转入退款
        /// NOTPAY：未支付
        /// CLOSED：已关闭
        /// REVOKED：已撤销（付款码支付）
        /// USERPAYING：用户支付中（付款码支付）
        /// PAYERROR：支付失败(其他原因，如银行返回失败)
        /// </summary>  
        public string trade_state { get; set; }

        /// <summary>   
        ///   交易状态描述   string[1,256]
        ///   交易状态描述
        /// </summary>  
        public string trade_state_desc { get; set; }

        /// <summary>   
        ///   付款银行   string[1,16]
        ///   银行类型，采用字符串类型的银行标识。银行标识请参考《银行类型对照表》示例值：CMC
        /// </summary>  
        public string bank_type { get; set; }

        /// <summary>   
        ///   附加数据   string[1,128]
        ///   附加数据，在查询API和支付通知中原样返回，可作为自定义参数使用
        /// </summary>  
        public string attach { get; set; }

        /// <summary>   
        ///   支付完成时间   string[1,64]
        ///   支付完成时间，遵循rfc3339标准格式，格式为YYYY-MM-DDTHH:mm:ss+TIMEZONE，YYYY-MM-DD表示年月日，T出现在字符串中，表示time元素的开头，HH:mm:ss表示时分秒，TIMEZONE表示时区（+08:00表示东八区时间，领先UTC   8小时，即北京时间）。例如：2015-05-20T13:29:35+08:00表示，北京时间2015年5月20日   13点29分35秒。
        /// </summary>  
        public string success_time { get; set; }

        /// <summary>   
        ///   +支付者   object
        ///   支付者信息
        /// </summary>  
        public PayResultPayer payer { get; set; }

        /// <summary>   
        ///   +订单金额   object
        ///   订单金额信息
        /// </summary>  
        public PayResultAmount amount { get; set; }

        /// <summary>   
        ///   +场景信息   object
        ///   支付场景信息描述
        /// </summary>  
        public PayResultSenceInfo scene_info { get; set; }

        /// <summary>   
        ///   +优惠功能   array
        ///   优惠功能，享受优惠时返回该字段。
        /// </summary>  
        public PayResultPromitionData[] promotion_detail { get; set; }
    }

    public class PayResultPayer
    {
        /// <summary>   
        ///   用户标识   string[1,128]
        ///   用户在直连商户appid下的唯一标识。
        /// </summary>  
        public string openid { get; set; }

    }
    
    public class PayResultAmount
    {
        /// <summary>   
        ///   总金额   int
        ///   订单总金额，单位为分。
        /// </summary>  
        public int total { get; set; }

        /// <summary>   
        ///   用户支付金额   int
        ///   用户支付金额，单位为分。
        /// </summary>  
        public int payer_total { get; set; }

        /// <summary>   
        ///   货币类型   string[1,16]
        ///   CNY：人民币，境内商户号仅支持人民币。
        /// </summary>  
        public string currency { get; set; }

        /// <summary>   
        ///   用户支付币种   string[1,16]
        ///   用户支付币种
        /// </summary>  
        public string payer_currency { get; set; }
    }
    
    public class PayResultSenceInfo
    {

        /// <summary>   
        ///   商户端设备号   string[1,32]
        ///   终端设备号（门店号或收银设备ID）。
        /// </summary>  
        public string device_id { get; set; }
    }
    
    public class PayResultPromitionData
    {
        /// <summary>   
        ///   券ID   string[1,32]
        /// </summary>  
        public string coupon_id { get; set; }

        /// <summary>   
        ///   优惠名称   string[1,64]
        /// </summary>  
        public string name { get; set; }

        /// <summary>   
        ///   优惠范围   string[1,32]
        ///   GLOBAL：全场代金券         SINGLE：单品优惠         
        /// </summary>  
        public string scope { get; set; }

        /// <summary>   
        ///   优惠类型   string[1,32]
        ///   CASH：充值 NOCASH：预充值
        /// </summary>  
        public string type { get; set; }

        /// <summary>   
        ///   优惠券面额   int
        ///   优惠券面额
        /// </summary>  
        public int amount { get; set; }

        /// <summary>   
        ///   活动ID   string[1,32]
        ///   活动ID
        /// </summary>  
        public string stock_id { get; set; }

        /// <summary>   
        ///   微信出资   int
        ///   微信出资，单位为分
        /// </summary>  
        public int wechatpay_contribute { get; set; }

        /// <summary>   
        ///   商户出资   int
        ///   商户出资，单位为分
        /// </summary>  
        public int merchant_contribute { get; set; }

        /// <summary>   
        ///   其他出资   int
        ///   其他出资，单位为分
        /// </summary>  
        public int other_contribute { get; set; }

        /// <summary>   
        ///   优惠币种   string[1,16]
        ///   CNY：人民币，境内商户号仅支持人民币。
        /// </summary>  
        public string currency { get; set; }

        /// <summary>   
        ///   +单品列表   array
        ///   单品列表信息
        /// </summary>  
        public PromotionGoodsDetail[] goods_detail { get; set; }
    }

    public class PromotionGoodsDetail
    {
        /// <summary>   
        ///   商品编码  
        /// </summary>  
        public string goods_id { get; set; }

        /// <summary>   
        ///   商品数量   int
        ///   用户购买的数量
        /// </summary>  
        public int quantity { get; set; }

        /// <summary>   
        ///   商品单价   int
        ///   商品单价，单位为分
        /// </summary>  
        public int unit_price { get; set; }

        /// <summary>   
        ///   商品优惠金额   int
        ///   商品优惠金额
        /// </summary>  
        public int discount_amount { get; set; }

        /// <summary>   
        ///   商品备注   string[1,128]
        ///   商品备注信息
        /// </summary>  
        public string goods_remark { get; set; }

    }
}
