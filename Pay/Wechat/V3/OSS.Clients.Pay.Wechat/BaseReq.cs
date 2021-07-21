#region Copyright (C) 2021 Kevin (OSS开源实验室) 公众号：osscore

/***************************************************************************
*　　	文件功能描述：微信支付模快 —— 请求基类
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2021-7-21
*       
*****************************************************************************/

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using OSS.Common.BasicMos.Resp;

namespace OSS.Clients.Pay.Wechat
{
    /// <summary>
    ///  请求基类
    /// </summary>
    public abstract class BaseReq
    {
        /// <summary>
        /// 基础请求
        /// </summary>
        /// <param name="apiUrl">去除域名部分的路径,如"/v3/..."</param>
        /// <param name="method"></param>
        public BaseReq(string apiUrl, HttpMethod method)
        {
            this.method = method;
            api_url     = apiUrl;
        }
        
        /// <summary>
        ///  接口请求地址(相对路径)
        /// </summary>
        internal string api_url { get;  }

        /// <summary>
        ///  请求方法
        /// </summary>
        internal HttpMethod method { get;  }

        /// <summary>
        ///  支付信息
        /// </summary>
        internal WechatPayConfig pay_config { get; set; }


        private Dictionary<string, string> _optionalDics; 
        /// <summary>
        ///  添加可选参数
        /// </summary>
        /// <param name="optionalPara"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public BaseReq AddOptional(string optionalPara,string value)
        {
            if (_optionalDics==null)
            {
                _optionalDics = new Dictionary<string, string>();
            }

            _optionalDics[optionalPara] = value;
            return this;
        }

        /// <summary>
        ///  设置当前请求对应的支付配置信息
        ///     仅当前请求下有效，如果配置全局信息，请设置     WechatPayConfigProvider.config  
        /// </summary>
        /// <param name="payConfig"></param>
        /// <returns></returns>
        public BaseReq SetContextConfig(WechatPayConfig payConfig)
        {
            pay_config = payConfig;
            return this;
        }

        /// <summary>
        ///  之类重写此方法返回需要发送的参数
        /// </summary>
        /// <returns></returns>
        protected virtual Dictionary<string, string> GetBodyDics()
        {
            return null;
        }

        internal string ToBody()
        {
            if (method == HttpMethod.Get)
            {
                return String.Empty;
            }

            var paras = GetBodyDics();
            if (paras == null)
            {
                throw new ArgumentException("GetBodyDics 未能获取到当前请求的必要参数");
            }

            if (_optionalDics != null)
            {
                foreach (var keyValuePair in _optionalDics)
                {
                    if (!paras.ContainsKey(keyValuePair.Key))
                    {
                        paras[keyValuePair.Key] = keyValuePair.Value;
                    }
                }
            }
            return GetReqJsonBody(paras);
        }

        private static string GetReqJsonBody(Dictionary<string,string> dics)
        {
            var str = string.Join(',', dics.Select(d => $"\"{d.Key}\":\"{d.Value}\""));
            return string.Concat("{", str, "}");
        }
    }

    /// <summary>
    ///  响应基类
    /// </summary>
    public class BaseResp:Resp
    {
        /// <summary>
        /// 响应内容
        /// </summary>
        public string response_body { get; set; }
    }

    public readonly struct HttpResponseDetail
    {
        public HttpResponseDetail(string reqId, HttpStatusCode statusCode,
            string serial_num, string body,
            string signature,
            string nonce, long timestamp)
        {
            this.status_code = statusCode;
            this.signature   = signature;

            this.body       = body;
            this.nonce      = nonce;
            this.serial_num = serial_num;

            this.timestamp  = timestamp;
            this.request_id = reqId;
        }

        public string request_id { get; }

        public string body { get; }

        public HttpStatusCode status_code { get; }

        public string signature { get; }

        public string serial_num { get; }

        public string nonce { get;  }

        public long timestamp { get; }
    }
}
