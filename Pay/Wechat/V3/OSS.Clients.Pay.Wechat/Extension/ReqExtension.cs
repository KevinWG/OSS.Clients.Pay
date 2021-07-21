#region Copyright (C) 2021 Kevin (OSS开源实验室) 公众号：osscore

/***************************************************************************
*　　	文件功能描述：微信支付模快 —— 请求扩展
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2021-7-21
*       
*****************************************************************************/

#endregion

using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using OSS.Clients.Pay.Wechat.Helpers;
using OSS.Common.Extension;
using OSS.Common.Helpers;
using OSS.Tools.Http.Extention;
using OSS.Tools.Http.Mos;

namespace OSS.Clients.Pay.Wechat
{
    public static class ReqExtension
    {

        /// <summary>
        /// 发送接口请求
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="req"></param>
        /// <param name="funcFormat"></param>
        /// <returns></returns>
        public static Task<T> SendAsync<T>(this BaseReq req,bool checkSign=true)
            where T : BaseResp,new ()
        {
            return SendAsync(req, (config,resp) => JsonFormat<T>(config, resp, checkSign));
        }
        


        /// <summary>
        /// 发送接口请求
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="req"></param>
        /// <param name="funcFormat"></param>
        /// <returns></returns>
        public static async Task<T> SendAsync<T>(this BaseReq req, Func<WechatPayConfig, HttpResponseDetail, Task<T>> funcFormat)
            where T : BaseResp
        {
            if (funcFormat==null)
            {
                throw new ArgumentNullException(nameof(funcFormat), "接口响应格式化方法不能为空!");
            }
            var config  = GetPayConfig(req);
            var reqBody = req.ToBody();

            var osHttpReq = GetHttpRequest(req, config, reqBody);

            var resp = await (
                WechatPayHelper.httpclient_factory == null
                    ? osHttpReq.RestSend()
                    : WechatPayHelper.httpclient_factory.CreateClient().RestSend(osHttpReq)
            );
            var respDetail =await GetResponseDetail(resp);

            return await funcFormat(req.pay_config, respDetail);
        }


        #region 响应处理
        
        private static async Task<T> JsonFormat<T>(WechatPayConfig config, HttpResponseDetail respDetail, bool checkSign)
            where T : BaseResp, new()
        {
            if (checkSign)
            {
                
            }
            return new T();
        }

        private static async Task<HttpResponseDetail> GetResponseDetail(HttpResponseMessage response)
        {
            var body       = string.Empty;
            var statusCode = response.StatusCode;
           
            response.Headers.TryGetValues("Request-ID", out var requestId);
            response.Headers.TryGetValues("Wechatpay-Nonce", out var nonce);
            response.Headers.TryGetValues("Wechatpay-Signature", out var signature);

            response.Headers.TryGetValues("Wechatpay-Timestamp", out var timestamp);
            response.Headers.TryGetValues("Wechatpay_Serial", out var serial);

            using (response)
            {
                if (statusCode!=HttpStatusCode.NoContent)
                {
                    using (var content=response.Content)
                    {
                        body =await content.ReadAsStringAsync();
                    }
                }
            }
            return new HttpResponseDetail(requestId.FirstOrDefault(),
                statusCode, serial.FirstOrDefault(), body,
                signature.FirstOrDefault(), 
                nonce.FirstOrDefault(), timestamp.FirstOrDefault().ToInt64());
        }

        #endregion


        #region 请求加工


        private static WechatPayConfig GetPayConfig(BaseReq req)
        {
            var config = req.pay_config ?? WechatPayHelper.pay_config;
            if (config == null)
            {
                throw new NotImplementedException("未发现商户支付配置信息！");
            }
            return config;
        }

        
        //  获取http请求对象
        private static OssHttpRequest GetHttpRequest(BaseReq req, WechatPayConfig config, string reqBody)
        {
            var privateCert = CertificateHelper.GetMchPrivateCertificate(config);

            var nonce     = NumHelper.RandomNum(16);
            var timestamp = DateTime.Now.ToUtcSeconds().ToString();
            var serialNo  = privateCert.serial_number;

            var signData  = GenerateSignData(req.api_url, req.method.ToString(), reqBody, timestamp, nonce);
            var signature = CertificateHelper.Sign(privateCert.private_key, signData);

            var headerValue = GenerateAuthHeaderValue(config.mch_id, serialNo, signature, timestamp, nonce);

            var osHttpReq = new OssHttpRequest();

            osHttpReq.AddressUrl = string.Concat(WechatPayHelper.api_domain, req.api_url);
            osHttpReq.HttpMethod = req.method;
            osHttpReq.CustomBody = reqBody;
            osHttpReq.RequestSet = hReqMsg => hReqMsg.Headers.Add("Authorization", headerValue);

            return osHttpReq;
        }
        
        private static string GenerateAuthHeaderValue(string mchId, string serialNo, string signature, string timestamp, string nonce)
        {
            var auth = $"mchid=\"{mchId}\",nonce_str=\"{nonce}\",timestamp=\"{timestamp}\",serial_no=\"{serialNo}\",signature=\"{signature}\"";
            return $"WECHATPAY2-SHA256-RSA2048 {auth}";
        }

        private static string GenerateSignData(string uri, string method, string body,  string timestamp, string nonce)
        {
            return $"{method}\n{uri}\n{timestamp}\n{nonce}\n{body}\n";
        }

        #endregion

    }
}
