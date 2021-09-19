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
    ///  获取支付二维码
    /// </summary>
    public class WechatNativePayReq : WechatPayBasicReq<WechatNativePayResp>
    {

        public override string GetApiPath()
        {
            return IsSpPartnerReq
                ? "/v3/pay/partner/transactions/native"
                : "/v3/pay/transactions/native";
        }
    }

    /// <summary>
    ///  扫码支付响应实体
    /// </summary>
    public class WechatNativePayResp : WechatBaseResp
    {
        /// <summary> 
        ///   二维码链接 string[1,512]
        ///   此URL用于生成支付二维码，然后提供给用户扫码支付。注意：code_url并非固定值，使用时按照URL格式转成二维码即可。
        /// </summary>  
        public string code_url {get; set;} 
    }
}
