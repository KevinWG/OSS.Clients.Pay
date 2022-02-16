#region Copyright (C) 2021 Kevin (OSS开源实验室) 公众号：osscore

/***************************************************************************
*　　	文件功能描述：微信支付模快 —— 用户扫码支付请求
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2021-7-21
*       
*****************************************************************************/

#endregion


namespace OSS.Clients.Pay.Wechat.Basic
{
    /// <summary>
    ///  微信H5支付请求
    /// </summary>
    public class WechatH5PayReq : WechatPayBasicReq<WechatH5PayResp>
    {
        /// <inheritdoc />
        public override string GetApiPath()
        {
            return IsSpPartnerReq
                ? "/v3/pay/partner/transactions/h5"
                : "/v3/pay/transactions/h5";
        }

        /// <summary>   
        ///    支付场景描述
        /// </summary>  
        public SceneInfo scene_info { get; set; }

        /// <summary>
        ///  处理参数
        /// </summary>
        protected override void PrepareBodyPara()
        {
            base.PrepareBodyPara();
            this.AddBodyPara("scene_info", scene_info);
        }
    }

    /// <summary>
    ///  微信H5支付响应结果
    /// </summary>
    public class WechatH5PayResp : WechatBaseResp
    {
        /// <summary>   
        ///   预支付交易会话标识   string[1,64]
        ///   预支付交易会话标识。用于后续接口调用中使用，该值有效期为2小时
        /// </summary>  
        public string h5_url { get; set; }
    }

    public class SceneInfo
    {
        /// <summary>
        /// 【是】 用户的客户端IP，支持IPv4和IPv6两种格式的IP地址
        /// </summary>
        public string payer_client_ip { get; set; }

        /// <summary>
        /// 【是】   H5场景信息
        /// </summary>
        public H5Info h5_info { get; set; }
    }

    public class H5Info
    {
        /// <summary>
        /// 【是】场景类型,示例值：iOS, Android, Wap
        /// </summary>
        public string type { get; set; }
    }


}
