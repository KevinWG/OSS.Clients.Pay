
#region Copyright (C) 2017 Kevin (OSS开源作坊) 公众号：osscoder

/***************************************************************************
*　　	文件功能描述：支付宝支付模快 —— 统一下单交易支付接口实体
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2017-3-28
*       
*****************************************************************************/

#endregion

using System.Collections.Generic;

namespace OSS.PayCenter.ZFB.Pay.Mos
{
    /// <summary>
    ///   统一下单交易支付接口实体
    /// </summary>
    public class ZAddPayTradeReq : ZAddTradeBaseReq
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="notifyUrl">通知回调地址</param>
        public ZAddPayTradeReq(string notifyUrl) : base(notifyUrl)
        {
        }

        /// <summary>   
        ///    String 必须 长度(32)  支付场景 声波支付，取值：wave_code bar_code,wave_code
        /// </summary>  
        public string scene { get; set; }

        /// <summary>   
        ///    String 必须 长度(32)  支付授权码
        /// </summary>  
        public string auth_code { get; set; }

        /// <summary>   
        ///    String 可选 长度(64)  预授权号，预授权转交易请求中传入
        /// </summary>  
        public string auth_no { get; set; }

        /// <summary>   
        ///    String 可选 长度(32)  销售产品码
        /// </summary>  
        public string product_code { get; set; }

        /// <summary>   
        ///    String 可选 长度(28)  买家的支付宝用户id，如果为空，会从传入了码值信息中获取买家ID
        /// </summary>  
        public string buyer_id { get; set; }

        /// <summary>   
        ///    GoodsDetail [] 长度(可选)  -
        /// </summary>  
        public ZTradeGoodDetailMo goods_detail { get; set; }

