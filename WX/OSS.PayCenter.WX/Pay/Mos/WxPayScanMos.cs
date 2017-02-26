#region Copyright (C) 2017 Kevin (OSS开源作坊) 公众号：osscoder

/***************************************************************************
*　　	文件功能描述：微信支付模快 —— 扫码支付相关实体
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2017-2-26
*       
*****************************************************************************/

#endregion

namespace OSS.PayCenter.WX.Pay.Mos
{
    /// <summary>
    ///   微信回调传递的消息实体
    /// </summary>
    public class WxPayScanCallBackMo:WxPayBaseResp
    {
        /// <summary>   
        ///    用户标识 String(128) 必填 用户在商户appid下的唯一标识
        /// </summary>  
        public string openid { get; set; }

        /// <summary>   
        ///    是否关注公众账号 String(1) 必填 用户是否关注公众账号，仅在公众账号类型支付有效，取值范围：Y或N;Y-关注;N-未关注
        /// </summary>  
        public string is_subscribe { get; set; }

        /// <summary>   
        ///    商品ID String(32) 必填 商户定义的商品id 或者订单号
        /// </summary>  
        public string product_id { get; set; }
        
        /// <summary>
        /// 格式化自身属性部分
        /// </summary>
        protected override void FormatPropertiesFromMsg()
        {
            base.FormatPropertiesFromMsg();
            openid = this["openid"];
            is_subscribe = this["is_subscribe"];
            product_id = this["product_id"];
        }
    }

    /// <summary>
    ///   回复微信回调的消息实体
    /// </summary>
    internal class WxPayScanCallBackResMo : WxPayBaseReq
    {
        /// <summary>   
        ///    返回状态码 String(16) 必填 SUCCESS/FAIL,此字段是通信标识，非交易标识，交易是否成功需要查看result_code来判断
        /// </summary>  
        public string return_code { get; set; }

        /// <summary>   
        ///    返回信息 String(128) 可空 返回信息，如非空，为错误原因;签名失败;具体某个参数格式校验错误.
        /// </summary>  
        public string return_msg { get; set; }

        /// <summary>   
        ///    预支付ID String(64) 必填 调用统一下单接口生成的预支付ID
        /// </summary>  
        public string prepay_id { get; set; }

        /// <summary>   
        ///    业务结果 String(16) 必填 SUCCESS/FAIL
        /// </summary>  
        public string result_code { get; set; }

        /// <summary>   
        ///    错误描述 String(128) 可空 当result_code为FAIL时，商户展示给用户的错误提
        /// </summary>  
        public string err_code_des { get; set; }

        protected override void SetSignDics()
        {
            base.SetSignDics();
            SetDicItem("return_code", return_code);
            SetDicItem("return_msg", return_msg);
            SetDicItem("prepay_id", prepay_id);
            SetDicItem("result_code", result_code);
            SetDicItem("err_code_des", err_code_des);
        }
    }
}
