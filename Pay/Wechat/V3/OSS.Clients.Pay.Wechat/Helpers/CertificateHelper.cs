#region Copyright (C) 2021 Kevin (OSS开源实验室) 公众号：osscore

/***************************************************************************
*　　	文件功能描述：微信支付模快 —— 证书辅助类
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2021-7-21
*       
*****************************************************************************/

#endregion

using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace OSS.Clients.Pay.Wechat.Helpers
{
    /// <summary>
    ///  证书辅助类
    /// </summary>
    public static class CertificateHelper
    {
        #region 商户私钥部分

        /// <summary>
        /// 商户私钥证书签名
        /// </summary>
        /// <param name="privateKeyRsa"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        internal static string Sign(RSA privateKeyRsa, string data)
        {
            return Convert.ToBase64String(
                privateKeyRsa.SignData(
                    Encoding.UTF8.GetBytes(data),
                    HashAlgorithmName.SHA256,
                    RSASignaturePadding.Pkcs1
                )
            );
        }

        /// <summary>
        /// 获取商户对应的私钥证书信息
        /// </summary>
        /// <param name="payConfig"></param>
        /// <returns></returns>
        internal static MchPrivateCertificate GetMchPrivateCertificate(WechatPayConfig payConfig)
        {
            if (_mchCertDics.TryGetValue(payConfig.mch_id, out var mchPrivateCert))
            {
                return mchPrivateCert;
            }

            var pCert = GetCertificateInfo(payConfig.mch_id, payConfig.cert_path, payConfig.cert_password);
            _mchCertDics[payConfig.mch_id] = pCert;

            return pCert;
        }
        private static Dictionary<string, MchPrivateCertificate> _mchCertDics = new Dictionary<string, MchPrivateCertificate>();

        private static MchPrivateCertificate GetCertificateInfo(string mchId, string certPath, string certPassword)
        {
            if (!File.Exists(certPath))
            {
                throw new ArgumentNullException(nameof(certPath), $"请检查商户({mchId})的证书是否放置在路径({certPath})下");
            }

            var cert = new X509Certificate2(certPath, certPassword,
                X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.Exportable);

            return new MchPrivateCertificate(cert.GetSerialNumberString(), cert.GetRSAPrivateKey());
        }

        #endregion






        /// <summary>
        /// 加密敏感信息
        /// </summary>
        /// <param name="publicKeyRsa">商户平台公钥Rsa算法对象</param>
        /// <param name="data"></param>
        /// <returns></returns>
        internal static string Encrypt(RSA publicKeyRsa, string data)
        {
            return Convert.ToBase64String(publicKeyRsa.Encrypt(Encoding.UTF8.GetBytes(data), RSAEncryptionPadding.OaepSHA1));
        }







        internal static bool Verify(RSA publicKeyRsa, string data, string sign)
        {
            return publicKeyRsa.VerifyData(Encoding.UTF8.GetBytes(data), Convert.FromBase64String(sign), HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        }


        private static 

        //public static bool Verify(WechatPayConfig payConfig, HttpResponseDetail detail)
        //{

        //}


    }


    /// <summary>
    ///  商户的私钥证书信息
    /// </summary>
    public readonly struct MchPrivateCertificate
    {
        public MchPrivateCertificate(string serialNumber, RSA privateKey)
        {
            serial_number = serialNumber;
            private_key   = privateKey;
        }

        /// <summary>
        ///  证书序列号
        /// </summary>
        public string serial_number { get;  }

        /// <summary>
        ///  私钥key
        /// </summary>
        public RSA private_key { get; }
        
    }
}
