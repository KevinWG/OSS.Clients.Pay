#region Copyright (C) 2021 Kevin (OSS开源实验室) 公众号：osscore

/***************************************************************************
*　　	文件功能描述：微信支付模快 —— 获取证书请求
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2021-7-23
*       
*****************************************************************************/

#endregion


using System.Collections.Generic;
using System.Net.Http;


#pragma warning disable 8618

namespace OSS.Clients.Pay.Wechat.Basic
{
    /// <summary>
    /// 获取平台证书请求
    /// </summary>
    public class WechatCertificateGetReq : WechatBaseReq<WechatCertificateGetResp>
    {
        public WechatCertificateGetReq() :base(HttpMethod.Get)
        {
        }

        /// <inheritdoc />
        public override string GetApiPath()
        {
            return "/v3/certificates";
        }
        
    }

    public class WechatCertificateGetResp : WechatBaseResp
    {
        public List<WechatCertificateEncrypt> data { get; set; }
    }
    
    public class WechatCertificateEncrypt
    {
        public string serial_no { get; set; }

        public string effective_time { get; set; }

        public string expire_time { get; set; }

        public CertificateEncryptDetail encrypt_certificate { get; set; }
    }

    public class CertificateEncryptDetail
    {
        public string algorithm { get; set; }
        
        public string nonce { get; set; }

        public string associated_data { get; set; }

        public string ciphertext { get; set; }
    }

}
