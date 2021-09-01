using System;

namespace OSS.Clients.Pay.Wechat.Basic
{
    public class RefundReq:BasePostReq<RefundReq, RefundResp>
    {
        public override string GetApiPath()
        {
            return "/v3/refund/domestic/refunds";
        }

        protected override void PrepareCommonBodyPara()
        {
            if (IsSpPartnerReq)
            {
                AddBodyPara("sub_mchid", sub_mch_id);
            }
            //else
            //{
            //    AddBodyPara("appid", pay_config.app_id);
            //    AddBodyPara("mchid", pay_config.mch_id);
            //}
        }
        
        /// <summary>   
        ///   微信支付订单号  
        ///   二选一   原支付交易对应的微信订单号
        /// </summary>  
        public string transaction_id { get; set; }

        /// <summary>   
        ///   商户订单号   
        ///     二选一  原支付交易对应的商户订单号
        /// </summary>  
        public string out_trade_no { get; set; }

        /// <summary>   
        ///   商户退款单号
        ///   是   商户系统内部的退款单号，商户系统内部唯一，只能是数字、大小写字母_-|*@   ，同一退款单号多次请求只退一笔。示例值：1217752501201407033233368018
        /// </summary>  
        public string out_refund_no { get; set; }

        /// <summary>   
        ///   可选 退款原因   
        ///   否   若商户传入，会在下发给用户的退款消息中体现退款原因示例值：商品已售完
        /// </summary>  
        public string reason { get; set; }

        /// <summary>   
        ///   退款结果回调url 
        ///   否   异步接收微信支付退款结果通知的回调地址，通知url必须为外网可访问的url，不能携带参数。
        ///     如果参数中传了notify_url，则商户平台上配置的回调地址将不会生效，优先回调当前传的这个地址。
        /// </summary>  
        public string notify_url { get; set; }

        /// <summary>   
        ///   可选
        ///    退款资金来源   string[1,32]
        ///   若传递此参数则使用对应的资金账户退款，否则默认使用未结算资金退款（仅对老资金流商户适用）
        ///     枚举值：
        ///     AVAILABLE：可用余额账户
        /// </summary>  
        public string funds_account { get; set; }

        /// <summary>   
        ///   -金额信息   object
        ///   订单金额信息
        /// </summary>  
        public RefundAmount amount { get; set; }

        /// <summary>   
        ///   退款金额   int
        ///   退款金额，币种的最小单位，只能为整数，不能超过原订单支付金额。
        /// </summary>  
        public int refund { get; set; }



        protected override void PrepareBodyPara()
        {
            if (string.IsNullOrEmpty(transaction_id) && string.IsNullOrEmpty(out_trade_no))
            {
                throw new ArgumentException($"{nameof(transaction_id)} 和 {nameof(out_trade_no)} 不能同时为空");
            }

            AddBodyPara("out_trade_no", out_trade_no);
            AddBodyPara("transaction_id", transaction_id);

            AddBodyPara("out_refund_no", out_refund_no);
            AddBodyPara("reason", reason);
            AddBodyPara("notify_url", notify_url);
            AddBodyPara("funds_account", funds_account);

            AddBodyPara("amount", amount);
            AddBodyPara("refund", refund);
        }
    }


    public class RefundAmount
    {
        /// <summary>   
        /// 必填  退款金额   int
        ///   退款金额，币种的最小单位，只能为整数，不能超过原订单支付金额。
        /// </summary>  
        public int refund { get; set; }

        /// <summary>   
        ///  可选
        ///  退款出资账户及金额   
        ///  退款需要从指定账户出资时，传递此参数指定出资金额（币种的最小单位，只能为整数）。
        ///  同时指定多个账户出资退款的使用场景需要满足以下条件：
        ///      1、未开通退款支出分离产品功能；
        ///      2、订单属于分账订单，且分账处于待分账或分账中状态。
        ///  参数传递需要满足条件：
        ///      1、基本账户可用余额出资金额与基本账户不可用余额出资金额之和等于退款金额；
        ///      2、账户类型不能重复。上述任一条件不满足将返回错误
        /// </summary>  
        public RefundAmountSource[] from { get; set; }

