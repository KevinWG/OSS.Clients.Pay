#region Copyright (C) 2017 Kevin (OSS开源作坊) 公众号：osscoder

/***************************************************************************
*　　	文件功能描述：微信支付模快 —— 刷卡下单接口实体
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2017-2-23
*       
*****************************************************************************/

#endregion

using OSS.Common.Extention;

namespace OSS.PaySdk.Wx.Pay.Mos
{
    /// <summary>
    ///  刷卡下单请求实体
    /// </summary>
    public class WxAddMicroPayOrderReq : WxAddPayOrderBaseReq
    {
        /// <summary>   
        ///    授权码 必填 String(128) 扫码支付授权码，设备读取用户微信中的条码或者二维码信息
        /// </summary>  
        public string auth_code { get; set; }

        /// <summary>
        ///  该字段用于上报场景信息，目前支持上报实际门店信息。 该字段为JSON对象数据，
        ///  对象格式为{"store_info":{"id": "门店ID","name": "名称","area_code": "编码","address": "地址" }} 
        /// </summary>
        public string scene_info { get; set; }

        protected override void SetSignDics()
        {
            base.SetSignDics();
            SetDicItem("auth_code", auth_code);
            SetDicItem("scene_info", scene_info);
        }
    }


    /// <summary>
    ///  刷卡支付响应值
    /// </summary>
    public class WxAddMicroPayOrderResp : WxAddPayOrderBaseResp
    {
        /// <summary>   
        /// 用户标识  是  String(128)  Y  用户在商户appid  下的唯一标识
        /// </summary>  
        public string openid { get; set; }

        /// <summary>   
        ///     是否关注公众账号  是  String(1)  Y  用户是否关注公众账号，仅在公众账号类型支付有效，取值范围：Y或N;Y-关注;N-未关注
        /// </summary>  
        public string is_subscribe { get; set; }

        /// <summary>   
        ///     付款银行  是  String(32)  CMC  银行类型，采用字符串类型的银行标识，详见银行类型
        /// </summary>  
        public string bank_type { get; set; }

        /// <summary>   
        ///     货币类型  否  String(16)  CNY  符合ISO  4217标准的三位字母代码，默认人民币：CNY，详见货币类型
        /// </summary>  
        public string fee_type { get; set; }

        /// <summary>   
        ///     订单金额  是  Int  888  订单总金额，单位为分，只能为整数，详见支付金额
        /// </summary>  
        public int total_fee { get; set; }

        /// <summary>   
        ///     应结订单金额  否  Int  100  当订单使用了免充值型优惠券后返回该参数，应结订单金额=订单金额-免充值优惠券金额。
        /// </summary>  
        public int settlement_total_fee { get; set; }

        /// <summary>   
        ///     代金券金额  否  Int  100  “代金券”金额<=订单金额，订单金额-“代金券”金额=现金支付金额，详见支付金额
        /// </summary>  
        public int coupon_fee { get; set; }

        /// <summary>   
        ///     现金支付货币类型  否  String(16)  CNY  符合ISO  4217标准的三位字母代码，默认人民币：CNY，其他值列表详见货币类型
        /// </summary>  
        public string cash_fee_type { get; set; }

        /// <summary>   
        ///     现金支付金额  是  Int  100  订单现金支付金额，详见支付金额
        /// </summary>  
        public int cash_fee { get; set; }

        /// <summary>   
        ///     微信支付订单号  是  String(32)  1217752501201407033233368018  微信支付订单号
        /// </summary>  
        public string transaction_id { get; set; }

        /// <summary>   
        ///     商户订单号  是  String(32)  1217752501201407033233368018  商户系统内部订单号，要求32个字符内，只能是数字、大小写字母_-|*@  ，且在同一个商户号下唯一。
        /// </summary>  
        public string out_trade_no { get; set; }

        /// <summary>   
        ///     商家数据包  否  String(128)  123456  商家数据包，原样返回
        /// </summary>  
        public string attach { get; set; }

        /// <summary>   
        ///     支付完成时间  是  String(14)  20141030133525  订单生成时间，格式为yyyyMMddHHmmss，如2009年12月25日9点10分10秒表示为20091225091010。详见时间规则
        /// </summary>  
        public string time_end { get; set; }

        /// <summary>   
        ///     营销详情  否  String(6000)  示例见下文  新增返回，单品优惠功能字段，需要接入请见详细说明
        /// </summary>  
        public string promotion_detail { get; set; }


        /// <inheritdoc />
        protected override void FormatPropertiesFromMsg()
        {
            base.FormatPropertiesFromMsg();
            openid = this["openid"];
            is_subscribe = this["is_subscribe"];
            bank_type = this["bank_type"];
            fee_type = this["fee_type"];
            total_fee = this["total_fee"].ToInt32();

            settlement_total_fee = this["settlement_total_fee"].ToInt32();
            coupon_fee = this["coupon_fee"].ToInt32();
            cash_fee_type = this["cash_fee_type"];
            cash_fee = this["cash_fee"].ToInt32();
            transaction_id = this["transaction_id"];

            out_trade_no = this["out_trade_no"];
            attach = this["attach"];
            time_end = this["time_end"];
            promotion_detail = this["promotion_detail"];
        }
    }
}