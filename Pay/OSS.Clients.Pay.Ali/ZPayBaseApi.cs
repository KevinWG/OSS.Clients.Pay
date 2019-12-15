
#region Copyright (C) 2017 Kevin (OSS开源作坊) 公众号：osscore

/***************************************************************************
*　　	文件功能描述：支付宝支付模快 —— 支付宝支付中心基类
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2017-3-28
*       
*****************************************************************************/

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OSS.Clients.Pay.Ali.Helpers;
using OSS.Common.BasicImpls;
using OSS.Common.Extention;
using OSS.Common.Resp;
using OSS.Tools.Http.Extention;
using OSS.Tools.Http.Mos;
using OSS.Tools.Log;

namespace OSS.Clients.Pay.Ali
{
    /// <summary>
    ///支付宝接口SDK基类
    /// </summary>
    public abstract class ZPayBaseApi: BaseApiConfigProvider<ZPayConfig>
    {
        #region  配置信息部分
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="config"></param>
        protected ZPayBaseApi(ZPayConfig config):base(config)
        {
        }
        
        private ZPayRsaHelper _rsaAssist;

        /// <summary>
        ///  加密对象提供者
        ///     为了同时满足多租户多线程上下文配置， 所以这里静态线程变量赋值，如果不存在则创建
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        private ZPayRsaHelper GenerateRsaAssist(ZPayConfig config)
        {
            if (ConfigMode==ConfigProviderMode.Context)
                return new ZPayRsaHelper(config.AppPrivateKey, config.AppPublicKey, config.Charset);
           
            if (_rsaAssist==null)
                return _rsaAssist= new ZPayRsaHelper(config.AppPrivateKey, config.AppPublicKey, config.Charset);
            
            return _rsaAssist;
        }

        #endregion

        /// <summary>
        /// 支付宝api接口地址
        /// </summary>
        protected const string m_ApiUrl = "https://openapi.alipay.com/gateway.do";


        /// <summary>
        /// 处理远程请求方法，并返回需要的实体
        /// </summary>
        /// <typeparam name="T">需要返回的实体类型</typeparam>
        /// <param name="request">远程请求组件的request基本信息</param>
        /// <param name="respColumnName">响应实体中的内容列表</param>
        /// <param name="funcFormat">获取实体格式化方法</param>
        /// <returns>实体类型</returns>
        public async Task<T> RestCommonAsync<T>(OssHttpRequest request, string respColumnName,
            Func<HttpResponseMessage, Task<T>> funcFormat = null)
            where T : ZPayBaseResp, new()
        {
            var t = default(T);
            try
            {
                request.AddressUrl = string.Concat(m_ApiUrl, "?charset=", ApiConfig.Charset);
                
                request.RequestSet = r =>
                {
                    if (r.Content == null) return;
                    var contentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded")
                    {
                        CharSet = ApiConfig.Charset
                    };
                    r.Content.Headers.ContentType = contentType;
                };

                var resp = await request.RestSend();
                if (resp.IsSuccessStatusCode)
                {
                    if (funcFormat != null)
                        t = await funcFormat(resp);
                    else
                    {
                        var contentStr = await resp.Content.ReadAsStringAsync();
                        var resJsonObj = JObject.Parse(contentStr);
                        if (resJsonObj == null)
                            return new T()
                            {
                                ret = (int) RespTypes.ObjectStateError,
                                msg = "基础请求响应不正确，请检查地址或者网络是否正常！"
                            };

                        t = resJsonObj[respColumnName].ToObject<T>();
                        if (t.IsSuccess())
                        {
                            var sign = resJsonObj["sign"].ToString();
                            var signContent = GetCehckSignContent(respColumnName, contentStr);

                            CheckSign(signContent, sign, t);
                        }
                        else
                            t.msg = string.Concat(t.msg, "-", t.sub_msg);
                    }
                }
            }
            catch (Exception ex)
            {
                var logCode = LogHelper.Error(string.Concat("基类请求出错，错误信息：", ex.Message), "Z_RestCommon",
                    ZPayConfigProvider.ModuleName);
                t = new T()
                {
                    ret = (int) RespTypes.InnerError,
                    msg = string.Concat("基类请求出错，请检查网络是否正常，错误码：", logCode)
                };
            }
            return t;
        }

        /// <summary>
        ///   发起post请求
        /// </summary>
        /// <typeparam name="TReq"></typeparam>
        /// <typeparam name="TResp"></typeparam>
        /// <param name="apiMethod"></param>
        /// <param name="respColumnName"></param>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<TResp> PostApiAsync<TReq, TResp>(string apiMethod, string respColumnName, TReq req)
            where TResp : ZPayBaseResp, new()
            where TReq : ZPayBaseReq
        {
            var contentDirs = GetReqBodyDics(apiMethod, req);
            if (!contentDirs.IsSuccess())
                return new TResp().WithResp(contentDirs);

            var reqHttp = new OssHttpRequest
            {
                HttpMethod = HttpMethod.Post,
                CustomBody = ConvertDicToEncodeReqBody(contentDirs.data)
            };
            
            return await RestCommonAsync<TResp>(reqHttp, respColumnName);
        }