        /// <summary>   
        ///  必填 原订单金额   int
        ///   原支付交易的订单总金额，币种的最小单位，只能为整数。
        /// </summary>  
        public int total { get; set; }

        /// <summary>   
        ///   必填 退款币种   string[1,16]
        ///   符合ISO   4217标准的三位字母代码，目前只支持人民币：CNY。
        /// </summary>  
        public string currency { get; set; } = "CNY";
    }

    public class RefundAmountSource
    {
        /// <summary>   
        ///   出资账户类型
        ///   下面枚举值多选一。
        ///     枚举值：
        ///         AVAILABLE:可用余额
        ///         UNAVAILABLE:不可用余额
        /// </summary>  
        public string account { get; set; }

        /// <summary>   
        ///   出资金额   int
        ///   对应账户出资金额
        /// </summary>  
        public int amount { get; set; }
    }
    
    public class RefundResp : BaseResp
    {

        /// <summary>   
        ///   微信支付退款单号   string[1,
        ///   是   微信支付退款单号
        /// </summary>  
        public string refund_id { get; set; }

        /// <summary>   
        ///   商户退款单号   string[1,
        ///   是   商户系统内部的退款单号，商户系统内部唯一，只能是数字、大小写字母_-|*@   ，同一退款单号多次请求只退一笔。
        /// </summary>  
        public string out_refund_no { get; set; }

        /// <summary>   
        ///   微信支付订单号   string[1,
        ///   是   微信支付交易订单号
        /// </summary>  
        public string transaction_id { get; set; }

        /// <summary>   
        ///   商户订单号   string[1,
        ///   是   原支付交易对应的商户订单号
        /// </summary>  
        public string out_trade_no { get; set; }

        /// <summary>   
        ///   退款渠道 
        ///   是
        /// 枚举值：ORIGINAL：原路退款
        /// BALANCE：退回到余额
        /// OTHER_BALANCE：原账户异常退到其他余额账户
        /// OTHER_BANKCARD：原银行卡异常退到其他银行卡
        /// </summary>  
        public string channel { get; set; }

        /// <summary>   
        ///   退款入账账户   string[1,
        ///   是   取当前退款单的退款入账方，有以下几种情况：
        /// 1）退回银行卡：{银行名称}{卡类型}{卡尾号}
        /// 2）退回支付用户零钱:支付用户零钱
        /// 3）退还商户:商户基本账户商户结算银行账户
        /// 4）退回支付用户零钱通:支付用户零钱通
        /// </summary>  
        public string user_received_account { get; set; }

        /// <summary>   
        ///   退款成功时间   string[1,
        ///   否   退款成功时间，当退款状态为退款成功时有返回。
        /// </summary>  
        public string success_time { get; set; }

        /// <summary>   
        ///   退款创建时间   string[1,
        ///   是   退款受理时间
        /// </summary>  
        public string create_time { get; set; }

        /// <summary>   
        ///   退款状态   string[1,
        ///   是   退款到银行发现用户的卡作废或者冻结了，导致原路退款银行卡失败，可前往商户平台-交易中心，手动处理此笔退款。
        /// 枚举值：SUCCESS：退款成功
        /// CLOSED：退款关闭
        /// PROCESSING：退款处理中
        /// ABNORMAL：退款异常
        /// </summary>  
        public string status { get; set; }

        /// <summary>   
        ///   资金账户   string[1,
        ///   否   退款所使用资金对应的资金账户类型
        /// 枚举值：
        /// UNSETTLED   :   未结算资金
        /// AVAILABLE   :   可用余额
        /// UNAVAILABLE   :   不可用余额
        /// OPERATION   :   运营户
        /// BASIC   :   基本账户（含可用余额和不可用余额）
        /// </summary>  
        public string funds_account { get; set; }

