#region Copyright (C) 2017 Kevin (OSS开源作坊) 公众号：osscore

/***************************************************************************
*　　	文件功能描述：微信支付模快 —— 企业付款相关实体
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2017-2-27
*       
*****************************************************************************/

#endregion



using OSS.Common.Extension;

namespace OSS.Clients.Pay.WX.Cash.Mos
{
    /// <summary>
    ///  企业付款请求实体
    /// </summary>
    public class WXPayTransferCashReq : WXPayBaseReq
    {
        /// <summary>   
        ///    设备号 可空 String(32) 微信支付分配的终端设备号
        /// </summary>  
        public string device_info { get; set; }

        /// <summary>   
        ///    商户订单号 必填 String 商户订单号，需保持唯一性
        /// </summary>  
        public string partner_trade_no { get; set; }

        /// <summary>   
        ///    用户openid 必填 String 商户appid下，某用户的openid
        /// </summary>  
        public string openid { get; set; }

        /// <summary>   
        ///    校验用户姓名选项 必填 String NO_CHECK：不校验真实姓名 FORCE_CHECK：强校验真实姓名（未实名认证的用户会校验失败，无法转账） OPTION_CHECK：针对已实名认证的用户才校验真实姓名（未实名认证用户不校验，可以转账成功）
        /// </summary>  
        public string check_name { get; set; }

        /// <summary>   
        ///    收款用户姓名 可选 String 收款用户真实姓名。 如果check_name设置为FORCE_CHECK或OPTION_CHECK，则必填用户真实姓名
        /// </summary>  
        public string re_user_name { get; set; }

        /// <summary>   
        ///    金额 必填 int 企业付款金额，单位为分
        /// </summary>  
        public int amount { get; set; }

        /// <summary>   
        ///    企业付款描述信息 必填 String 企业付款操作说明信息。必填。
        /// </summary>  
        public string desc { get; set; }

        /// <summary>   
        ///    Ip地址 必填 String(32) 调用接口的机器Ip地址
        /// </summary>  
        public string spbill_create_ip { get; set; }

        protected override void SetSignDics()
        {
            base.SetSignDics();
            SetDicItem("device_info", device_info);
            SetDicItem("partner_trade_no", partner_trade_no);
            SetDicItem("openid", openid);
            SetDicItem("check_name", check_name);
            SetDicItem("re_user_name", re_user_name);

            SetDicItem("amount", amount);
            SetDicItem("desc", desc);
            SetDicItem("spbill_create_ip", spbill_create_ip);
        }
    }

    /// <summary>
    ///  企业付款响应实体
    /// </summary>
    public class WXPayTransferCashResp : WXPayBaseResp
    {
        /// <summary>   
        ///    商户订单号 必填 String(32) 商户订单号，需保持唯一性
        /// </summary>  
        public string partner_trade_no { get; set; }

        /// <summary>   
        ///    微信订单号 必填 String 企业付款成功，返回的微信订单号
        /// </summary>  
        public string payment_no { get; set; }

        /// <summary>   
        ///    微信支付成功时间 必填 15：26：59 String 企业付款成功时间
        /// </summary>  
        public string payment_time { get; set; }

        /// <summary>
        /// 格式化自身属性部分
        /// </summary>
        protected override void FormatPropertiesFromMsg()
        {
            base.FormatPropertiesFromMsg();
            mch_id = this["mchid"];
            partner_trade_no = this["partner_trade_no"];
            payment_no = this["payment_no"];
            payment_time = this["payment_time"];
        }
    }


    public class WXPayGetTransferCashResp : WXPayBaseResp
    {/// <summary>   
     ///    商户单号 必填 String(28) 商户使用查询API填写的单号的原路返回.
     /// </summary>  
        public string partner_trade_no { get; set; }

        /// <summary>   
        ///    付款单号 必填 String(32) 调用企业付款API时，微信系统内部产生的单号
        /// </summary>  
        public string detail_id { get; set; }

        /// <summary>   
        ///    转账状态 必填 string(16) SUCCESS:转账成功FAILED:转账失败PROCESSING:处理中
        /// </summary>  
        public string status { get; set; }

        /// <summary>   
        ///    失败原因 可空 String 如果失败则有失败原因
        /// </summary>  
        public string reason { get; set; }

        /// <summary>   
        ///    收款用户openid 必填 转账的openid
        /// </summary>  
        public string openid { get; set; }

        /// <summary>   
        ///    收款用户姓名 可空 String 收款用户姓名
        /// </summary>  
        public string transfer_name { get; set; }

        /// <summary>   
        ///    付款金额 必填 int 付款金额单位分）
        /// </summary>  
        public int payment_amount { get; set; }

        /// <summary>   
        ///    转账时间 必填 20:00:00 String 发起转账的时间
        /// </summary>  
        public string transfer_time { get; set; }

        /// <summary>   
        ///    付款描述 必填 String 付款时候的描述
        /// </summary>  
        public string desc { get; set; }

        protected override void FormatPropertiesFromMsg()
        {
            base.FormatPropertiesFromMsg();
            partner_trade_no = this["partner_trade_no"];
            detail_id = this["detail_id"];
            status = this["status"];
            reason = this["reason"];
            openid = this["openid"];

            transfer_name = this["transfer_name"];
            payment_amount = this["payment_amount"].ToInt32();
            transfer_time = this["transfer_time"];
            desc = this["desc"];
        }
    }



}
