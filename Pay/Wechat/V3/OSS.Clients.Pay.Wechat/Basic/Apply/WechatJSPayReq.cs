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


using System;
using System.Collections.Generic;
using OSS.Clients.Pay.Wechat.Helpers;
using OSS.Common;
using OSS.Common.Extension;

namespace OSS.Clients.Pay.Wechat.Basic
{
    /// <summary>
    ///  获取支付二维码
    /// </summary>
    public class WechatJSPayReq : WechatPayBasicReq<WechatJSPayResp>
    {

        public override string GetApiPath()
        {
            return IsSpPartnerReq
                ? "/v3/pay/partner/transactions/jsapi"
                : "/v3/pay/transactions/jsapi";
        }

        /// <summary>   
        ///   支付者   object
        ///      支付者信息
        /// </summary>  
        public WechatPayer payer { get; set; }

        /// <summary>
        ///  处理参数
        /// </summary>
        protected override void PrepareBodyPara()
        {
            base.PrepareBodyPara();
            this.AddBodyPara("payer", payer);
        }
    }

    /// <summary>
    ///  扫码支付响应实体
    /// </summary>
    public class WechatJSPayResp : WechatBaseResp
    {
        /// <summary>   
        ///   预支付交易会话标识   string[1,64]
        ///   预支付交易会话标识。用于后续接口调用中使用，该值有效期为2小时
        /// </summary>  
        public string prepay_id { get; set; }


        /// <summary>
        ///   获取js唤起支付的参数信息
        /// </summary>
        /// <param name="payConfig"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetJsPreParas(WechatPayConfig payConfig)
        {
            var timeStamp = DateTime.Now.ToUtcSeconds().ToString();
            var appId     = payConfig.app_id;
            var nonceStr  = NumHelper.RandomNum(8);
            var package   = $"prepay_id={prepay_id}";

            var waitSignData = $"{appId}\n{timeStamp}\n{nonceStr}\n{package}\n";
            var privateCert  = WechatCertificateHelper.GetMchPrivateCertificate(payConfig);
            var signature    = WechatCertificateHelper.Sign(privateCert.private_key, waitSignData);

            return new Dictionary<string, string>()
            {
                //{"appId", appId},
                {"timeStamp", timeStamp},
                {"nonceStr", nonceStr},
                {"package", package},

                {"signType", "RSA"},
                {"paySign", signature},
            };
        }
    }


    public class WechatPayer
    {
        /// <summary>   
        ///   用户标识   string[1,128]
        ///   用户在直连商户appid下的唯一标识。
        /// </summary>  
        public string openid { get; set; }

    }
}
