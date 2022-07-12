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
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using OSS.Clients.Pay.Wechat.Basic;
using OSS.Common.Resp;
using OSS.Common.Extension;

namespace OSS.Clients.Pay.Wechat.Helpers
{
    /// <summary>
    ///  证书辅助类
    /// </summary>
    internal static class WechatCertificateHelper
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
            var path = Path.GetFullPath(certPath);
            if (!File.Exists(path))
            {
                throw new ArgumentNullException(nameof(certPath), $"请检查商户({mchId})的证书是否放置在路径({certPath})下");
            }

            var cert = new X509Certificate2(path, certPassword,
                X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.Exportable);

            return new MchPrivateCertificate(cert.GetSerialNumberString(), cert.GetRSAPrivateKey());
        }

        #endregion

        #region 微信公钥加密敏感信息

        /// <summary>
        /// 加密敏感信息
        /// </summary>
        /// <param name="publicKeyRsa">商户平台公钥Rsa算法对象</param>
        /// <param name="data"></param>
        /// <returns></returns>
        internal static string OAEPEncrypt(RSA publicKeyRsa, string data)
        {
            return Convert.ToBase64String(publicKeyRsa.Encrypt(Encoding.UTF8.GetBytes(data),
                RSAEncryptionPadding.OaepSHA1));
        }

        /// <summary>
        ///  获取最新的公钥信息
        /// </summary>
        /// <param name="payConfig"></param>
        /// <returns></returns>
        internal static async Task<GetCertItemResp> GetLatestCertsByConfig(WechatPayConfig payConfig)
        {
            if (_wechatCertDics.TryGetValue(payConfig.mch_id, out var certDics))
            {
                var item     = certDics.Values.FirstOrDefault();
                var timeLine = DateTime.Now.ToUtcSeconds();
                if (item != null && item.effective_time < timeLine && item.expire_time > timeLine)
                {
                    return new GetCertItemResp()
                    {
                        item = item
                    };
                }
            }

            var refreshDicRes = await RefreshCertsByConfig(payConfig);
            if (!refreshDicRes.IsSuccess())
            {
                return refreshDicRes.ToResp<GetCertItemResp>();
            }

            return new GetCertItemResp() { item = refreshDicRes.dics.Values.First() };
        }

        #endregion


        #region 微信公钥验签

        /// <summary>
        /// 根据微信商户配置，验证结果签名
        /// </summary>
        /// <param name="payConfig">支付配置</param>
        /// <param name="signature">微信返回头信息中的签名</param>
        /// <param name="serialNo">微信返回头信息中的平台证书编号</param>
        /// <param name="nonce">微信返回头信息中的随机串</param>
        /// <param name="timestamp">微信返回头信息中的时间戳</param>
        /// <param name="respBody">微信返回的内容字符串</param>
        /// <returns></returns>
        public static async Task<WechatBaseResp> Verify(WechatPayConfig payConfig,string signature,
                                                  string serialNo,  string nonce, long timestamp, string respBody)
        {
            var certRes = await GetCertsByConfigAndSNo(payConfig, serialNo);
            if (!certRes.IsSuccess())
            {
                return certRes;
            }

            var cert = certRes.item;

            var verContent = $"{timestamp}\n{nonce}\n{respBody}\n";
            var isOk = cert.cert_public_key.VerifyData(Encoding.UTF8.GetBytes(verContent),
                Convert.FromBase64String(signature), HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

            if (isOk)
                return new WechatBaseResp();

            var errRes = new WechatBaseResp()
            {
                code          = RespCodes.ParaSignError.ToString(),
                message       = "验证微信支付签名失败!",
                response_body = respBody
            };
            errRes.ret = (int) RespCodes.ParaSignError;
            return errRes;
        }

        private static async Task<GetCertItemResp> GetCertsByConfigAndSNo(WechatPayConfig payConfig, string serialNo)
        {
            if (_wechatCertDics.TryGetValue(payConfig.mch_id, out var certDics)
                && certDics.TryGetValue(serialNo, out var item))
                return new GetCertItemResp() { item = item };

            var refreshDicRes = await RefreshCertsByConfig(payConfig);
            if (!refreshDicRes.IsSuccess())
            {
                return refreshDicRes.ToResp<GetCertItemResp>();
            }

            var dics = refreshDicRes.dics;
            if (dics.TryGetValue(serialNo, out item))
            {
                return new GetCertItemResp() { item = item };
            }

            return new GetCertItemResp() { code = "Unknow", message = $"最新的微信平台公钥证书列表不存在证书（编号{serialNo}）",response_body = refreshDicRes.response_body};
        }

        #endregion
        

        // <商户号，<证书编号，证书信息>> 
        private static Dictionary<string, Dictionary<string, WechatCertificateItem>>
            _wechatCertDics = new Dictionary<string, Dictionary<string, WechatCertificateItem>>();

        // 刷新获取微信对应的最新公钥信息
        private static async Task<RefreshCertDicResp> RefreshCertsByConfig(WechatPayConfig payConfig)
        {
            var certRes = await (
                WechatPayHelper.WechatPublicCertificateProvider != null
                    ? WechatPayHelper.WechatPublicCertificateProvider.GetCertificates(payConfig)
                    : new WechatCertificateGetReq().SetContextConfig(payConfig).SendAsync()
            );

            if (!certRes.IsSuccess())
                return certRes.ToResp<RefreshCertDicResp>();

            var dics = certRes.data.Select(wcert =>
            {
                var encryptCertificate = wcert.encrypt_certificate;

                if (encryptCertificate.algorithm != "AEAD_AES_256_GCM")
                    throw new NotSupportedException($"微信支付返回平台加密证书使用了未提供的加解密算法{encryptCertificate.algorithm}!");

                var certBytes = WechatAesGcmHelper.DecryptFromBase64(payConfig.api_v3_key, encryptCertificate.nonce,
                    encryptCertificate.ciphertext, encryptCertificate.associated_data);

                var cert = new WechatCertificateItem
                {
                    serial_no       = wcert.serial_no,
                    effective_time  = DateTime.Parse(wcert.effective_time).ToUtcSeconds(),
                    expire_time     = DateTime.Parse(wcert.expire_time).ToUtcSeconds(),
                    cert_public_key = new X509Certificate2(certBytes).GetRSAPublicKey()
                };
                return cert;

            }).OrderByDescending(c => c.effective_time).ToDictionary(c => c.serial_no, c => c);

            _wechatCertDics[payConfig.mch_id] = dics??new Dictionary<string, WechatCertificateItem>();

            return new RefreshCertDicResp() {dics = dics};
        }

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


    internal class GetCertItemResp : WechatBaseResp
    {
        public WechatCertificateItem item { get; set; }
    }

    internal class RefreshCertDicResp:WechatBaseResp
    {
        public Dictionary<string, WechatCertificateItem> dics { get; set; }
    }
}