        /// <summary>   
        ///    ExtUserInfo 可选 长度(-)  外部指定买家
        /// </summary>  
        public ZTradeExtUserMo ext_user_info { get; set; }

    }

    /// <summary>
    ///  统一收单的商品详情实体
    /// </summary>
    public class ZTradeGoodDetailMo
    {
        /// <summary>   
        ///    String 可选 长度(28)  商户操作员编号
        /// </summary>  
        public string operator_id { get; set; }

        /// <summary>   
        ///    String 可选 长度(32)  商户门店编号
        /// </summary>  
        public string store_id { get; set; }

        /// <summary>   
        ///    String 可选 长度(32)  商户机具终端编号
        /// </summary>  
        public string terminal_id { get; set; }

        /// <summary>   
        ///    String 可选 长度(32)  支付宝的店铺编号
        /// </summary>  
        public string alipay_store_id { get; set; }
    }

    /// <summary>
    /// 	外部指定买家
    /// </summary>
    public class ZTradeExtUserMo
    {
        /// <summary>   
        ///    String 可选 长度(16)  姓名
        /// </summary>  
        public string name { get; set; }

        /// <summary>   
        ///    String 可选 长度(20)  手机号
        /// </summary>  
        public string mobile { get; set; }

        /// <summary>   
        ///    String 可选 长度(32)  身份证：IDENTITY_CARD、护照：PASSPORT、军官证：OFFICER_CARD、士兵证：SOLDIER_CARD、户口本：HOKOU等。如有其它类型需要支持，请与蚂蚁金服工作人员联系。
        /// </summary>  
        public string cert_type { get; set; }

        /// <summary>   
        ///    String 可选 长度(64)  证件号
        /// </summary>  
        public string cert_no { get; set; }
    }


    #region  基类部分

    /// <summary>
    ///  下单基类部分
    /// </summary>
    public class ZAddTradeBaseReq : ZPayBaseReq
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="notifyUrl">通知回调地址</param>
        public ZAddTradeBaseReq(string notifyUrl)
        {
            notify_url = notifyUrl;
        }

        /// <summary>   
        ///    String 必须 长度(64)  商户订单号,64个字符以内、可包含字母、数字、下划线；需保证在商户端不重复
        /// </summary>  
        public string out_trade_no { get; set; }


        /// <summary>   
        ///    String 可选 长度(28)  如果该值为空，则默认为商户签约账号对应的支付宝用户ID
        /// </summary>  
        public string seller_id { get; set; }

        /// <summary>   
        ///    Price 必须 长度(11)  订单总金额，单位为元，精确到小数点后两位，取值范围[0.01,100000000]。 如果同时传入了【可打折金额】，【不可打折金额】，【订单总金额】三者，则必须满足如下条件：【订单总金额】=【可打折金额】+【不可打折金额】 88.88
        /// </summary>  
        public decimal total_amount { get; set; }

        /// <summary>   
        ///    Price 可选 长度(11)  参与优惠计算的金额，单位为元，精确到小数点后两位，取值范围[0.01,100000000]。 8.88
        /// </summary>  
        public decimal discountable_amount { get; set; }

        /// <summary>   
        ///    Price 可选 长度(11)  不参与优惠计算的金额，单位为元，精确到小数点后两位，取值范围[0.01,100000000]。如果该值未传入，但传入了【订单总金额】和【可打折金额】，则该值默认为【订单总金额】-【可打折金额】
        /// </summary>  
        public decimal undiscountable_amount { get; set; }

        /// <summary>   
        ///    String 必须 长度(256)  订单标题 16G
        /// </summary>  
        public string subject { get; set; }

        /// <summary>   
        ///    String 可选 长度(128)  订单描述 16G
        /// </summary>  
        public string body { get; set; }

        /// <summary>   
        ///    String 可选 长度(28)  商户操作员编号
        /// </summary>  
        public string operator_id { get; set; }

        /// <summary>   
        ///    String 可选 长度(32)  商户门店编号
        /// </summary>  
        public string store_id { get; set; }

        /// <summary>   
        ///    String 可选 长度(32)  商户机具终端编号
        /// </summary>  
        public string terminal_id { get; set; }

        /// <summary>   
        ///    String 可选 长度(32)  支付宝的店铺编号
        /// </summary>  
        public string alipay_store_id { get; set; }

        /// <summary>   
        ///    ExtendParams 可选 长度(-)  业务扩展参数
        /// </summary>  
        public ZTradeExtendParaMo extend_params { get; set; }

        /// <summary>   
        ///    String 可选 长度(6)  该笔订单允许的最晚付款时间，逾期将关闭交易。取值范围：1m～15d。m-分钟，h-小时，d-天，1c-当天（1c-当天的情况下，无论交易何时创建，都在0点关闭）。 如 1.5h，可转换为 90m 90m
        /// </summary>  
        public string timeout_express { get; set; }

        /// <summary>   
        ///    RoyaltyInfo 可选 长度(-)  描述分账信息，Json格式，其它说明详见分账说明
        /// </summary>  
        public ZTradeRoyaltyMo royalty_info { get; set; }

        /// <summary>   
        ///    SubMerchant 可选 长度(-)  二级商户信息,当前只对特殊银行机构特定场景下使用此字段
        /// </summary>  
        public ZTradeSubMerchantMo sub_merchant { get; set; }

    }

    /// <summary>
    ///  统一收单业务扩展参数
    /// </summary>
    public class ZTradeExtendParaMo
    {
        /// <summary>   
        ///    String 可选 长度(64)  系统商编号 2088511833207846
        /// </summary>  
        public string sys_service_provider_id { get; set; }

        /// <summary>   
        ///    String 可选 长度(5)  使用花呗分期要进行的分期数
        /// </summary>  
        public string hb_fq_num { get; set; }

        /// <summary>   
        ///    String 可选 长度(3)  使用花呗分期需要卖家承担的手续费比例的百分值，传入100代表100%
        /// </summary>  
        public string hb_fq_seller_percent { get; set; }

    }

    /// <summary>
    /// 描述分账信息，Json格式，其它说明详见分账说明
    /// </summary>
    public class ZTradeRoyaltyMo
    {
        /// <summary>   
        ///    String 可选 长度(150)  分账类型目前只支持传入ROYALTY（普通分账类型）
        /// </summary>  
        public string royalty_type { get; set; }

        /// <summary>
        /// 分账明细的信息，可以描述多条分账指令，json数组。
        /// </summary>
        public List<ZTradeRoyaltyDetailMo> royalty_detail_infos { get; set; }
    }

    /// <summary>
    /// 分账明细的信息，可以描述多条分账指令，
    /// </summary>
    public class ZTradeRoyaltyDetailMo
    {
        /// <summary>   
        ///    Number 可选 长度(9)  分账序列号，表示分账执行的顺序，必须为正整数
        /// </summary>  
        public uint serial_no { get; set; }

        /// <summary>   
        ///    String 可选 长度(24)  接受分账金额的账户类型： userId：支付宝账号对应的支付宝唯一用户号。  bankIndex：分账到银行账户的银行编号。目前暂时只支持分账到一个银行编号。 storeId：分账到门店对应的银行卡编号。 默认值为userId。 userId
        /// </summary>  
        public string trans_in_type { get; set; }

        /// <summary>   
        ///    String 必填 长度(32)  分账批次号 目前需要和转入账号类型为bankIndex配合使用。 123
        /// </summary>  
        public string batch_no { get; set; }

        /// <summary>   
        ///    String 可选 长度(64)  商户分账的外部关联号，用于关联到每一笔分账信息，商户需保证其唯一性。 20131124001
        /// </summary>  
        public string out_relation_id { get; set; }

        /// <summary>   
        ///    String 必填 长度(24)  要分账的账户类型。 默认值为userId。 userId
        /// </summary>  
        public string trans_out_type { get; set; }

        /// <summary>   
        ///    String 必填 长度(16)  如果转出账号类型为userId，本参数为要分账的支付宝账号对应的支付宝唯一用户号。以2088开头的纯16位数字。
        /// </summary>  
        public string trans_out { get; set; }

        /// <summary>   
        ///    String 必填 长度(28)  如果转入账号类型为userId，本参数为接受分账金额的支付宝账号对应的支付宝唯一用户号。以2088开头的纯16位数字。 如果转入账号类型为bankIndex，本参数为28位的银行编号（商户和支付宝签约时确定）。 如果转入账号类型为storeId，本参数为商户的门店ID。 2088101126708402
        /// </summary>  
        public string trans_in { get; set; }

        /// <summary>   
        ///    Number 必填 长度(9)  分账的金额，单位为元
        /// </summary>  
        public decimal amount { get; set; }

        /// <summary>   
        ///    String 可选 长度(1000)  分账描述信息
        /// </summary>  
        public string desc { get; set; }

        /// <summary>   
        ///    String 可选 长度(3)  分账的比例，值为20代表按20%的比例分账
        /// </summary>  
        public string amount_percentage { get; set; }
    }

    /// <summary>
    /// 二级商户信息,当前只对特殊银行机构特定场景下使用此字段
    /// </summary>
    public class ZTradeSubMerchantMo
    {
        /// <summary>   
        ///    String 必填 长度(11)  二级商户的支付宝id
        /// </summary>  
        public string merchant_id { get; set; }
    }

    #endregion
}
