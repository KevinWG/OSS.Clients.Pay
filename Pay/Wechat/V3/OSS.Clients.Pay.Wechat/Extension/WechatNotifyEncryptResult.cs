using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using OSS.Clients.Pay.Wechat.Helpers;
using OSS.Common.Extension;
using OSS.Common.Resp;

namespace OSS.Clients.Pay.Wechat.Basic
{
    /// <summary>
    ///  微信支付通知接收器
    /// </summary>
    public class WechatNotifyReceiver
    {
        internal Dictionary<string, string> header_dics;
        internal string                     body;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="wechatHeaderDics"></param>
        /// <param name="httpBody"></param>
        public WechatNotifyReceiver(Dictionary<string, string> wechatHeaderDics, string httpBody)
        {
            header_dics = wechatHeaderDics;
            this.body   = httpBody;
        }
    }


    /// <summary>
    /// 微信通知的解密实体
    /// </summary>
    public class WechatNotifyEncryptResult: Resp
    {
        /// <summary>   
        ///   通知ID   string[1,36]
        ///   通知的唯一ID
        /// </summary>  
        public string id { get; set; }

        /// <summary>   
        ///   通知创建时间   string[1,32]
        ///   通知创建的时间，遵循rfc3339标准格式，格式为YYYY-MM-DDTHH:mm:ss+TIMEZONE，YYYY-MM-DD表示年月日，T出现在字符串中，表示time元素的开头，HH:mm:ss.表示时分秒，TIMEZONE表示时区（+08:00表示东八区时间，领先UTC   8小时，即北京时间）。例如：2015-05-20T13:29:35+08:00表示北京时间2015年05月20日13点29分35秒。
        /// </summary>  
        public string create_time { get; set; }

        /// <summary>   
        ///   通知类型   string[1,32]
        ///   通知的类型，支付成功通知的类型为TRANSACTION.SUCCESS
        /// </summary>  
        public string event_type { get; set; }

        /// <summary>   
        ///   通知数据类型   string[1,32]
        ///   通知的资源数据类型，支付成功通知为encrypt-resource
        /// </summary>  
        public string resource_type { get; set; }

        /// <summary>   
        ///   +通知数据   object
        ///   微信通知的加密内容
        /// </summary>  
        public WechatResultResource resource { get; set; }

        /// <summary>   
        ///   回调摘要   string[1,64]
        ///   回调摘要
        /// </summary>  
        public string summary { get; set; }
    }

    /// <summary>
    ///  微信通知的加密内容
    /// </summary>
    public class WechatResultResource
    {
        /// <summary>   
        ///   加密算法类型   string[1,32]
        ///   对开启结果数据进行加密的加密算法，目前只支持AEAD_AES_256_GCM
        /// </summary>  
        public string algorithm { get; set; }

        /// <summary>   
        ///   数据密文   string[1,1048576]
        ///   Base64编码后的开启/停用结果数据密文
        /// </summary>  
        public string ciphertext { get; set; }

        /// <summary>   
        ///   附加数据   string[1,16]
        /// </summary>  
        public string associated_data { get; set; }

        /// <summary>   
        ///   原始类型   string[1,16]
        ///   原始回调类型，为transaction
        /// </summary>  
        public string original_type { get; set; }

        /// <summary>   
        ///   随机串   string[1,16]
        ///   加密使用的随机串
        /// </summary>  
        public string nonce { get; set; }

        /// <summary>   
        ///   回调摘要   string[1,64]
        /// </summary>  
        public string summary { get; set; }
    }

    /// <summary>
    ///  通知实体的扩展方法
    /// </summary>
    public static class WechatResultResourceExtension
    {
        /// <summary>
        ///  解密通知的支付结果
        /// </summary>
        /// <param name="receiver"></param>
        /// <param name="apiV3Key"></param>
        /// <returns></returns>
        public static Task<WechatNotifyPayResult> DecrytToPayResult(this WechatNotifyReceiver receiver, string apiV3Key)
        {
            return DecrytToResult<WechatNotifyPayResult>(receiver, apiV3Key);
        }

