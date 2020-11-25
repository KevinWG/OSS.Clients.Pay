﻿#region Copyright (C) 2017 Kevin (OSS开源作坊) 公众号：osscore

/***************************************************************************
*　　	文件功能描述：微信支付模快 —— 请求基类
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2017-2-23
*       
*****************************************************************************/

#endregion

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using OSS.Clients.Pay.WX.Helpers;
using OSS.Common.BasicImpls;
using OSS.Common.BasicMos.Resp;
using OSS.Common.Encrypt;
using OSS.Common.Extention;
using OSS.Tools.Http.Extention;
using OSS.Tools.Http.Mos;
using OSS.Tools.Log;

namespace OSS.Clients.Pay.WX
{
    /// <summary>
    ///  微信支付基类
    /// </summary>
    public abstract class WXPayBaseApi : BaseApiConfigProvider<WXPayCenterConfig>
    {
        /// <summary>
        /// 微信api接口地址
        /// </summary>
        protected const string m_ApiUrl = "https://api.mch.weixin.qq.com";

        #region  处理基本配置

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="config"></param>
        protected WXPayBaseApi(WXPayCenterConfig config) : base(config)
        {
       
        }

        #endregion

        #region  调用基础请求方法

        /// <summary>
        /// 处理远程请求方法，并返回需要的实体
        /// </summary>
        /// <typeparam name="T">需要返回的实体类型</typeparam>
        /// <param name="request">远程请求组件的request基本信息</param>
        /// <param name="funcFormat">获取实体格式化方法</param>
        /// <param name="needCert">是否需要双向证书</param>
        /// <param name="checkSign">是否检查返回签名，个别接口没有</param>
        /// <returns>实体类型</returns>
        protected async Task<T> RestCommonAsync<T>(OssHttpRequest request,
            Func<HttpResponseMessage, Task<T>> funcFormat , bool needCert , bool checkSign)
            where T : WXPayBaseResp, new()
        {
            var t = default(T);
            try
            {
                var client = GetCertHttpClient(needCert);

                var resp = await request.RestSend(client);
                if (resp.IsSuccessStatusCode)
                {
                    if (funcFormat != null)
                        t = await funcFormat(resp);
                    else
                    {
                        var contentStr = await resp.Content.ReadAsStringAsync();
                        t = GetRespResult<T>(contentStr);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(string.Concat("基类请求出错，错误信息：", ex.Message), "RestCommon", WXPayConfigProvider.ModuleName);
                t = new T {ret = (int) RespTypes.InnerError, msg = "微信支付请求失败"};
            }

            return t;
        }

        /// <summary>
        /// 获取响应结果
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="contentStr"></param>
        /// <param name="checkSign">是否检查返回签名，个别接口没有</param>
        /// <returns></returns>
        protected T GetRespResult<T>(string contentStr, bool checkSign = true) where T : WXPayBaseResp, new()
        {
            XmlDocument resultXml = null;
            var dics = WXXmlHelper.ChangXmlToDir(contentStr, ref resultXml);

            var t = new T {RespXml = resultXml};
            t.FromResContent(dics);


            if (t.return_code.ToUpper() != "SUCCESS"
                || t.result_code.ToUpper() != "SUCCESS")
            {
                t.ret = (int) RespTypes.ObjectStateError;
                t.msg = string.Concat(t.return_msg, t.err_code_des);
                return t;
            }

            if (checkSign)
            {
                var sign = dics["sign"]?.ToString();
                if (string.IsNullOrEmpty(sign) || sign != GetSign(GetSignContent(dics)))
                {
                    t.ret = (int) RespTypes.ParaError;
                    t.msg = "返回签名信息校验不正确！";
                }
            }
            return t;
        }


        /// <summary>
        ///   post 支付接口相关请求
        /// </summary>
        /// <typeparam name="T">返回参数类型</typeparam>
        /// <param name="addressUrl">接口地址</param>
        /// <param name="xmlDirs">请求参数的排序字典（不包括：appid,mch_id,sign。 会自动补充）</param>
        /// <param name="funcFormat"></param>
        /// <param name="needCert">是否需要双向证书</param>
        /// <param name="dirformat">生成签名后对字典发送前的操作，例如urlencode操作</param>
        /// <param name="checkSign">是否需要检查返回Sign</param>
        /// <returns></returns>
        protected async Task<T> PostApiAsync<T>(string addressUrl, SortedDictionary<string, object> xmlDirs,
            Func<HttpResponseMessage, Task<T>> funcFormat = null, bool needCert = false, bool checkSign = true,
            Action<SortedDictionary<string, object>> dirformat = null) where T : WXPayBaseResp, new()
        {
            xmlDirs.Add("appid", ApiConfig.AppId);
            xmlDirs.Add("mch_id", ApiConfig.MchId);

            CompleteDicSign(xmlDirs);
            dirformat?.Invoke(xmlDirs);

            var req = new OssHttpRequest
            {
                HttpMethod = HttpMethod.Post,
                AddressUrl = addressUrl,
                CustomBody = xmlDirs.ProduceXml()
            };

            return await RestCommonAsync<T>(req, funcFormat, needCert, checkSign);
        }

        /// <summary>
        ///  补充完善 字典sign签名
        ///     因为AppId的参数名称在不同接口中不同，所以不放在这里补充
        /// </summary>
        protected internal void CompleteDicSign(SortedDictionary<string, object> xmlDirs)
        {
            // 设置服务商子商户号信息 
            if (!string.IsNullOrEmpty(ApiConfig.sub_mch_id))
            {
                if (!string.IsNullOrEmpty(ApiConfig.sub_appid))
                    xmlDirs.Add("sub_appid", ApiConfig.sub_appid);

                xmlDirs.Add("sub_mch_id", ApiConfig.sub_mch_id);
            }

            var encStr = GetSignContent(xmlDirs);
            var sign = GetSign(encStr);
            xmlDirs.Add("sign", sign);
        }

        /// <summary> 生成签名,统一方法 </summary>
        /// <param name="encStr">不含key的参与签名串</param>
        /// <returns></returns>
        protected string GetSign(string encStr)
        {
            return Md5.EncryptHexString(string.Concat(encStr, "&key=", ApiConfig.Key)).ToUpper();
        }

        /// <summary> 获取签名内容字符串</summary>
        protected static string GetSignContent(SortedDictionary<string, object> xmlDirs)
        {
            var sb = new StringBuilder();

            foreach (var item in xmlDirs)
            {
                var value = item.Value?.ToString();
                if (item.Key == "sign" || string.IsNullOrEmpty(value)) continue;

                sb.Append(item.Key).Append("=").Append(value).Append("&");
            }

            var encStr = sb.ToString().TrimEnd('&');
            return encStr;
        }

        /// <summary>   接受微信支付通知后需要返回的信息 </summary>
        public string GetCallBackReturnXml(Resp res)
        {
            return string.Format(
                $"<xml><return_code><![CDATA[{(res.IsSuccess() ? "SUCCESS" : "FAIL")}]]></return_code><return_msg><![CDATA[{res.msg}]]></return_msg></xml>");
        }

        #endregion

        /// <summary>
        ///   获取设置了证书的HttpClient
        /// </summary>
        /// <returns></returns>
        protected internal HttpClient GetCertHttpClient(bool needCert)
        {
            var cusClient = WXPayConfigProvider.ClientFactory?.Invoke(ApiConfig, needCert);
            if (cusClient != null)
                return cusClient;

            if (!needCert)
                return null; // 返回NULL 由底层控件自己补充默认Client

            SetHandlerAndClient();

            if (needCert)
            {
                if (!_certAppIdList.Contains(ApiConfig.AppId))
                {
                    _certAppIdList.Add(ApiConfig.AppId);

                    var cert = new X509Certificate2(ApiConfig.CertPath, ApiConfig.CertPassword);
                    _handler.ClientCertificates.Add(cert);
                }
            }
            return _client;
        }

        private static HttpClient _client;
        private static HttpClientHandler _handler;
        private static long _clientCreateTime = 0;
        private static List<string> _certAppIdList=new List<string>();
        private static object _lockObj=new object();

        private static void SetHandlerAndClient()
        {
            //  两分钟创建一个新的请求
            if (_handler == null || _clientCreateTime + 120 < DateTime.Now.ToUtcSeconds())
            {
                lock (_lockObj)
                {
                    if (_handler == null || _clientCreateTime + 180 < DateTime.Now.ToUtcSeconds())
                    {
                        _clientCreateTime = DateTime.Now.ToUtcSeconds();
                        _handler = new HttpClientHandler
                        {
                            ServerCertificateCustomValidationCallback =
                                (msg, c, chain, sslErrors) => sslErrors == SslPolicyErrors.None
                        };

                        _client = new HttpClient(_handler);
                    }
                }
            }
        }

        /// <inheritdoc />
        protected override WXPayCenterConfig GetDefaultConfig()
        {
            return WXPayConfigProvider.DefaultConfig;
        }
    }
}
