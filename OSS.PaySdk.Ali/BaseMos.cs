
#region Copyright (C) 2017 Kevin (OSS开源作坊) 公众号：osscoder

/***************************************************************************
*　　	文件功能描述：支付宝支付模快 —— 基础实体模块
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2017-3-28
*       
*****************************************************************************/

#endregion

using Newtonsoft.Json;
using OSS.Common.ComModels;

namespace OSS.PaySdk.Ali
{
    /// <summary>
    ///   支付请求基础实体
    /// </summary>
    public class ZPayBaseReq
    {
        protected internal string notifyUrl;
        internal string GetNotifyUrl()
        {
            return notifyUrl;
        }

        protected internal string returnUrl;
        internal string GetReturnUrl()
        {
            return returnUrl;
        }

        /// <summary>
        ///   商户授权信息
        /// </summary>
        [JsonIgnore]
        public ZMerchTokenMo app_auth_token { get;set; }
    }

    /// <summary>
    /// 基础响应实体
    /// </summary>
    public class ZPayBaseResp:ResultMo
    {

        private string _code = string.Empty;

        /// <summary>   
        ///    String 必填 长度(-)  网关返回码,详见文档
        /// </summary>  
        public string code
        {
            get => _code;
            set
            {
                _code = value;
                if (_code!= "10000")
                {
                    ret = -1;
                }
            }
        }

        /// <summary>   
        ///    String 可空 长度(-)  业务返回码,详见文档
        /// </summary>  
        public string sub_code { get; set; }

        /// <summary>   
        ///    String 可空 长度(-)  业务返回码描述,详见文档
        /// </summary>  
        public string sub_msg { get; set; }
    }
    
    /// <summary>
    ///  商户授权信息实体
    /// </summary>
    public class ZMerchTokenMo
    {
        /// <summary>
        /// 应用授权 可空 token
        /// </summary>
        public string app_auth_token { get; set; }
    }

    /// <summary>
    ///  支付宝支付配置信息
    /// </summary>
    public class ZPayConfig
    {
        /// <summary>
        ///  格式化类型  只有json（默认） 
        /// </summary>
        public string Format { get; set; } = "JSON";

        /// <summary>
        /// 请求和签名使用的字符编码格式，支持GBK和UTF-8（默认） 
        /// </summary>
        public string Charset { get; set; } = "UTF-8";

        /// <summary>
        ///   商户生成签名字符串所使用的签名算法类型，只提供 RSA2,不再支持RSA
        /// </summary>
        public string SignType { get; } = "RSA2";

        /// <summary>
        ///   应用Id
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        ///   开发者应用私钥，由开发者自己生成
        /// </summary>
        public string AppPrivateKey { get; set; }

        /// <summary>
        /// 支付宝公钥，由支付宝生成
        /// </summary>
        public string AppPublicKey { get; set; }

        /// <summary>   
        ///    String 必填 长度(3)  调用的接口版本，固定为：1.0
        /// </summary>  
        public string Version { get; set; } = "1.0";
        
        /// <summary>
        ///  如果接口参数中没有传值，则默认使用此值
        /// </summary>
        public string NotifyUrl { get; set; }
    }
}
