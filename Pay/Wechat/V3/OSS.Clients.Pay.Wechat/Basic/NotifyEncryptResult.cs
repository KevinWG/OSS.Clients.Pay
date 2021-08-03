using System.Text;
using System.Text.Json;
using OSS.Clients.Pay.Wechat.Helpers;

namespace OSS.Clients.Pay.Wechat.Basic
{
    public class NotifyEncryptResult
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
        ///   通知资源数据
        /// </summary>  
        public ResultResource resource { get; set; }

        /// <summary>   
        ///   回调摘要   string[1,64]
        ///   回调摘要
        /// </summary>  
        public string summary { get; set; }
    }
    
    public class ResultResource
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

    public static class ResultResourceExtension
    {
        public static string DecrytResource(this ResultResource resource, string apiV3Key)
        {
            var bytes = AesGcmHelper.DecryptFromBase64(apiV3Key, resource.nonce,
                resource.ciphertext, resource.associated_data);

            return Encoding.UTF8.GetString(bytes);
        }
        /// <summary>
        ///  解密通知的支付结果
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="apiV3Key"></param>
        /// <returns></returns>
        public static NotifyPayResult DecrytToPayResult(this ResultResource resource, string apiV3Key)
        {
            var str = DecrytResource(resource, apiV3Key);

            return JsonSerializer.Deserialize<NotifyPayResult>(str);
        }
        /// <summary>
        ///  解密通知的支付结果（服务商结果实体
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="apiV3Key"></param>
        /// <returns></returns>
        public static NotifySPPayResult DecrytToSPPayResult(this ResultResource resource, string apiV3Key)
        {
            var str = DecrytResource(resource, apiV3Key);

            return JsonSerializer.Deserialize<NotifySPPayResult>(str);
        }

        /// <summary>
        ///  解密退款结果
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="apiV3Key"></param>
        /// <returns></returns>
        public static NotifyRefundResult DecrytToRefundResult(this ResultResource resource, string apiV3Key)
        {
            var str = DecrytResource(resource, apiV3Key);

            return JsonSerializer.Deserialize<NotifyRefundResult>(str);
        }

        /// <summary>
        ///  解密退款结果（服务商退款结果实体
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="apiV3Key"></param>
        /// <returns></returns>
        public static NotifySPRefundResult DecrytToSPRefundResult(this ResultResource resource, string apiV3Key)
        {
            var str = DecrytResource(resource, apiV3Key);

            return JsonSerializer.Deserialize<NotifySPRefundResult>(str);
        }
    }

    public class NotifyResponse
    {
        public readonly static NotifyResponse Success = new NotifyResponse("SUCCESS", null);

        public NotifyResponse(string code, string messsage)
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