        /// <summary>
        ///  解密通知的支付结果（服务商结果实体
        /// </summary>
        /// <param name="receiver"></param>
        /// <param name="apiV3Key"></param>
        /// <returns></returns>
        public static Task<NotifySPPayResult> DecrytToSPPayResult(this WechatNotifyReceiver receiver, string apiV3Key)
        {
            return DecrytToResult<NotifySPPayResult>(receiver, apiV3Key);
        }

        /// <summary>
        ///  解密退款结果
        /// </summary>
        /// <param name="receiver"></param>
        /// <param name="apiV3Key"></param>
        /// <returns></returns>
        public static Task<WechatNotifyRefundResult> DecrytToRefundResult(this WechatNotifyReceiver receiver, string apiV3Key)
        {
            return DecrytToResult<WechatNotifyRefundResult>(receiver, apiV3Key);
        }

        /// <summary>
        ///  解密退款结果（服务商退款结果实体
        /// </summary>
        /// <param name="receiver"></param>
        /// <param name="apiV3Key"></param>
        /// <returns></returns>
        public static Task<NotifySPRefundResult> DecrytToSPRefundResult(this WechatNotifyReceiver receiver, string apiV3Key)
        {
            return DecrytToResult<NotifySPRefundResult>(receiver, apiV3Key);
        }

        private static async Task<TRes> DecrytToResult<TRes>(WechatNotifyReceiver receiver, string apiV3Key)
            where TRes : Resp, new()
        {
            var eRes = await receiver.ToNotifyEncryptResult();
            if (!eRes.IsSuccess())
                return new TRes().WithResp(eRes);
            
            var str = DecrytResource(eRes.resource, apiV3Key);
            return JsonSerializer.Deserialize<TRes>(str);
        }


        /// <summary>
        ///  解密通知内容
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="apiV3Key"></param>
        /// <returns></returns>
        public static string DecrytResource(this WechatResultResource resource, string apiV3Key)
        {
            var bytes = WechatAesGcmHelper.DecryptFromBase64(apiV3Key, resource.nonce,
                resource.ciphertext, resource.associated_data);

            return Encoding.UTF8.GetString(bytes);
        }

        /// <summary>
        ///   转化为微信通知的实体（包含验证签名
        /// </summary>
        /// <param name="receiver"></param>
        /// <returns></returns>
        public static async Task<WechatNotifyEncryptResult> ToNotifyEncryptResult(this WechatNotifyReceiver receiver)
        {
            var config = WechatPayHelper.pay_config;

            if (!receiver.header_dics.TryGetValue("Wechatpay-Nonce", out var nonce)
                || !receiver.header_dics.TryGetValue("Wechatpay-Signature", out var signature)
                || !receiver.header_dics.TryGetValue("Wechatpay-Timestamp", out var timestamp)
                || !receiver.header_dics.TryGetValue("Wechatpay-Serial", out var serial))
            {
                return new WechatNotifyEncryptResult().WithResp(RespCodes.ParaError, "微信支付通知头部参数异常!");
            }

            var checkRes = await WechatPayHelper.Verify(config, signature, serial, nonce, timestamp.ToInt64(), receiver.body);
            // 签名正确
            if (checkRes.IsSuccess())
            {
                var wRes = JsonSerializer.Deserialize<WechatNotifyEncryptResult>(receiver.body);
                return wRes ?? new WechatNotifyEncryptResult().WithResp(RespCodes.OperateFailed, "微信支付通知内容异常!");
            }
            return new WechatNotifyEncryptResult().WithResp(checkRes);
        }
    }

    /// <summary>
    ///  微信被动通知响应实体
    /// </summary>
    public class WechatNotifyResponse
    {
        /// <summary>
        /// 结果响应（默认成功
        /// </summary>
        public WechatNotifyResponse():this("SUCCESS",String.Empty)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code">成功返回：SUCCESS</param>
        /// <param name="messsage"></param>
        public WechatNotifyResponse(string code, string messsage)
        {
            this.code    = code;
            this.message = messsage;
        }

        /// <summary>   
        ///   返回状态码   string[1,32]
        ///   错误码，SUCCESS为接收成功，其他错误码为失败
        /// </summary>  
        public string code { get; set; }

        /// <summary>   
        ///   返回信息   string[1,256]
        ///   返回信息，如非空，为错误原因
        /// </summary>  
        public string message { get; set; }
    }
}