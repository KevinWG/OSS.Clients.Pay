#region Copyright (C) 2017 Kevin (OSS开源作坊) 公众号：osscoder

/***************************************************************************
*　　	文件功能描述：微信支付模快 —— 交易实体
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2017-2-24
*       
*****************************************************************************/

#endregion

using System.Collections.Generic;
using OSS.Common.Extention;

namespace OSS.PayCenter.WX.Pay.Mos
{
    /// <summary>
    ///  查询订单响应实体
    /// </summary>
    public class WxPayQueryOrderResp : WxPayOrderTradeResp
    {
        /// <summary>   
        ///    交易状态 必填 String(32) SUCCESS—支付成功,REFUND—转入退款,NOTPAY—未支付,CLOSED—已关闭,REVOKED—已撤销（刷卡支付）,USERPAYING--用户支付中,PAYERROR--支付失败(其他原因，如银行返回失败)
        /// </summary>  
        public string trade_state { get; set; }
        /// <summary>   
        ///    交易状态描述 必填 String(256) 对当前查询订单状态的描述和下一步操作的指引
        /// </summary>  
        public string trade_state_desc { get; set; }
        
        /// <summary>
        /// 格式化自身属性部分
        /// </summary>
        protected override void FormatPropertiesFromMsg()
        {
            base.FormatPropertiesFromMsg();  //  父类包含部分格式化内容
            trade_state_desc = this["trade_state_desc"];
            trade_state = this["trade_state"];
        }
    }

    /// <summary>
    ///  支付订单交易响应实体
    /// </summary>
    public class WxPayOrderTradeResp : WxPayBaseResp
    {
        /// <summary>   
        ///    设备号 可空 String(32) 微信支付分配的终端设备号，
        /// </summary>  
        public string device_info { get; set; }

        /// <summary>   
        ///    用户标识 必填 String(128) 用户在商户appid下的唯一标识
        /// </summary>  
        public string openid { get; set; }

        /// <summary>   
        ///    是否关注公众账号 可空 String(1) 用户是否关注公众账号，Y-关注，N-未关注，仅在公众账号类型支付有效
        /// </summary>  
        public string is_subscribe { get; set; }

        /// <summary>   
        ///    交易类型 必填 String(16) 调用接口提交的交易类型，取值如下：JSAPI，NATIVE，APP，MICROPAY，详细说明见参数规定
        /// </summary>  
        public string trade_type { get; set; }
        
        /// <summary>   
        ///    付款银行 必填 String(16) 银行类型，采用字符串类型的银行标识
        /// </summary>  
        public string bank_type { get; set; }

        /// <summary>   
        ///    标价金额 必填 Int 订单总金额，单位为分
        /// </summary>  
        public int total_fee { get; set; }

        /// <summary>   
        ///    应结订单金额 可空 Int 应结订单金额=订单金额-非充值代金券金额，应结订单金额小于等于订单金额。
        /// </summary>  
        public int settlement_total_fee { get; set; }

        /// <summary>   
        ///    标价币种 可空 String(8) 货币类型，符合ISO 4217标准的三位字母代码，默认人民币：CNY，其他值列表详见货币类型
        /// </summary>  
        public string fee_type { get; set; } = "CNY";

        /// <summary>   
        ///    现金支付金额 必填 Int 现金支付金额订单现金支付金额，详见支付金额
        /// </summary>  
        public int cash_fee { get; set; }

        /// <summary>   
        ///    现金支付币种 可空 String(16) 货币类型，符合ISO 4217标准的三位字母代码，默认人民币：CNY，其他值列表详见货币类型
        /// </summary>  
        public string cash_fee_type { get; set; }

        /// <summary>   
        ///    代金券金额 可空 Int “代金券”金额小于订单金额，订单金额-“代金券”金额=现金支付金额，详见支付金额
        /// </summary>  
        public int coupon_fee { get; set; }

