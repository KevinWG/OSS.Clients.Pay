#region Copyright (C) 2021 Kevin (OSS开源实验室) 公众号：osscore

/***************************************************************************
*　　	文件功能描述：微信支付模快 —— AesGcm 加密算法辅助类
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2021-7-21
*       
*****************************************************************************/

#endregion

using System;
using System.Security.Cryptography;
using System.Text;

namespace OSS.Clients.Pay.Wechat.Helpers
{
    /// <summary>
    /// AesGcm 加密算法辅助类（主要用于解密证书接口
    /// 参考来源：https://www.cnblogs.com/jzblive/p/14386757.html
    /// </summary>
    public static class WechatAesGcmHelper
    {
        /// <summary>
        /// 使用 AesGcm 解密
        /// </summary>
        /// <param name="key">key32位字符</param>
        /// <param name="nonce">随机串12位</param>
        /// <param name="encryptedData">密文（Base64字符）</param>
        /// <param name="associatedData">(可能null)</param>
        /// <returns></returns>
        public static byte[] DecryptFromBase64(string key, string nonce, string encryptedData,
            string associatedData)
        {
            var encryptedBytes = Convert.FromBase64String(encryptedData);
            var cipherBytes    = encryptedBytes[..^16]; //tag size is 16
            var tag            = encryptedBytes[^16..];

            var associatedBytes = associatedData == null ? null : Encoding.UTF8.GetBytes(associatedData);
            var decryptedData   = new byte[cipherBytes.Length];
            var nonceBytes      = Encoding.UTF8.GetBytes(nonce);

            using var cipher = new AesGcm(Encoding.UTF8.GetBytes(key));

            cipher.Decrypt(nonceBytes, cipherBytes, tag, decryptedData, associatedBytes);
            return decryptedData;
        }
    }
}