        /// <summary>   
        ///   +金额信息   object
        ///   金额详细信息
        /// </summary>  
        public RefundRespAmount amount { get; set; }

        /// <summary>   
        ///   +优惠退款信息   array
        ///   优惠退款信息
        /// </summary>  
        public RefundPromotionDetail[] promotion_detail { get; set; }

        /// <summary>   
        ///   商品列表   array
        ///   优惠商品发生退款时返回商品信息
        /// </summary>  
        public RefundGoodsDetail[] goods_detail { get; set; }

    }

    public class RefundRespAmount: RefundAmount
    {
        /// <summary>   
        ///   用户支付金额   int
        ///   现金支付金额，单位为分，只能为整数
        /// </summary>  
        public int payer_total { get; set; }

        /// <summary>   
        ///   用户退款金额   int
        ///   退款给用户的金额，不包含所有优惠券金额
        /// </summary>  
        public int payer_refund { get; set; }

        /// <summary>   
        ///   应结退款金额   int
        ///   去掉非充值代金券退款金额后的退款金额，单位为分，退款金额=申请退款金额-非充值代金券退款金额，退款金额<=申请退款金额
        /// </summary>  
        public int settlement_refund { get; set; }

        /// <summary>   
        ///   应结订单金额   int
        ///   应结订单金额=订单金额-免充值代金券金额，应结订单金额<=订单金额，单位为分
        /// </summary>  
        public int settlement_total { get; set; }

        /// <summary>   
        ///   优惠退款金额   int
        ///   优惠退款金额<=退款金额，退款金额-代金券或立减优惠退款金额为现金，说明详见代金券或立减优惠，单位为分
        /// </summary>  
        public int discount_refund { get; set; }
    }


    public class RefundPromotionDetail
    {
        /// <summary>   
        ///   券ID   string[1,32]
        ///   券或者立减优惠id
        /// </summary>  
        public string promotion_id { get; set; }

        /// <summary>   
        ///   优惠范围   string[1,32]
        ///   枚举值：GLOBAL：全场代金券SINGLE：单品优惠
        /// </summary>  
        public string scope { get; set; }

        /// <summary>   
        ///   优惠类型   string[1,32]
        ///   枚举值：COUPON：代金券，需要走结算资金的充值型代金券DISCOUNT：优惠券，不走结算资金的免充值型优惠券
        /// </summary>  
        public string type { get; set; }

        /// <summary>   
        ///   优惠券面额   int
        ///   用户享受优惠的金额（优惠券面额=微信出资金额+商家出资金额+其他出资方金额   ），单位为分
        /// </summary>  
        public int amount { get; set; }

        /// <summary>   
        ///   优惠退款金额   int
        ///   优惠退款金额<=退款金额，退款金额-代金券或立减优惠退款金额为用户支付的现金，说明详见代金券或立减优惠，单位为分
        /// </summary>  
        public int refund_amount { get; set; }
    }


    public class RefundGoodsDetail
    {

        /// <summary>   
        ///   商户侧商品编码   string[1,
        ///   是   由半角的大小写字母、数字、中划线、下划线中的一种或几种组成
        /// </summary>  
        public string merchant_goods_id { get; set; }

        /// <summary>   
        ///   微信侧商品编码   string[1,
        ///   否   微信支付定义的统一商品编号（没有可不传）
        /// </summary>  
        public string wechatpay_goods_id { get; set; }

        /// <summary>   
        ///   商品名称   string[1,
        ///   否   商品的实际名称
        /// </summary>  
        public string goods_name { get; set; }

        /// <summary>   
        ///   商品单价   int
        ///   商品单价金额，单位为分
        /// </summary>  
        public int unit_price { get; set; }

        /// <summary>   
        ///   商品退款金额   int
        ///   商品退款金额，单位为分
        /// </summary>  
        public int refund_amount { get; set; }

        /// <summary>   
        ///   商品退货数量   int
        ///   单品的退款数量
        /// </summary>  
        public int refund_quantity { get; set; }
    }
}
