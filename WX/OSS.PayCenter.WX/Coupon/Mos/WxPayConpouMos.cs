#region Copyright (C) 2017 Kevin (OSS开源作坊) 公众号：osscoder

/***************************************************************************
*　　	文件功能描述：微信支付模快 —— 代金券相关实体
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2017-2-27
*       
*****************************************************************************/

#endregion

using OSS.Common.Extention;

namespace OSS.PayCenter.WX.Coupon.Mos
{
   /// <summary>
   ///  发送代金券请求实体
   /// </summary>
    public class WxPaySendConpouReq:WxPayBaseReq
    {
        /// <summary>   
        ///    代金券批次id 必填 String 代金券批次id
        /// </summary>  
        public string coupon_stock_id { get; set; }

        /// <summary>   
        ///    openid记录数 必填 int openid记录数（目前支持num=1）
        /// </summary>  
        public int openid_count { get; set; } = 1;

        /// <summary>   
        ///    商户单据号 必填 String 商户此次发放凭据号（格式：商户id+日期+流水号），商户侧需保持唯一性
        /// </summary>  
        public string partner_trade_no { get; set; }

        /// <summary>   
        ///    用户openid 必填 String Openid信息
        /// </summary>  
        public string openid { get; set; }

        /// <summary>   
        ///    操作员 可空 String(32) 操作员帐号, 默认为商户号 可在商户平台配置操作员对应的api权限
        /// </summary>  
        public string op_user_id { get; set; }

        /// <summary>   
        ///    设备号 可空 String(32) 微信支付分配的终端设备号
        /// </summary>  
        public string device_info { get; set; }
        
        protected override void SetSignDics()
        {
            base.SetSignDics();
            SetDicItem("coupon_stock_id", coupon_stock_id);
            SetDicItem("openid_count", openid_count);
            SetDicItem("partner_trade_no", partner_trade_no);
            SetDicItem("openid", openid);
            SetDicItem("op_user_id", op_user_id);
            SetDicItem("device_info", device_info);
        }
    }

    /// <summary>
    ///   发送代金券响应实体
    /// </summary>
    public class WxPaySendConpouResp : WxPayBaseResp
    {
        /// <summary>   
        ///    设备号 可空 String(32) 微信支付分配的终端设备号
        /// </summary>  
        public string device_info { get; set; }

        /// <summary>   
        ///    代金券批次id 必填 String 用户在商户appid下的唯一标识
        /// </summary>  
        public string coupon_stock_id { get; set; }

        /// <summary>   
        ///    返回记录数 必填 Int 返回记录数
        /// </summary>  
        public int resp_count { get; set; }

        /// <summary>   
        ///    成功记录数 必填 Int 成功记录数
        /// </summary>  
        public int success_count { get; set; }

        /// <summary>   
        ///    失败记录数 必填 Int 失败记录数
        /// </summary>  
        public int failed_count { get; set; }

        /// <summary>   
        ///    用户标识 必填 String 用户在商户appid下的唯一标识
        /// </summary>  
        public string openid { get; set; }

        /// <summary>   
        ///    返回码 必填 String 返回码，SUCCESS/FAILED
        /// </summary>  
        public string ret_code { get; set; }

        /// <summary>   
        ///    代金券id 必填 String 对一个用户成功发放代金券则返回代金券id，即ret_code为SUCCESS的时候； 如果ret_code为FAILED则填写空串""
        /// </summary>  
        public string coupon_id { get; set; }

        /// <summary>   
        ///    返回信息 必填 String 返回信息，当返回码是FAILED的时候填写，否则填空串“”
        /// </summary>  
        public string ret_msg { get; set; }


        protected override void FormatPropertiesFromMsg()
        {
            base.FormatPropertiesFromMsg();
            device_info = this["device_info"];
            coupon_stock_id = this["coupon_stock_id"];
            resp_count = this["resp_count"].ToInt32();
            success_count = this["success_count"].ToInt32();
            failed_count = this["failed_count"].ToInt32();


            openid = this["openid"];
            ret_code = this["ret_code"];
            coupon_id = this["coupon_id"];
            ret_msg = this["ret_msg"];
        }
    }
}
