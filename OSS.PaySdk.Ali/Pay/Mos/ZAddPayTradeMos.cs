
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

namespace OSS.PaySdk.Ali.Pay.Mos
{
    /// <summary>
    ///   统一下单交易支付接口实体（商户扫码收款 - 商家扫用户二维码、读取声波发起支付）
    /// </summary>
    public class ZAddPayTradeReq : ZAddPreTradeReq
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
        ///    String 可选 长度(32)  销售产品码
        /// </summary>  
        public string product_code { get; set; }

        /// <summary>   
        ///    String 可选 长度(28)  买家的支付宝用户id，如果为空，会从传入了码值信息中获取买家ID
        /// </summary>  
        public string buyer_id { get; set; }
    }
  
    /// <summary>
    ///   条码支付响应实体
    /// </summary>
    public class ZAddPayTradeResp: ZTradeInfoBaseResp
    {
    
        /// <summary>   
        ///    Date 必填 长度(32)  交易支付时间 15:45:57
        /// </summary>  
        public string gmt_payment { get; set; }
        
        /// <summary>   
        ///    Price 选填 长度(11)  支付宝卡余额
        /// </summary>  
        public decimal card_balance { get; set; }
    }
    
    /// <summary>
    ///   返回的交易信息基础类
    /// </summary>
    public class ZTradeInfoBaseResp : ZPayBaseResp
    {
        /// <summary>   
        ///    String 必填 长度(64)  支付宝交易号
        /// </summary>  
        public string trade_no { get; set; }

        /// <summary>   
        ///    String 必填 长度(64)  商户订单号
        /// </summary>  
        public string out_trade_no { get; set; }


        /// <summary>   
        ///    String 必填 长度(100)  买家支付宝账号
        /// </summary>  
        public string buyer_logon_id { get; set; }

        /// <summary>   
        ///    Price 必填 长度(11)  交易金额
        /// </summary>  
        public decimal total_amount { get; set; }
        
        /// <summary>   
        ///    String 必填 长度(11)  实收金额
        /// </summary>  
        public string receipt_amount { get; set; }

        /// <summary>   
        ///    Price 选填 长度(11)  买家付款的金额
        /// </summary>  
        public decimal buyer_pay_amount { get; set; }

        /// <summary>   
        ///    Price 选填 长度(11)  使用积分宝付款的金额
        /// </summary>  
        public decimal point_amount { get; set; }

        /// <summary>   
        ///    Price 选填 长度(11)  交易中可给用户开具发票的金额
        /// </summary>  
        public decimal invoice_amount { get; set; }

        /// <summary>   
        ///    TradeFundBill[] 必填 长度(-)  交易支付使用的资金渠道
        /// </summary>  
        public List<ZTradeFundBillMo> fund_bill_list { get; set; }
        /// <summary>   
        ///    String 选填 长度(512)  发生支付交易的商户门店名称
        /// </summary>  
        public string store_name { get; set; }

        /// <summary>   
        ///    String 必填 长度(28)  买家在支付宝的用户id
        /// </summary>  
        public string buyer_user_id { get; set; }

        /// <summary>   
        ///    String 必填 长度(-)  本次交易支付所使用的单品券优惠的商品优惠信息
        /// </summary>  
        public string discount_goods_detail { get; set; }
    }

    /// <summary>
    /// 交易支付使用的资金渠道
    /// </summary>
    public class ZTradeFundBillMo
    {
        /// <summary>   
        ///    String 必填 长度(32)  交易使用的资金渠道，详见 ALIPAYACCOUNT
        /// </summary>  
        public string fund_channel { get; set; }

        /// <summary>   
        ///    Price 选填 长度(-)  该支付工具类型所使用的金额
        /// </summary>  
        public string amount { get; set; }

        /// <summary>   
        ///    Price 选填 长度(11)  渠道实际付款金额
        /// </summary>  
        public string real_amount { get; set; }
    }

    #region  基类部分

    #endregion

    /// <summary>
    ///  支付宝异步通知回调
    /// </summary>
    public class ZPayCallBackResp
    {
        /// <summary>   
        ///    通知时间 Date 长度(是)  通知的发送时间。格式为yyyy-MM-dd 2015-14-27 15:45:58
        /// </summary>  
        public string notify_time { get; set; }

        /// <summary>   
        ///    通知类型 String(64) 长度(是)  通知的类型
        /// </summary>  
        public string notify_type { get; set; }

        /// <summary>   
        ///    通知校验ID String(128) 长度(是)  通知校验ID
        /// </summary>  
        public string notify_id { get; set; }

        /// <summary>   
        ///    签名类型 String(10) 长度(是)  商户生成签名字符串所使用的签名算法类型，目前支持RSA2和RSA，推荐使用RSA2
        /// </summary>  
        public string sign_type { get; set; }

        /// <summary>   
        ///    签名 String(256) 长度(是)  请参考异步返回结果的验签
        /// </summary>  
        public string sign { get; set; }

        /// <summary>   
        ///    支付宝交易号 String(64) 长度(是)  支付宝交易凭证号
        /// </summary>  
        public string trade_no { get; set; }

        /// <summary>   
        ///    开发者的app_id String(32) 长度(是)  支付宝分配给开发者的应用Id
        /// </summary>  
        public string app_id { get; set; }

        /// <summary>   
        ///    商户订单号 String(64) 长度(是)  原支付请求的商户订单号
        /// </summary>  
        public string out_trade_no { get; set; }

        /// <summary>   
        ///    商户业务号 String(64) 长度(否)  商户业务ID，主要是退款通知中返回退款申请的流水号
        /// </summary>  
        public string out_biz_no { get; set; }

        /// <summary>   
        ///    买家支付宝用户号 String(16) 长度(否)  买家支付宝账号对应的支付宝唯一用户号。以2088开头的纯16位数字
        /// </summary>  
        public string buyer_id { get; set; }

        /// <summary>   
        ///    买家支付宝账号 String(100) 长度(否)  买家支付宝账号
        /// </summary>  
        public string buyer_logon_id { get; set; }

        /// <summary>   
        ///    卖家支付宝用户号 String(30) 长度(否)  卖家支付宝用户号
        /// </summary>  
        public string seller_id { get; set; }

        /// <summary>   
        ///    卖家支付宝账号 String(100) 长度(否)  卖家支付宝账号
        /// </summary>  
        public string seller_email { get; set; }

        /// <summary>   
        ///    交易状态 String(32) 长度(否)  交易目前所处的状态
        /// </summary>  
        public string trade_status { get; set; }

        /// <summary>   
        ///    订单金额 Number(9,2) 长度(否)  本次交易支付的订单金额，单位为人民币（元）
        /// </summary>  
        public string total_amount { get; set; }

        /// <summary>   
        ///    实收金额 Number(9,2) 长度(否)  商家在交易中实际收到的款项，单位为元
        /// </summary>  
        public decimal receipt_amount { get; set; }

        /// <summary>   
        ///    开票金额 Number(9,2) 长度(否)  用户在交易中支付的可开发票的金额
        /// </summary>  
        public decimal invoice_amount { get; set; }

        /// <summary>   
        ///    付款金额 Number(9,2) 长度(否)  用户在交易中支付的金额
        /// </summary>  
        public decimal buyer_pay_amount { get; set; }

        /// <summary>   
        ///    集分宝金额 Number(9,2) 长度(否)  使用集分宝支付的金额
        /// </summary>  
        public decimal point_amount { get; set; }

        /// <summary>   
        ///    总退款金额 Number(9,2) 长度(否)  退款通知中，返回总退款金额，单位为元，支持两位小数
        /// </summary>  
        public decimal refund_fee { get; set; }

        /// <summary>   
        ///    实际退款金额 Number(9,2) 长度(否)  商户实际退款给用户的金额，单位为元，支持两位小数
        /// </summary>  
        public string send_back_fee { get; set; }

        /// <summary>   
        ///    订单标题 String(256) 长度(否)  商品的标题/交易标题/订单标题/订单关键字等，是请求时对应的参数，原样通知回来
        /// </summary>  
        public string subject { get; set; }

        /// <summary>   
        ///    商品描述 String(400) 长度(否)  该订单的备注、描述、明细等。对应请求时的body参数，原样通知回来
        /// </summary>  
        public string body { get; set; }

        /// <summary>   
        ///    交易创建时间 Date 长度(否)  该笔交易创建的时间。格式为yyyy-MM-dd 2015-04-27 15:45:57
        /// </summary>  
        public string gmt_create { get; set; }

        /// <summary>   
        ///    交易付款时间 Date 长度(否)  该笔交易的买家付款时间。格式为yyyy-MM-dd 2015-04-27 15:45:57
        /// </summary>  
        public string gmt_payment { get; set; }

        /// <summary>   
        ///    交易退款时间 Date 长度(否)  该笔交易的退款时间。格式为yyyy-MM-dd 2015-04-28 15:45:57.320
        /// </summary>  
        public string gmt_refund { get; set; }

        /// <summary>   
        ///    交易结束时间 Date 长度(否)  该笔交易结束时间。格式为yyyy-MM-dd 2015-04-29 15:45:57
        /// </summary>  
        public string gmt_close { get; set; }

        /// <summary>   
        ///    支付金额信息 String(512) 长度(否)  支付成功的各个渠道金额信息，详见资金明细信息说明
        /// </summary>  
        public string fund_bill_list { get; set; }
    }
}
