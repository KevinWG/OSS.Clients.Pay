
#region Copyright (C) 2017 Kevin (OSS开源作坊) 公众号：osscoder

/***************************************************************************
*　　	文件功能描述：微信支付模快 —— 实体基类
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2017-2-23
*       
*****************************************************************************/

#endregion

using System;
using System.Collections.Generic;
using OSS.Common.ComModels;

namespace OSS.PayCenter.WX
{
    /// <summary>
    ///  请求基类
    /// </summary>
    public class WxPayBaseReq
    {
        private readonly SortedDictionary<string,object> _dics=new SortedDictionary<string, object>();

        public WxPayBaseReq()
        {
            var nonceStr = Guid.NewGuid().ToString().Replace("-","");
            _dics.Add("nonce_str", nonceStr);
        }

        /// <summary>
        ///  设置当前实体中涉及加密的字段
        /// </summary>
        protected virtual void SetSignDics()
        {
        }

        /// <summary>
        ///  设置加密字典条目
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        protected void SetDicItem(string key,object value)
        {
            if (!string.IsNullOrEmpty(value?.ToString()))
            {
                _dics.Add(key, value);
            }
          
        }

        public SortedDictionary<string, object> GetDics()
        {
            SetSignDics();

            _dics.Add("sign_type", "MD5");
            return _dics;
        }
    }

    /// <summary>
    ///   请求响应基类
    /// </summary>
    public class WxPayBaseResp : ResultMo
    {
        /// <summary>   
        ///    公众账号ID 必填 String(32) 调用接口提交的公众账号ID
        /// </summary>  
        public string appid { get; set; }

        /// <summary>   
        ///    商户号 必填 String(32) 调用接口提交的商户号
        /// </summary>  
        public string mch_id { get; set; }

        /// <summary>   
        ///    随机字符串 必填 String(32) 微信返回的随机字符串
        /// </summary>  
        public string nonce_str { get; set; }

        /// <summary>   
        ///    签名 必填 String(32) 微信返回的签名值，详见签名算法
        /// </summary>  
        public string sign { get; set; }

        /// <summary>   
        ///    返回状态码 必填 String(16) SUCCESS/FAIL
        ///     完全没意思，但是微信返回就收着吧
        /// </summary>  
        public string return_code { get; set; }

        /// <summary>   
        ///    返回信息 可空 String(128) 返回信息，如非空，为错误原因签名失败
        /// </summary>  
        public string return_msg { get; set; }

        private string _resultCode = string.Empty;

        /// <summary>   
        ///    业务结果 必填 String(16) SUCCESS/FAIL
        /// </summary>  
        public string result_code
        {
            get { return _resultCode; }
            set
            {
                _resultCode = value;
                if (_resultCode.ToUpper() != "SUCCESS")
                {
                    Ret = 0;
                }
            }
        }

        /// <summary>   
        ///    错误代码 可空 String(32) 详细参见下文错误列表
        /// </summary>  
        public string err_code { get; set; }

        /// <summary>   
        ///    错误代码描述 可空 String(128) 错误信息描述
        /// </summary>  
        public string err_code_des { get; set; }
    }




    public class WxPayCenterConfig
    {
        /// <summary>
        ///  应用来源，自定义字段
        /// </summary>
        public string AppSource { get; set; }

        /// <summary>
        ///  应用Id
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// 商户号id
        /// </summary>
        public string MchId { get; set; }

        /// <summary>
        /// AppSecret 值
        /// </summary>
        public string AppSecret { get; set; }

        /// <summary>
        /// 参与加密的key值
        /// </summary>
        public string Key { get; set; }
    }




}
