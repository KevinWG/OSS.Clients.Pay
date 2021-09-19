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
using System.Net.Http;
using System.Net.Http.Headers;
using OSS.Clients.Pay.Wechat.Helpers;
using OSS.Common.Extension;
using OSS.Common.Helpers;
using OSS.Tools.Http;

namespace OSS.Clients.Pay.Wechat
{
    /// <summary>
    ///  请求基类
    /// </summary>
    /// <typeparam name="TResp"></typeparam>
    public abstract class WechatBaseReq<TResp> : WechatBaseReq
        where TResp:WechatBaseResp
    {
        /// <inheritdoc />
        protected WechatBaseReq(HttpMethod method) : base(method)
        {
        }
    }
    
    /// <summary>
    ///  请求基类
    /// </summary>
    public abstract class WechatBaseReq:OssHttpRequest
    {
        /// <summary>
        /// 接口请求
        /// </summary>
        /// <param name="method"></param>
        protected WechatBaseReq(HttpMethod method)
        {
            this.http_method = method;
        }
        
        #region 服务商子商户

        /// <summary>
        ///  是否是服务商请求
        /// </summary>
        public bool IsSpPartnerReq { get; internal set; } = false;

        /// <summary>
        ///  子应用id
        /// </summary>
        public string sub_app_id { get; internal set; }

        /// <summary>
        ///  子商户id
        /// </summary>
        public string sub_mch_id { get; internal set; }
        
        #endregion


        #region 支付配置

        /// <summary>
        ///  支付信息
        /// </summary>
        protected internal WechatPayConfig pay_config { get; set; }

        #endregion

        internal Dictionary<string, object> ParaDics;
        internal Dictionary<string, string> EncryptParaDics;

        internal void InternalPrepareBodyPara()
        {
            PrepareBodyPara();
        }
        /// <summary>
        ///  准备参数
        /// </summary>
        protected virtual void PrepareBodyPara()
        {
        }
        /// <summary>
        ///  获取请求接口路径地址
        /// </summary>
        /// <returns></returns>
        public abstract string GetApiPath();


        
        /// <inheritdoc />
        protected override void OnSending(HttpRequestMessage hReqMsg)
        {
            hReqMsg.Headers.UserAgent.TryParseAdd("Mozilla/5.0");
            hReqMsg.Headers.Accept.ParseAdd("application/json");
            hReqMsg.Headers.Add("Authorization", GetHeaderWithSign(this));

            if (hReqMsg.Content != null)
                hReqMsg.Content.Headers.ContentType =
                    new MediaTypeHeaderValue("application/json") { CharSet = "UTF-8" };
            
            base.OnSending(hReqMsg);
        }

        private static string GetHeaderWithSign(WechatBaseReq req)
        {
            var privateCert = WechatCertificateHelper.GetMchPrivateCertificate(req.pay_config);

            var nonce     = NumHelper.RandomNum(16);
            var timestamp = DateTime.Now.ToUtcSeconds().ToString();
            var serialNo  = privateCert.serial_number;

            var signData = GenerateSignData(req.GetApiPath(), req.http_method.ToString(), req.custom_body, timestamp, nonce);
            var signature = WechatCertificateHelper.Sign(privateCert.private_key, signData);

            var headerValue = GenerateAuthHeaderValue(req.pay_config.mch_id, serialNo, signature, timestamp, nonce);
            return headerValue;
        }

        private static string GenerateAuthHeaderValue(string mchId, string serialNo, string signature, string timestamp,
            string nonce)
        {
            var auth =
                $"mchid=\"{mchId}\",nonce_str=\"{nonce}\",timestamp=\"{timestamp}\",serial_no=\"{serialNo}\",signature=\"{signature}\"";
            return $"WECHATPAY2-SHA256-RSA2048 {auth}";
        }

        private static string GenerateSignData(string uri, string method, string body, string timestamp, string nonce)
        {
            return $"{method}\n{uri}\n{timestamp}\n{nonce}\n{body}\n";
        }
    }
}
