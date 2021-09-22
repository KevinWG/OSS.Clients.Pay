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
using OSS.Tools.Http;

namespace OSS.Clients.Pay.Wechat
{
    /// <summary>
    ///  请求扩展
    /// </summary>
    public static class WechatReqExtension
    {
        /// <summary>
        /// 发送接口请求
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public static Task<WechatCertificateGetResp> SendAsync(this WechatCertificateGetReq req)
        {
            return SendAsync(req,(config, resp) => JsonFormat<WechatCertificateGetResp>(config, resp, false));
        }

        /// <summary>
        /// 发送接口请求
        /// </summary>
        /// <typeparam name="TResp"></typeparam>
        /// <param name="req"></param>
        /// <returns></returns>
        public static Task<TResp> SendAsync<TResp>(this WechatBaseReq<TResp> req)
            where TResp : WechatBaseResp, new()
        {
            return SendAsync(req, (config, resp) => JsonFormat<TResp>(config, resp, true));
        }

        /// <summary>
        /// 发送接口请求
        /// </summary>
        /// <param name="req"></param>
        /// <param name="funcFormat"></param>
        /// <returns></returns>
        public static async Task<TResp> SendAsync<TResp>(this WechatBaseReq<TResp> req,
            Func<WechatPayConfig, HttpResponseMessage, Task<TResp>> funcFormat)
            where TResp : WechatBaseResp, new()
        {
            if (funcFormat == null)
                throw new ArgumentNullException(nameof(funcFormat), "接口响应格式化方法不能为空!");

            PrepareBody(req);

            if (req.http_method != HttpMethod.Get)
            {
                var bodyRes = await GetPostReqBody(req);
                if (!bodyRes.IsSuccess())
                    return bodyRes.ToResp<TResp>();

                req.custom_body = bodyRes.body;
            }
            
            var resp = await (WechatPayHelper.httpclient_factory == null
                    ? ((OssHttpRequest) req).SendAsync()
                    : WechatPayHelper.httpclient_factory.CreateClient().SendAsync(req)
                );
            return await funcFormat(req.pay_config, resp);
        }
        
        #region 响应处理

        // Json 格式化处理
        private static async Task<T> JsonFormat<T>(WechatPayConfig config, HttpResponseMessage resp, bool needCheckSign)
            where T : WechatBaseResp, new()
        {
            if (!resp.IsSuccessStatusCode)
                return new T().WithResp(SysRespTypes.NetworkError, $"微信支付接口请求异常({resp.ReasonPhrase})");
            
            var respDetail = await GetResponseDetail(resp);
            if (needCheckSign)
            {
                var verifyRes = await WechatCertificateHelper.Verify(config, respDetail.signature, respDetail.serial_no,
                    respDetail.nonce, respDetail.timestamp, respDetail.body);

                if (!verifyRes.IsSuccess())
                {
                    verifyRes.request_id = respDetail.request_id;
                    return verifyRes.ToResp<T>();
                }
            }
            return string.IsNullOrEmpty(respDetail.body)
                ? new T()
                : JsonSerializer.Deserialize<T>(respDetail.body);

        }

        private static async Task<HttpResponseDetail> GetResponseDetail(HttpResponseMessage response)
        {
            var body      = string.Empty;
            var isSuccess = response.IsSuccessStatusCode;

            using (response)
            {
                if (response.StatusCode != HttpStatusCode.NoContent)
                {
                    using var content = response.Content;
                    body = await content.ReadAsStringAsync();
                }
            }

            response.Headers.TryGetValues(WechatConstKeys.RequestID, out var requestId);
            response.Headers.TryGetValues(WechatConstKeys.WechatpayNonce, out var nonce);
            response.Headers.TryGetValues(WechatConstKeys.WechatpaySignature, out var signature); 
            response.Headers.TryGetValues(WechatConstKeys.WechatpayTimestamp, out var timestamp);
            response.Headers.TryGetValues(WechatConstKeys.WechatpaySerial, out var serial);
         
            return new HttpResponseDetail(requestId?.FirstOrDefault(),
                response.StatusCode, isSuccess, 
                serial?.FirstOrDefault(), body,
                signature?.FirstOrDefault(), nonce?.FirstOrDefault(), 
                (timestamp?.FirstOrDefault().ToInt64()??0));
        }

        #endregion


        #region 请求加工
        
        /// <inheritdoc />
        private static void PrepareBody(WechatBaseReq req)
        {
            if (req.pay_config == null)
                req.SetContextConfig(WechatPayHelper.pay_config);

            if (req.pay_config == null)
                throw new NotImplementedException("未发现商户支付配置信息！");

            req.address_url = string.Concat(WechatPayHelper.api_domain, req.GetApiPath()); //GetApiPath();
            req.InternalPrepareBodyPara();
        }

        internal static async Task<SendBodyResp> GetPostReqBody(WechatBaseReq req)
        {
            var paraDics = req.ParaDics;
            if (paraDics == null)
            {
                throw new ArgumentException("未能获取到当前请求需要的必要参数");
            }

            var encryptParas = req.EncryptParaDics;
            if (encryptParas != null && encryptParas.Count > 0)
            {
                var encryptBodyRes = await GetReqContent_JsonEncryptSegment(req.pay_config, encryptParas);
                if (!encryptBodyRes.IsSuccess())
                    return encryptBodyRes.ToResp<SendBodyResp>();

                foreach (var keyValuePair in encryptBodyRes.body)
                {
                    paraDics[keyValuePair.Key] = keyValuePair.Value;
                }
            }

            var reqBody = JsonSerializer.Serialize(paraDics, _jsonOption);
            return new SendBodyResp() { body = reqBody };
        }

        private static async Task<SendEncryptBodyResp> GetReqContent_JsonEncryptSegment(WechatPayConfig payConfig, Dictionary<string, string> dics)
        {
            var certRes = await WechatCertificateHelper.GetLatestCertsByConfig(payConfig);
            if (!certRes.IsSuccess())
                return certRes.ToResp<SendEncryptBodyResp>();

            var cert = certRes.item;

            return new SendEncryptBodyResp()
            {
                body = dics.ToDictionary(
                    d => d.Key,
                    d => WechatCertificateHelper.OAEPEncrypt(cert.cert_public_key, d.Value)
                )
            };
        }

        private static readonly JsonSerializerOptions _jsonOption = new JsonSerializerOptions()
        {
            IgnoreNullValues = true
        };

        #endregion

    }


    internal class SendBodyResp : WechatBaseResp
    {
        public string body { get; set; }
    }


    internal class SendEncryptBodyResp : WechatBaseResp
    {
        public Dictionary<string, string> body { get; set; }
    }
}
