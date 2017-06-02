
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
        protected internal string notify_url;
        internal string GetNotifyUrl()
        {
            return notify_url;
        }

        protected internal string return_url;
        internal string GetReturnUrl()
        {
            return return_url;
        }

        /// <summary>
        ///   商户授权信息
        /// </summary>
        [JsonIgnore]
        public ZMerchTokenMo auth_token { get;set; }
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
            get { return _code; }
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
        ///    String 必填 长度(-)  网关返回码描述,详见文档 Failed
        /// </summary>  
        public string msg { get; set; }

        /// <summary>   
        ///    String 可空 长度(-)  业务返回码,详见文档
        /// </summary>  
        public string sub_code { get; set; }

        /// <summary>   
        ///    String 可空 长度(-)  业务返回码描述,详见文档
        /// </summary>  
        public string sub_msg { get; set; }
        
        //private readonly SortedDictionary<string, string> _dics = new SortedDictionary<string, string>();

        ///// <summary>
        /////  设置子类中的涉及特有加密属性
        ///// </summary>
        //protected virtual void SetSignDicItems()
        //{
        //}

        ///// <summary>
        /////  设置加密字典条目
        ///// </summary>
        ///// <param name="key"></param>
        ///// <param name="value"></param>
        //protected void SetDicItem(string key, string value)
        //{
        //    if (!string.IsNullOrEmpty(value))
        //        _dics[key] = value;
        //}

        //public SortedDictionary<string, string> GetDics()
        //{
        //    SetDicItem("code", code);
        //    SetDicItem("msg", msg);
        //    SetDicItem("sub_code", sub_code);
        //    SetDicItem("sub_msg", sub_msg);
        //    SetSignDicItems();

        //    return _dics;
        //}
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
    public class ZPayCenterConfig
    {
        /// <summary>
        ///  格式化类型  只有json
        /// </summary>
        public string Format { get; set; } = "JSON";

        /// <summary>
        /// 请求和签名使用的字符编码格式，支持GBK和UTF-8（默认） 
        /// </summary>
        public string Charset { get; set; } = "UTF-8";

        /// <summary>
        ///   商户生成签名字符串所使用的签名算法类型，目前支持RSA2和RSA，推荐使用RSA2
        /// </summary>
        public string SignType { get; set; } = "RSA2";
        
        /// <summary>
        ///  应用来源  用户自定义
        /// </summary>
        public string AppSource { get; set; }
        
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

    }

    // 测试公钥  MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEA1k2O3FwIUWfkM6+vglCsNjKecxN5C2NBbrQaH+zV4Uax3oknuA+AJrl8XDN3BQwsTYQxnMGf/6EOH8QINsjw0MOfoVxZYU20LsxJbv6H0HYA7LLTAsYxFAe3IwtZVrMlWCwTP+XngslI6eJZH/eOZTH6PPnYn7AqcfBkle0JWkBhZ3hq59SUKXOtoUc3gxXOk2509eeldqhHiCLxhoI+zc+GjUaVm8BY16IjRowrU2NCGq6f7CVBTH+TxRhjqwWja1TC41SHFHobvhtW7Lp8CIGOpNBnVI1lidIxLFumWleqwCxkzlF7e1fgGyPTdS16r6COid2wEPmXKojzLmBtLQIDAQAB
    // 测试私钥  MIIEpQIBAAKCAQEA1k2O3FwIUWfkM6+vglCsNjKecxN5C2NBbrQaH+zV4Uax3oknuA+AJrl8XDN3BQwsTYQxnMGf/6EOH8QINsjw0MOfoVxZYU20LsxJbv6H0HYA7LLTAsYxFAe3IwtZVrMlWCwTP+XngslI6eJZH/eOZTH6PPnYn7AqcfBkle0JWkBhZ3hq59SUKXOtoUc3gxXOk2509eeldqhHiCLxhoI+zc+GjUaVm8BY16IjRowrU2NCGq6f7CVBTH+TxRhjqwWja1TC41SHFHobvhtW7Lp8CIGOpNBnVI1lidIxLFumWleqwCxkzlF7e1fgGyPTdS16r6COid2wEPmXKojzLmBtLQIDAQABAoIBAFEF3b9+pD5y8xp/j/HLInETTkjf0eH2UuTR/xaW6n5TxigG3xd99MuvUU9ivdsQsSdVlZRYuG9jqv1jll9wNWwYqh/N1JUvVbJj1le6sAqmss7LBXCFEkUqbZk4TzyyWqotb9G20ILoK0MSKvQlGpc0ABJRvA8UAdg2NTFh/yk5gPHZSbefksfvDfIyGZ28gLEUMiCdxVod+cQ4qu3FlmbBiI+wQlHAAhejhCdSH4djHrGxJwHHdBtkLUackE61hpLMklgQjrYTYrTrwOnf2PAzx/f2Pm39m80I6IudnuO7Mf2y0N+ym0XDmWyOqAE8L3AbGkLV3CZKbQNbusaPa9kCgYEA74iSHtl2xe1YkZA6R9Cy69+LpCqdOTif36w3OXc47I+3aeLpWO3k+gAw9RUz12AJdMZLmMDrFnUsONVKh7KMZpawLQYTnoEnkPM+8zPDBf3hG/F1KoGHR3v+X0gMm9QQkoXb9HjLvW/8dnyiruefpXj/FcqpjjML/Yxv4gKcnssCgYEA5Qj30v/98Yh62EcF6aTkS+ztwTnh/WeNgB2wLPLT06zUnIS/iH5OjOd12j++n8Uerte0dKJo06HoVQTSEKuGQ1YWfc4uOH7NdhBTqTJwf9stCmokuSd0W/nRXYJ3JMtlbnfM6OZ2Ju2s4F7heF19ITs7rfS90tkg1QG1fQK+7OcCgYEA1i++yUsXQ8EKE49uLc1WUEhia3eXgxU5EB7EeuQ6yH1yOoKmudhviYUmQeClrI65peuyqXLIRBqeYmuG63Qiy5EvE/N9E5zVrm1z+rBsUS1FX2E3rbyJJMihGr4oWCb2cq6zkhV5yXkbvS+RoOiI/sQFBI1ltDu9Gwm6+dPwDKUCgYEAswvUuPV4bvZTmnnDTIikFBrPLsvyOChYvPv4etsF76dfulAobyrWe16CijBk8/+kYeis4LUKH8+lkbkoAvIkDsXg5U5mYbH3KsHOtSmCOuF7j3W06a3HUBO2sVRJkdETpU0wOi3X1czd5bUmq/Lh3DWzDOWT853cBcjFOUoJOacCgYEAhEX+jbupYZM0x72arhngAfR6rJh8T7ponMug9efNkOCxW1xx7Bwwb4WdPqYfEolyEr9UVUxSqiO0dKcuLlfYQwrPJ8o7w0PzJGUozSlnIEHBDMpB8iY0SEqh/3sWHme2F73HN67VBS87cK6v82Nqt9zGhXtZOwf7IAQVmXwhVtQ=

}
