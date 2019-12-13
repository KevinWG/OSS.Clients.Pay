#region Copyright (C) 2017 Kevin (OSS开源作坊) 公众号：osscore

/***************************************************************************
*　　	文件功能描述：支付宝支付模快 —— 交易相关实体
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2017-4-3
*       
*****************************************************************************/

#endregion

using System.Collections.Generic;

namespace OSS.PaySdk.Ali.Pay.Mos
{

    #region  取消交易相关实体

    /// <summary>
    ///  取消交易请求实体
    /// </summary>
    public class ZPayCancelReq:ZPayBaseReq
    {
        /// <summary>   
        ///    String 特殊可选 长度(64)  原支付请求的商户订单号,和支付宝交易号不能同时为空
        /// </summary>  
        public string out_trade_no { get; set; }

        /// <summary>   
        ///    String 特殊可选 长度(64)  支付宝交易号，和商户订单号不能同时为空
        /// </summary>  
        public string trade_no { get; set; }
    }

    /// <summary>
    /// 取消交易响应实体
    /// </summary>
    public class ZPayCancelResp : ZPayBaseResp
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
        ///    String 必填 长度(1)  是否需要重试
        /// </summary>  
        public string retry_flag { get; set; }

        /// <summary>   
        ///    String 必填 长度(10)  本次撤销触发的交易动作 refund：产生了退款
        /// </summary>  
        public string action { get; set; }
    }

    #endregion


    #region  退款相关实体

    /// <summary>
    ///  退款请求实体
    /// </summary>
    public class ZPayRefundReq : ZPayBaseReq
    {
        /// <summary>   
        ///    String 特殊可选 长度(64)  订单支付时传入的商户订单号,不能和trade_no同时为空。
        /// </summary>  
        public string out_trade_no { get; set; }

        /// <summary>   
        ///    String 特殊可选 长度(64)  支付宝交易号，和商户订单号不能同时为空
        /// </summary>  
        public string trade_no { get; set; }

        /// <summary>   
        ///    Price 必须 长度(9)  需要退款的金额，该金额不能大于订单金额,单位为元，支持两位小数
        /// </summary>  
        public decimal refund_amount { get; set; }

        /// <summary>   
        ///    String 可选 长度(256)  退款的原因说明
        /// </summary>  
        public string refund_reason { get; set; }

        /// <summary>   
        ///    String 可选 长度(64)  标识一次退款请求，同一笔交易多次退款需要保证唯一，如需部分退款，则此参数必传。
        /// </summary>  
        public string out_request_no { get; set; }

        /// <summary>   
        ///    String 可选 长度(30)  商户的操作员编号
        /// </summary>  
        public string operator_id { get; set; }

        /// <summary>   
        ///    String 可选 长度(32)  商户的门店编号
        /// </summary>  
        public string store_id { get; set; }

        /// <summary>   
        ///    String 可选 长度(32)  商户的终端编号
        /// </summary>  
        public string terminal_id { get; set; }
    }
    /// <summary>
    /// 退款响应实体
    /// </summary>
    public class ZPayRefundResp : ZPayBaseResp
    {

        /// <summary>   
        ///    String 必填 长度(64)  支付宝交易号 2013112011001004330000121536
        /// </summary>  
        public string trade_no { get; set; }

        /// <summary>   
        ///    String 必填 长度(64)  商户订单号
        /// </summary>  
        public string out_trade_no { get; set; }

        /// <summary>   
        ///    String 必填 长度(100)  用户的登录id
        /// </summary>  
        public string buyer_logon_id { get; set; }

        /// <summary>   
        ///    String 必填 长度(1)  本次退款是否发生了资金变化
        /// </summary>  
        public string fund_change { get; set; }

        /// <summary>   
        ///    Price 必填 长度(11)  退款总金额
        /// </summary>  
        public decimal refund_fee { get; set; }

        /// <summary>   
        ///    Date 必填 长度(32)  退款支付时间 15:45:57
        /// </summary>  
        public string gmt_refund_pay { get; set; }

        /// <summary>   
        ///    TradeFundBill [] 长度(选填)  退款使用的资金渠道
        /// </summary>  
        public List<ZPayRefundItemMo> refund_detail_item_list { get; set; }

        /// <summary>   
        ///    String 选填 长度(512)  交易在支付时候的门店名称
        /// </summary>  
        public string store_name { get; set; }

        /// <summary>   
        ///    String 必填 长度(28)  买家在支付宝的用户id
        /// </summary>  
        public string buyer_user_id { get; set; }
    }

    /// <summary>
    ///   退款使用渠道信息
    /// </summary>
    public class ZPayRefundItemMo
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


    #endregion

    #region 获取对账单下载地址相关实体
    /// <summary>
    ///  获取对账单下载地址请求实体
    /// </summary>
    public class ZGetDownloadUrlReq : ZPayBaseReq
    {
        /// <summary>   
        ///    String 必须 长度(10)  账单类型，商户通过接口或商户经开放平台授权后其所属服务商通过接口可以获取以下账单类型：trade、signcustomer；trade指商户基于支付宝交易收单的业务账单；signcustomer是指基于商户支付宝余额收入及支出等资金变动的帐务账单；
        /// </summary>  
        public string bill_type { get; set; }

        /// <summary>   
        ///    String 必须 长度(15)  账单时间：日账单格式为yyyy-MM-dd，月账单格式为yyyy-MM。
        /// </summary>  
        public string bill_date { get; set; }
    }


    /// <summary>
    /// 获取对账单下载地址响应实体
    /// </summary>
    public class ZGetDownloadUrlResp : ZPayBaseResp
    {
        /// <summary>   
        ///    String 必填 长度(2048)  账单下载地址链接，获取连接后30秒后未下载，链接地址失效。
        /// </summary>  
        public string bill_download_url { get; set; }
    }
    #endregion
}