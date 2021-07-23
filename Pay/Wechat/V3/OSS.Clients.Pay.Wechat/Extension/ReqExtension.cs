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
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using OSS.Clients.Pay.Wechat.Basic;
using OSS.Clients.Pay.Wechat.Helpers;
using OSS.Common.BasicMos.Resp;
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
        /// <param name="req"></param>
        /// <returns></returns>
        public static Task<WechatCertificateGetResp> SendAsync(this WechatCertificateGetReq req)
        {
            return req.SendAsync((config, resp) => JsonFormat<WechatCertificateGetResp>(config, resp, false));
        }

        /// <summary>
        /// 发送接口请求
        /// </summary>
        /// <typeparam name="TReq"></typeparam>
        /// <typeparam name="TResp"></typeparam>
        /// <param name="req"></param>
        /// <returns></returns>
        public static Task<TResp> SendAsync<TReq,TResp>(this InternalBaseReq<TReq, TResp> req)
            where TReq : InternalBaseReq<TReq, TResp>
            where TResp : BaseResp,new()
        {
            return SendAsync(req, (config,resp) => JsonFormat<TResp>(config, resp, true));
        }

        /// <summary>
        /// 发送接口请求
        /// </summary>
        /// <param name="req"></param>
        /// <param name="funcFormat"></param>
        /// <returns></returns>
        public static async Task<TResp> SendAsync<TReq, TResp>(this InternalBaseReq<TReq, TResp> req,
            Func<WechatPayConfig, HttpResponseDetail, Task<TResp>> funcFormat)
            where TReq : InternalBaseReq<TReq, TResp>
            where TResp : BaseResp,new()
        {
            if (funcFormat == null)
            {
                throw new ArgumentNullException(nameof(funcFormat), "接口响应格式化方法不能为空!");
            }

            CheckAndSetPayConfig(req);

            var reqBody = string.Empty;
            if (req.method != HttpMethod.Get)
            {
                var bodyRes = await GetReqBody(req);
                if (!bodyRes.IsSuccess())
                {
                    return bodyRes.ToResp<TResp>();
                }
                reqBody = bodyRes.body;
            }

            var osHttpReq = GetHttpRequest(req,  reqBody);

            var resp = await (
                WechatPayHelper.httpclient_factory == null
                    ? osHttpReq.RestSend()
                    : WechatPayHelper.httpclient_factory.CreateClient().RestSend(osHttpReq)
            );
          
            var respDetail = await GetResponseDetail(resp);
            return await funcFormat(req.pay_config, respDetail);
        }


        #region 响应处理

        // Json 格式化处理
        private static async Task<T> JsonFormat<T>(WechatPayConfig config, HttpResponseDetail respDetail,
                                                   bool checkSign)
            where T : BaseResp, new()
        {
            if (respDetail.IsSuccessStatusCode && checkSign)
            {
                var verifyRes = await CertificateHelper.Verify(config, respDetail.signature,
                    respDetail.serial_no, respDetail.body,
                    respDetail.nonce,respDetail.timestamp);

                if (!verifyRes.IsSuccess())
                {
                    verifyRes.request_id = respDetail.request_id;
                    return verifyRes.ToResp<T>();
                }
            }
            return JsonSerializer.Deserialize<T>(respDetail.body);
        }

        private static async Task<HttpResponseDetail> GetResponseDetail(HttpResponseMessage response)
        {
            if (!(response.Headers.TryGetValues("Request-ID", out var requestId)
                  && response.Headers.TryGetValues("Wechatpay-Nonce", out var nonce) 
                  && response.Headers.TryGetValues("Wechatpay-Signature", out var signature) 
                  && response.Headers.TryGetValues("Wechatpay-Timestamp", out var timestamp)
                  && response.Headers.TryGetValues("Wechatpay-Serial", out var serial)))
            {
                throw new NotSupportedException("当前微信返回头部信息缺失!");
            }

            var body      = string.Empty;
            var isSuccess = response.IsSuccessStatusCode;
            using (response)
            {
                if (response.StatusCode!=HttpStatusCode.NoContent)
                {
                    using var content =response.Content;
                    body = await content.ReadAsStringAsync();
                }
            }
            return new HttpResponseDetail(requestId.FirstOrDefault(),
                response.StatusCode,isSuccess, serial.FirstOrDefault(), body,
                signature.FirstOrDefault(), nonce.FirstOrDefault(), timestamp.FirstOrDefault().ToInt64());
        }

        #endregion


        #region 请求加工


        private static void CheckAndSetPayConfig<TReq, TResp>(InternalBaseReq<TReq, TResp> req)
            where TReq : InternalBaseReq<TReq, TResp>
            where TResp : BaseResp
        {
            if (req.pay_config==null)
            {
                req.pay_config = WechatPayHelper.pay_config;
            }

            if (req.pay_config == null)
            {
                throw new NotImplementedException("未发现商户支付配置信息！");
            }
        }
        
        private static string GetHeaderWithSign<TReq, TResp>(InternalBaseReq<TReq, TResp> req, string signDataBody)
            where TReq : InternalBaseReq<TReq, TResp> where TResp : BaseResp
        {
            var privateCert = CertificateHelper.GetMchPrivateCertificate(req.pay_config);

            var nonce     = NumHelper.RandomNum(16);
            var timestamp = DateTime.Now.ToUtcSeconds().ToString();
            var serialNo  = privateCert.serial_number;

            var signData  = GenerateSignData(req.GetApiPath(), req.method.ToString(), signDataBody, timestamp, nonce);
            var signature = CertificateHelper.Sign(privateCert.private_key, signData);

            var headerValue = GenerateAuthHeaderValue(req.pay_config.mch_id, serialNo, signature, timestamp, nonce);
            return headerValue;
        }

        private static string GenerateAuthHeaderValue(string mchId, string serialNo, string signature, string timestamp, string nonce)
        {
            var auth = $"mchid=\"{mchId}\",nonce_str=\"{nonce}\",timestamp=\"{timestamp}\",serial_no=\"{serialNo}\",signature=\"{signature}\"";
            return $"WECHATPAY2-SHA256-RSA2048 {auth}";
        }

        private static string GenerateSignData(string uri, string method, string body, string timestamp, string nonce)
        {
            return $"{method}\n{uri}\n{timestamp}\n{nonce}\n{body}\n";
        }

        //  获取http请求对象
        private static OssHttpRequest GetHttpRequest<TReq, TResp>(InternalBaseReq<TReq, TResp> req, string reqBody)
            where TReq : InternalBaseReq<TReq, TResp>
            where TResp : BaseResp
        {
            var headerValue = GetHeaderWithSign(req, reqBody);

            var osHttpReq = new OssHttpRequest();

            osHttpReq.AddressUrl = string.Concat(WechatPayHelper.api_domain, req.GetApiPath());
            osHttpReq.HttpMethod = req.method;
            osHttpReq.CustomBody = reqBody;
            osHttpReq.RequestSet = hReqMsg =>
            {
                hReqMsg.Headers.UserAgent.TryParseAdd("Mozilla/5.0");
                hReqMsg.Headers.Accept.ParseAdd("application/json");
                hReqMsg.Headers.Add("Authorization", headerValue);
                
                if (hReqMsg.Content!=null)
                    hReqMsg.Content.Headers.ContentType =
                        new MediaTypeHeaderValue("application/json") { CharSet = "UTF-8" };
            };
            return osHttpReq;
        }

        private static async Task<SendBodyResp> GetReqBody<TReq, TResp>(InternalBaseReq<TReq, TResp> req)
            where TReq : InternalBaseReq<TReq, TResp>
            where TResp : BaseResp
        {
            var paraDics = req.GetSendParaDics();
            if (paraDics == null)
            {
                throw new ArgumentException("未能获取到当前请求需要的必要参数");
            }

            var encryptParas = req.GetSendEncryptParaDics();
            if (encryptParas!=null&& encryptParas.Count>0)
            {
                var encryptBodyRes = await GetReqContent_JsonEncryptSegment(req.pay_config, encryptParas);
                if (!encryptBodyRes.IsSuccess())
                    return encryptBodyRes.ToResp<SendBodyResp>();

                foreach (var keyValuePair in encryptBodyRes.body)
                {
                    paraDics[keyValuePair.Key] = keyValuePair.Value;
                }
            }

            var reqBody = JsonSerializer.Serialize(paraDics,_jsonOption);
            return new SendBodyResp(){body = reqBody};
        }

        private static readonly JsonSerializerOptions _jsonOption = new JsonSerializerOptions()
        {
            IgnoreNullValues = true
        };

        private static async Task<SendEncryptBodyResp> GetReqContent_JsonEncryptSegment(WechatPayConfig payConfig,Dictionary<string, string> dics)
        {
            var certRes = await CertificateHelper.GetLatestCertsByConfig(payConfig);
            if (!certRes.IsSuccess())
                return certRes.ToResp<SendEncryptBodyResp>();

            var cert = certRes.item;
            
            return new SendEncryptBodyResp()
            {
                body =dics.ToDictionary(
                    d=>d.Key,
                    d=>CertificateHelper.OAEPEncrypt(cert.cert_public_key, d.Value)
                    )
            };
        }

        #endregion

    }


    internal class SendBodyResp:BaseResp
    {
        public string body { get; set; }
    }
    
    
    internal class SendEncryptBodyResp:BaseResp
    {
        public Dictionary<string,string> body { get; set; }
    }
}
