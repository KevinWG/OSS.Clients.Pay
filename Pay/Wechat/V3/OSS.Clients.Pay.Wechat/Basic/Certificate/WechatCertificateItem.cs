#region Copyright (C) 2021 Kevin (OSS开源实验室) 公众号：osscore

/***************************************************************************
*　　	文件功能描述：微信支付模快 —— 平台证书
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2021-7-23
*       
*****************************************************************************/

#endregion


using System.Security.Cryptography;

namespace OSS.Clients.Pay.Wechat.Basic
{
    /// <summary>
    ///  平台证书信息（已解密
    /// </summary>
    public class WechatCertificateItem
    {
        /// <summary>
        /// 证书编号
        /// </summary>
        public string serial_no { get; set; }

        /// <summary>
        /// 生效时间
        /// </summary>
        public long effective_time { get; set; }

        /// <summary>
        ///  过期时间
        /// </summary>
        public long expire_time { get; set; }

        /// <summary>
        ///  证书公钥RSA
        /// </summary>
        public RSA cert_public_key { get; set; }
    }
}