

#region Copyright (C) 2017 Kevin (OSS开源作坊) 公众号：osscoder

/***************************************************************************
*　　	文件功能描述：支付宝支付模快 —— 线下收单查询模块
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2017-4-3
*       
*****************************************************************************/

#endregion

using System.Collections.Generic;

namespace OSS.PayCenter.ZFB.Pay.Mos
{
    /// <summary>
    /// 线下交易查询请求实体
    /// </summary>
    public class ZQueryTradeReq : ZPayBaseReq
    {
        /// <summary>   
        ///    String 特殊可选 长度(64)  订单支付时传入的商户订单号,和支付宝交易号不能同时为空,trade_no,out_trade_no如果同时存在优先取trade_no	
        /// </summary>  
        public string out_trade_no { get; set; }

        /// <summary>   
        ///    String 特殊可选 长度(64)  支付宝交易号，和商户订单号不能同时为空 
        /// </summary>  
        public string trade_no { get; set; }
    }

    /// <summary>
    /// 线下交易查询响应实体
    /// </summary>
    public class ZQueryTradeResp : ZTradeInfoBaseResp
    {
        /// <summary>   
        ///    String 必填 长度(32)  交易状态：WAIT_BUYER_PAY（交易创建，等待买家付款）、TRADE_CLOSED（未付款交易超时关闭，或支付完成后全额退款）、TRADE_SUCCESS（交易支付成功）、TRADE_FINISHED（交易结束，不可退款）
        /// </summary>  
        public string trade_status { get; set; }

        /// <summary>   
        ///    Date 必填 长度(32)  本次交易打款给卖家的时间 15:45:57
        /// </summary>  
        public string send_pay_date { get; set; }

        /// <summary>   
        ///    String 选填 长度(64)  支付宝店铺编号
        /// </summary>  
        public string alipay_store_id { get; set; }

        /// <summary>   
        ///    String 选填 长度(32)  商户门店编号
        /// </summary>  
        public string store_id { get; set; }

        /// <summary>   
        ///    String 选填 长度(32)  商户机具终端编号
        /// </summary>  
        public string terminal_id { get; set; }

        /// <summary>   
        ///    String 选填 长度(4096)  行业特殊信息（例如在医保卡支付业务中，向用户返回医疗信息）。
        /// </summary>  
        public string industry_sepc_detail { get; set; }

        /// <summary>   
        ///    VoucherDetail[] 本交易支付时使用的所有优惠券信息
        /// </summary>  
        public List<ZTradeVoucherDetailMo> voucher_detail_list { get; set; }
    }
    /// <summary>
    /// 交易信息中的优惠券信息
    /// </summary>
    public class ZTradeVoucherDetailMo
    {
        /// <summary>   
        ///    String 必填 长度(32)  券id
        /// </summary>  
        public string id { get; set; }

        /// <summary>   
        ///    String 必填 长度(64)  券名称
        /// </summary>  
        public string name { get; set; }

        /// <summary>   
        ///    String 必填 长度(32)  当前有三种类型： - 全场代金券 ALIPAY_DISCOUNT_VOUCHER - 折扣券 ALIPAY_ITEM_VOUCHER - 单品优惠 注：不排除将来新增其他类型的可能，商家接入时注意兼容性避免硬编码 ALIPAY_FIX_VOUCHER
        /// </summary>  
        public string type { get; set; }

        /// <summary>   
        ///    Price 必填 长度(8)  优惠券面额，它应该会等于商家出资加上其他出资方出资
        /// </summary>  
        public string amount { get; set; }

        /// <summary>   
        ///    Price 选填 长度(8)  商家出资（特指发起交易的商家出资金额）
        /// </summary>  
        public string merchant_contribute { get; set; }

        /// <summary>   
        ///    Price 选填 长度(8)  其他出资方出资金额，可能是支付宝，可能是品牌商，或者其他方，也可能是他们的一起出资
        /// </summary>  
        public string other_contribute { get; set; }

        /// <summary>   
        ///    String 选填 长度(256)  优惠券备注信息
        /// </summary>  
        public string memo { get; set; }


    }

}