        /// <summary>   
        ///    代金券使用数量 可空 Int 代金券使用数量
        /// </summary>  
        public int coupon_count { get; set; }

        /// <summary>   
        ///    微信支付订单号 必填 String(32) 微信支付订单号
        /// </summary>  
        public string transaction_id { get; set; }

        /// <summary>   
        ///    商户订单号 必填 String(32) 商户系统的订单号，与请求一致。
        /// </summary>  
        public string out_trade_no { get; set; }

        /// <summary>   
        ///    附加数据 可空 String(128) 附加数据，原样返回
        /// </summary>  
        public string attach { get; set; }

        /// <summary>   
        ///    支付完成时间 必填 String(14) 订单支付时间，格式为yyyyMMddHHmmss，如2009年12月25日9点10分10秒表示为20091225091010。其他详见时间规则
        /// </summary>  
        public string time_end { get; set; }
        
        /// <summary>
        ///  订单中使用的优惠券列表
        /// </summary>
        public List<WxPayOrderCouponMo> coupons { get; set; }

        /// <summary>
        /// 格式化自身属性部分
        /// </summary>
        protected override void FormatPropertiesFromMsg()
        {
            device_info = this["device_info"];
            openid = this["openid"];
            is_subscribe = this["is_subscribe"];
            trade_type = this["trade_type"];

            bank_type = this["bank_type"];
            total_fee = this["total_fee"].ToInt32();
            settlement_total_fee = this["settlement_total_fee"].ToInt32();
            fee_type = this["fee_type"];
            cash_fee = this["cash_fee"].ToInt32();

            cash_fee_type = this["cash_fee_type"];
            coupon_fee = this["coupon_fee"].ToInt32();
            coupon_count = this["coupon_count"].ToInt32();
            transaction_id = this["transaction_id"];
            out_trade_no = this["out_trade_no"];

            attach = this["attach"];
            time_end = this["time_end"];

            if (coupon_count > 0)
            {
                coupons = new List<WxPayOrderCouponMo>(coupon_count);
                for (int i = 0; i < coupon_count; i++)
                {
                    var coupon = new WxPayOrderCouponMo();
                    coupon.coupon_fee = this["coupon_fee_" + i].ToInt32();
                    coupon.coupon_id = this["coupon_id_" + i];
                    coupon.coupon_type = this["coupon_type_" + i];
                }
            }
        }
    }

    /// <summary>
    ///   支付订单对应的代金券实体
    /// </summary>
    public class WxPayOrderCouponMo
    {
        /// <summary>   
        ///    代金券类型 可空 String CASH--充值代金券 NO_CASH---非充值代金券订单使用代金券时有返回（取值：CASH、NO_CASH）。$n为下标,从0开始编号，举例：coupon_type_$0
        /// </summary>  
        public string coupon_type { get; set; }

        /// <summary>   
        ///    代金券ID 可空 String(20) 代金券ID, $n为下标，从0开始编号
        /// </summary>  
        public string coupon_id { get; set; }

        /// <summary>   
        ///    单个代金券支付金额 可空 Int 单个代金券支付金额, $n为下标，从0开始编号
        /// </summary>  
        public int coupon_fee { get; set; }
    }




    /// <summary>
    /// 下载对账单请求
    /// </summary>
    public class WxPayDownloadBillReq : WxPayBaseReq
    {
        /// <summary>   
        ///    对账单日期 必填 20140603 下载对账单的日期，格式：20140603
        /// </summary>  
        public string bill_date { get; set; }

        /// <summary>   
        ///    账单类型 必填 ALL ALL，返回当日所有订单信息，默认值,SUCCESS，返回当日成功支付的订单，REFUND，返回当日退款订单，RECHARGE_REFUND，返回当日充值退款订单（相比其他对账单多一栏“返还手续费”）
        /// </summary>  
        public string bill_type { get; set; }

        /// <summary>   
        ///    设备号 可空 013467007045764 微信支付分配的终端设备号
        /// </summary>  
        public string device_info { get; set; }
    }

}