        #region 验证返回签名部分

        /// <summary>
        ///  返回结果验签
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="signContent"></param>
        /// <param name="sign"></param>
        /// <param name="t"></param>
        protected void CheckSign<T>(string signContent, string sign, T t)
            where T : Resp, new()
        {
            try
            {
                var rsaAssist = GenerateRsaAssist(ApiConfig);
                var checkSignRes = rsaAssist.CheckSign(signContent, sign);
                if (checkSignRes) return;

                if (!string.IsNullOrEmpty(signContent) &&
                    signContent.Contains("\\/"))
                {
                    signContent = signContent.Replace("\\/", "/");
                    // 如果验签不通过，转义字符后再次验签
                    checkSignRes = rsaAssist.CheckSign(signContent, sign);
                }

                if (checkSignRes) return;

                t.ret = (int) RespTypes.OperateFailed;
                t.msg = "当前签名非法！";
            }
            catch (Exception e)
            {
                t.ret = (int) RespTypes.InnerError;
                t.msg = "解密签名过程中出错，详情请查看日志";
                LogHelper.Info(
                    $"解密签名过程中出错，解密内容：{signContent}, 待验证签名：{sign}, 错误信息：{e.Message}",
                    "CheckSign", ZPayConfigProvider.ModuleName);
            }
        }

        /// <summary>
        ///  获取需要验签的内容部分
        /// </summary>
        /// <param name="respColumnName"></param>
        /// <param name="contentStr"></param>
        /// <returns></returns>
        private static string GetCehckSignContent(string respColumnName, string contentStr)
        {
            var startIndex = contentStr.IndexOf(respColumnName, StringComparison.Ordinal) + respColumnName.Length + 2;
            var endIndex = contentStr.LastIndexOf(',');

            var signContent = contentStr.Substring(startIndex, endIndex - startIndex);
            return signContent;
        }

        #endregion

        #region 补充相关属性并签名

        /// <summary>
        /// 补充默认属性并返回请求内容
        /// </summary>
        /// <param name="method">接口方法名</param>
        /// <param name="req">请求实体</param>
        /// <returns>返回最终的内容</returns>
        protected internal Resp<IDictionary<string, string>> GetReqBodyDics<T>(string method, T req)
            where T : ZPayBaseReq
        {
            var dirs = new SortedDictionary<string, string>();
            try
            {
                SetDefaultPropertyFormat(dirs, "app_id", ApiConfig.AppId);
                SetDefaultPropertyFormat(dirs, "charset", ApiConfig.Charset);
                SetDefaultPropertyFormat(dirs, "method", method);
                SetDefaultPropertyFormat(dirs, "notify_url", req.GetNotifyUrl());
                SetDefaultPropertyFormat(dirs, "return_url", req.GetReturnUrl());

                SetDefaultPropertyFormat(dirs, "sign_type", "RSA2");
                SetDefaultPropertyFormat(dirs, "format", ApiConfig.Format);
                SetDefaultPropertyFormat(dirs, "version", ApiConfig.Version);
                SetDefaultPropertyFormat(dirs, "timestamp", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                SetDefaultPropertyFormat(dirs, "biz_content",
                    JsonConvert.SerializeObject(req, Formatting.None, new JsonSerializerSettings()
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        DefaultValueHandling = DefaultValueHandling.Ignore
                    }));

                if (req.app_auth_token != null)
                    SetDefaultPropertyFormat(dirs, "app_auth_token", req.app_auth_token.app_auth_token);

                //  签名
                var signContent = string.Join("&", dirs.Select(d => string.Concat(d.Key, "=", d.Value)));
                var sign = GenerateRsaAssist(ApiConfig).GenerateSign(signContent);
                dirs.Add("sign", sign);
            }
            catch (Exception e)
            {
                LogHelper.Error(string.Concat("处理签名字典出错，详细信息：", e.Message), "Z_GetReqBodyDics", ZPayConfigProvider.ModuleName);
                return new Resp<IDictionary<string, string>>().WithResp(RespTypes.InnerError, "处理签名字典出错，详细信息请查看日志");
            }
            return new Resp<IDictionary<string, string>>(dirs);
        }

        private static void SetDefaultPropertyFormat(IDictionary<string, string> dirs, string key, string value)
        {
            if (!string.IsNullOrEmpty(value))
                dirs[key] = value;
        }

        /// <summary>
        ///  转化生成签名后的请求内容
        /// </summary>
        /// <param name="dics"></param>
        /// <returns></returns>
        protected static string ConvertDicToEncodeReqBody(IDictionary<string, string> dics)
        {
            return string.Join("&", dics.Select(d => string.Concat(d.Key, "=", d.Value.UrlEncode())));
        }

        #endregion


        /// <inheritdoc />
        protected override ZPayConfig GetDefaultConfig()
        {
            return ZPayConfigProvider.DefaultConfig;
        }
    }

}
