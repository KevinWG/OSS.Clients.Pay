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

using OSS.Common.Extention;

namespace OSS.PayCenter.WX.Pay.Mos
{
    #region  查询订单接口
    public class WxQueryOrderReq : WxPayBaseReq
    {
        /// <summary>   
        ///    微信订单号 二选一 String(32) 微信的订单号，建议优先使用
        /// </summary>  
        public string transaction_id { get; set; }

        /// <summary>   
        ///    商户订单号 String(32) 20150806125346
        /// </summary>  
        public string out_trade_no { get; set; }

        /// <summary>
        ///  设置参与加密的字段
        /// </summary>
        protected override void SetSignDics()
        {
            SetDicItem("transaction_id", transaction_id);
            SetDicItem("out_trade_no", out_trade_no);
        }
    }

    /// <summary>
    ///  查询订单响应实体
    /// </summary>
    public class WxQueryOrderResp : WxPayBaseResp
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
        ///    交易状态 必填 String(32) SUCCESS—支付成功,REFUND—转入退款,NOTPAY—未支付,CLOSED—已关闭,REVOKED—已撤销（刷卡支付）,USERPAYING--用户支付中,PAYERROR--支付失败(其他原因，如银行返回失败)
        /// </summary>  
        public string trade_state { get; set; }

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
        public string fee_type { get; set; }

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
        ///    交易状态描述 必填 String(256) 对当前查询订单状态的描述和下一步操作的指引
        /// </summary>  
        public string trade_state_desc { get; set; }


        // 注意：这里代金券coupon_count>0 时，类型金额等可以通过 this["coupon_fee_下标"] 获取


        /// <summary>
        /// 格式化自身属性部分
        /// </summary>
        protected override void FormatPropertiesFromMsg()
        {
            device_info = this["device_info"];
            openid = this["openid"];
            is_subscribe = this["is_subscribe"];
            trade_type = this["trade_type"];
            trade_state = this["trade_state"];

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
            trade_state_desc = this["trade_state_desc"];
        }
    }
    #endregion  
}
