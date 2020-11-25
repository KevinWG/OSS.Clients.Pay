﻿
#region Copyright (C) 2017 Kevin (OSS开源作坊) 公众号：osscore

/***************************************************************************
*　　	文件功能描述：微信支付模快 —— 实体基类
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2017-2-23
*       
*****************************************************************************/

#endregion

using System.Collections.Generic;
using System.Xml;
using OSS.Clients.Pay.WX.Helpers;
using OSS.Common.BasicMos.Resp;

namespace OSS.Clients.Pay.WX
{
    /// <summary>
    ///  请求基类
    /// </summary>
    public class WXPayBaseReq
    {
        private readonly SortedDictionary<string, object> _dics = new SortedDictionary<string, object>();

        public WXPayBaseReq()
        {
            var nonceStr = WXXmlHelper.GenerateNonceStr();
            _dics["nonce_str"] = nonceStr;
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
        protected void SetDicItem(string key, object value)
        {
            if (!string.IsNullOrEmpty(value?.ToString()))
                _dics[key] = value;
        }

        public SortedDictionary<string, object> GetDics()
        {
            SetSignDics();

            //_dics["sign_type"] = "MD5";
            return _dics;
        }
    }

    /// <summary>
    ///   请求响应基类
    /// </summary>
    public class WXPayBaseResp : Resp
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
        ///   微信分配的子商户公众账号ID
        /// </summary>  
        public string sub_appid { get; set; }

        /// <summary>   
        ///   微信支付分配的子商户号
        /// </summary>  
        public string sub_mch_id { get; set; }


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
        
        /// <summary>   
        ///    业务结果 必填 String(16) SUCCESS/FAIL
        /// </summary>  
        public string result_code { get; set; }

        /// <summary>   
        ///    错误代码 可空 String(32) 详细参见下文错误列表
        /// </summary>  
        public string err_code { get; set; }

        /// <summary>   
        ///    错误代码描述 可空 String(128) 错误信息描述
        /// </summary>  
        public string err_code_des { get; set; }

        /// <summary>
        ///   响应对象的xml实体
        /// </summary>
        protected internal XmlDocument RespXml { get; set; }

        #region  处理结果字典赋值

        private SortedDictionary<string, object> _dics;

        /// <summary>
        ///  把消息对应的xml字典，给属性赋值
        /// </summary>
        /// <param name="contentDirs"></param>
        internal void FromResContent(SortedDictionary<string, object> contentDirs)
        {
            _dics = contentDirs;

            return_code = this["return_code"];
            return_msg = this["return_msg"];
            nonce_str = this["nonce_str"];

            appid = this["appid"];
            mch_id = this["mch_id"];
            sub_appid = this["sub_appid"];
            sub_mch_id = this["sub_mch_id"];

            sign = this["sign"];
            result_code = this["result_code"];
            err_code = this["err_code"];
            err_code_des = this["err_code_des"];
            
            
            FormatPropertiesFromMsg();
        }


        /// <summary>
        /// 格式化自身属性部分
        /// </summary>
        protected virtual void FormatPropertiesFromMsg()
        {
        }

        /// <summary>
        /// 自定义索引，获取指定字段的值
        /// </summary>
        /// <param name="key"></param>
        public string this[string key]
        {
            get
            {
                _dics.TryGetValue(key, out var value);
                return value?.ToString() ?? string.Empty;
            }
        }
        #endregion
    }

    /// <summary>
    ///  微信支付配置信息
    /// </summary>
    public class WXPayCenterConfig
    {
        /// <summary>
        ///  应用Id
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// 商户号id
        /// </summary>
        public string MchId { get; set; }


        /// <summary>
        /// 【非必填】 微信分配的子商户公众账号ID
        /// </summary>
        public string sub_appid { get; set; }

        /// <summary>
        /// 微信支付分配的子商户号
        /// </summary>
        public string sub_mch_id { get; set; }
        
        ///// <summary>
        ///// AppSecret 值
        ///// </summary>
        //public string AppSecret { get; set; }

        /// <summary>
        /// 参与加密的key值
        /// </summary>
        public string Key { get; set; }

        
        /// <summary>
        /// 证书路径,  请填写绝对路径，为了安全，请不要将证书放在网站目录下
        /// </summary>
        public string CertPath { get; set; }

        /// <summary>
        ///  证书密码
        /// </summary>
        public string CertPassword { get; set; }
        
        /// <summary>
        ///  如果接口参数中没有传值，则默认使用此值
        /// </summary>
        public string NotifyUrl { get; set; }
    }
}
