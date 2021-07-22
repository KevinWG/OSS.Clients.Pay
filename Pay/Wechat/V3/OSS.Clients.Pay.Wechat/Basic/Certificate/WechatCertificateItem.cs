using System.Security.Cryptography;

namespace OSS.Clients.Pay.Wechat.Basic
{
    public class WechatCertificateItem
    {
        public string serial_no { get; set; }

        public long effective_time { get; set; }

        public long expire_time { get; set; }

        public RSA cert_public_key { get; set; }
    }
}