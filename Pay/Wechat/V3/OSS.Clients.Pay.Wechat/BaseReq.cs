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
    public abstract class BaseReq<TReq, TResp>
        where TReq : BaseReq<TReq, TResp>
        where TResp : BaseResp
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
        internal string api_url { get; }

        /// <summary>
        ///  请求方法
        /// </summary>
        internal HttpMethod method { get; }

        /// <summary>
        ///  支付信息
        /// </summary>
        internal WechatPayConfig pay_config { get; set; }

        

        /// <summary>
        ///  设置当前请求对应的支付配置信息
        ///     仅当前请求下有效，如果配置全局信息，请设置     WechatPayConfigProvider.config  
        /// </summary>
        /// <param name="payConfig"></param>
        /// <returns></returns>
        public TReq SetContextConfig(WechatPayConfig payConfig)
        {
            pay_config = payConfig;
            return (TReq)this;
        }

        #region 请求参数处理

        private Dictionary<string, string> _paraDics;
        /// <summary>
        ///  添加可选参数
        /// </summary>
        /// <param name="optionalPara"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public TReq AddOptionalPara(string optionalPara, string value)
        {
            if (!_paraDics.ContainsKey(optionalPara))
                AddPara(optionalPara, value);

            return (TReq)this;
        }

        /// <summary>
        ///  添加参数
        /// </summary>
        /// <param name="paraName"></param>
        /// <param name="value"></param>
        protected void AddPara(string paraName, string value)
        {
            if (_paraDics == null)
            {
                _paraDics = new Dictionary<string, string>();
            }
            _paraDics[paraName] = value;
        }

        private Dictionary<string, string> _needEncryptParaDics;
        /// <summary>
        ///  添加敏感需要加密参数
        /// </summary>
        /// <param name="paraName"></param>
        /// <param name="value"></param>
        protected void AddEncryptPara(string paraName, string value)
        {
            if (_paraDics == null)
            {
                _needEncryptParaDics = new Dictionary<string, string>();
            }
            _needEncryptParaDics[paraName] = value;
        }

        /// <summary>
        /// 发送之前准备数据，在这里完成参数处理
        /// </summary>
        /// <returns></returns>
        protected virtual void PrepareSend()
        {
        }



        internal Dictionary<string, string> GetSendParaDics()
        {
            PrepareSend();
            return _paraDics;
        }

        internal Dictionary<string, string> GetSendEncryptParaDics()
        {
            return _needEncryptParaDics;
        }

        #endregion
    }

    /// <summary>
    ///  响应基类
    /// </summary>
    public class BaseResp : Resp
    {
        private string _code;

        /// <summary>
        ///  返回错误码
        /// </summary>
        public string code
        {
            get { return _code; }
            set
            {
                _code = value;
                if (!string.IsNullOrEmpty(_code))
                {
                    ret = (int) RespTypes.OperateFailed;
                }
            }
        }


        private string _message;

        /// <summary>
        ///  返回错误码
        /// </summary>
        public string message
        {
            get { return _message; }
            set
            {
                _message = value;
                if (!string.IsNullOrEmpty(_message))
                {
                    msg = _message;
                }
            }
        }

        /// <summary>
        /// 响应内容
        /// </summary>
        public string response_body { get; set; }
    }

    internal static class RespMap
    {
        public static TResp ToResp<TResp>(this BaseResp res)
            where TResp : BaseResp, new()
        {
            var newRes = new TResp() {code = res.code, message = res.message, response_body = res.response_body};
            newRes.ret = res.ret; // 因为code赋值时会覆写，放在后边
            return newRes;
        }
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

        public string nonce { get; }

        public long timestamp { get; }
    }
}
