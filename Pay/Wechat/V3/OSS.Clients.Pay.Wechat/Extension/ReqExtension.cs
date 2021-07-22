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
        /// <typeparam name="T"></typeparam>
        /// <param name="req"></param>
        /// <param name="funcFormat"></param>
        /// <returns></returns>
        public static Task<WechatCertificateGetResp> SendAsync(this WechatCertificateGetReq req)
        {
            return req.SendAsync((config, resp) => JsonFormat<WechatCertificateGetResp>(config, resp, false));
        }

        /// <summary>
        /// 发送接口请求
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="req"></param>
        /// <param name="funcFormat"></param>
        /// <returns></returns>
        public static Task<TResp> SendAsync<TReq,TResp>(this BaseReq<TReq, TResp> req)
            where TReq : BaseReq<TReq, TResp>
            where TResp : BaseResp,new()
        {
            return SendAsync(req, (config,resp) => JsonFormat<TResp>(config, resp, true));
        }

        /// <summary>
        /// 发送接口请求
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="req"></param>
        /// <param name="funcFormat"></param>
        /// <returns></returns>
        public static async Task<TResp> SendAsync<TReq, TResp>(this BaseReq<TReq, TResp> req,
            Func<WechatPayConfig, HttpResponseDetail, Task<TResp>> funcFormat)
            where TReq : BaseReq<TReq, TResp>
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
                var bodyRes = await GetReqContent(req);
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
        private static async Task<T> JsonFormat<T>(WechatPayConfig config, HttpResponseDetail respDetail, bool checkSign)
            where T : BaseResp, new()
        {
            if (checkSign)
            {
                var vertifyRes = await CertificateHelper.Verify(config, respDetail);
                if (!vertifyRes.IsSuccess())
                {
                    var res = new T {response_body = respDetail.body, code = vertifyRes.code, message = vertifyRes.message};
                    res.ret = vertifyRes.ret;
                    return res;
                }
            }
            return JsonSerializer.Deserialize<T>(respDetail.body);
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


        private static void CheckAndSetPayConfig<TReq, TResp>(BaseReq<TReq, TResp> req)
            where TReq : BaseReq<TReq, TResp>
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

        
        //  获取http请求对象
        private static OssHttpRequest GetHttpRequest<TReq, TResp>(BaseReq<TReq, TResp> req, string reqBody)
            where TReq : BaseReq<TReq, TResp>
            where TResp : BaseResp
        {
            var privateCert = CertificateHelper.GetMchPrivateCertificate(req.pay_config);

            var nonce     = NumHelper.RandomNum(16);
            var timestamp = DateTime.Now.ToUtcSeconds().ToString();
            var serialNo  = privateCert.serial_number;

            var signData  = GenerateSignData(req.api_url, req.method.ToString(), reqBody, timestamp, nonce);
            var signature = CertificateHelper.Sign(privateCert.private_key, signData);

            var headerValue = GenerateAuthHeaderValue(req.pay_config.mch_id, serialNo, signature, timestamp, nonce);

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


        private static async Task<SendBodyResp> GetReqContent<TReq, TResp>(BaseReq<TReq, TResp> req)
            where TReq : BaseReq<TReq, TResp>
            where TResp : BaseResp
        {
            var paraDics = req.GetSendParaDics();

            if (paraDics == null)
            {
                throw new ArgumentException("未能获取到当前请求需要的必要参数");
            }

            var segBody = GetReqContent_JsonSegment(paraDics);

            var encryptParas = req.GetSendEncryptParaDics();
            if (encryptParas!=null&& encryptParas.Count>0)
            {
                var encryptBodyRes = await GetReqContent_JsonEncryptSegment(req.pay_config, encryptParas);
                if (!encryptBodyRes.IsSuccess())
                    return encryptBodyRes;

                segBody = string.Concat(segBody, ",", encryptBodyRes.body);
            }

            var reqBody = string.Concat("{", segBody, "}");
            return new SendBodyResp(){body = reqBody};
        }


        //private static void PrepareCommonPara(WechatPayConfig payConfig, Dictionary<string, string> paras)
        //{
        //    paras["appid"] = payConfig.app_id;
        //    paras["mchid"] = payConfig.mch_id;


        //    paras["sp_appid"] = payConfig.sub_app_id;
        //    paras["mchid"] = payConfig.mch_id;
        //    paras["mchid"] = payConfig.mch_id;

        //    paras["mchid"] = payConfig.mch_id;
        //}

        private static string GetReqContent_JsonSegment(Dictionary<string, string> dics)
        {
            return string.Join(',', dics.Select(d => $"\"{d.Key}\":\"{d.Value}\""));
        }

        private static async Task<SendBodyResp> GetReqContent_JsonEncryptSegment(WechatPayConfig payConfig,Dictionary<string, string> dics)
        {
            var certRes = await CertificateHelper.GetLatestCertsByConfig(payConfig);
            if (!certRes.IsSuccess())
                return certRes.ToResp<SendBodyResp>();

            var cert = certRes.item;

            var body= string.Join(',', dics.Select(d => $"\"{d.Key}\":\"{ CertificateHelper.OAEPEncrypt(cert.cert_public_key, d.Value) }\""));
            return new SendBodyResp(){body = body};
        }

        #endregion

    }


    internal class SendBodyResp:BaseResp
    {
        public string body { get; set; }
    }
}
