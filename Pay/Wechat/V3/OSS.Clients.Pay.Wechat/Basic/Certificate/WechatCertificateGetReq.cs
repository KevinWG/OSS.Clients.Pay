using System;
using System.Collections.Generic;
using System.Net.Http;

namespace OSS.Clients.Pay.Wechat.Basic
{
    public class WechatCertificateGetReq : BaseReq
    {
        public WechatCertificateGetReq() : base("/v3/certificates", HttpMethod.Get)
        {
        }
    }

    public class WechatCertificateGetResp : BaseResp
    {
        public List<WechatCertificateEncrypt> data { get; set; }
    }


    public class WechatCertificateEncrypt
    {
        public string serial_no { get; set; }

        public DateTime effective_time { get; set; }

        public DateTime expire_time    { get; set; }

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
