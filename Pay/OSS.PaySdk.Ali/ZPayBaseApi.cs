
#region Copyright (C) 2017 Kevin (OSS开源作坊) 公众号：osscoder

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
using OSS.Common.ComModels;
using OSS.Common.ComModels.Enums;
using OSS.Common.Extention;
using OSS.Common.Plugs;
using OSS.Common.Plugs.LogPlug;
using OSS.Http.Extention;
using OSS.Http.Mos;
using OSS.PaySdk.Ali.SysTools;

namespace OSS.PaySdk.Ali
{
    /// <summary>
    ///支付宝接口SDK基类
    /// </summary>
    public abstract class ZPayBaseApi
    {
        #region  配置信息部分

        private readonly ZPayRsaAssist m_RsaAssist;

        /// <summary>
        ///   默认配置信息，如果实例中的配置为空会使用当前配置信息
        /// </summary>
        public static ZPayCenterConfig DefaultConfig { get; set; }

        /// <summary>
        /// 支付宝接口配置
        /// </summary>
        public ZPayCenterConfig ApiConfig { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="config"></param>
        protected ZPayBaseApi(ZPayCenterConfig config)
        {
            if (config == null && DefaultConfig == null)
                throw new ArgumentNullException(nameof(config),
                    "构造函数中的config 和 全局DefaultConfig 配置信息同时为空，请通过构造函数赋值，或者在程序入口处给 DefaultConfig 赋值！");

            ApiConfig = config ?? DefaultConfig;
            m_RsaAssist = new ZPayRsaAssist(ApiConfig.AppPrivateKey, ApiConfig.AppPublicKey, ApiConfig.SignType,
                ApiConfig.Charset);
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
        public async Task<T> RestCommonAsync<T>(OsHttpRequest request, string respColumnName,
            Func<HttpResponseMessage, Task<T>> funcFormat = null)
            where T : ZPayBaseResp, new()
        {
            var t = default(T);
            try
            {
                request.AddressUrl = string.Concat(m_ApiUrl, "?charset=", ApiConfig.Charset);

                var contentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded")
                {
                    CharSet = ApiConfig.Charset
                };
                request.RequestSet = message => message.Content.Headers.ContentType = contentType;

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
                                ret = (int) ResultTypes.ObjectStateError,
                                message = "基础请求响应不正确，请检查地址或者网络是否正常！"
                            };

                        t = resJsonObj[respColumnName].ToObject<T>();
                        if (t.IsSuccess())
                        {
                            var sign = resJsonObj["sign"].ToString();
                            var signContent = GetCehckSignContent(respColumnName, contentStr);

                            CheckSign(signContent, sign, t);
                        }
                        else
                            t.message = string.Concat(t.msg, "-", t.sub_msg);
                    }
                }
            }
            catch (Exception ex)
            {
                var logCode = LogUtil.Error(string.Concat("基类请求出错，错误信息：", ex.Message), "Z_RestCommon",
                    ModuleNames.SocialCenter);
                t = new T()
                {
                    ret = (int) ResultTypes.InnerError,
                    message = string.Concat("基类请求出错，请检查网络是否正常，错误码：", logCode)
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
                return contentDirs.ConvertToResult<TResp>();

            var reqHttp = new OsHttpRequest();

            reqHttp.HttpMothed = HttpMothed.POST;
            reqHttp.CustomBody = ConvertDicToEncodeReqBody(contentDirs.data);

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
            where T : ResultMo, new()
        {
            try
            {
                var checkSignRes = m_RsaAssist.CheckSign(signContent, sign);
                if (!checkSignRes)
                {
                    if (!string.IsNullOrEmpty(signContent) &&
                        signContent.Contains("\\/"))
                    {
                        signContent = signContent.Replace("\\/", "/");
                        // 如果验签不通过，转义字符后再次验签
                        checkSignRes = m_RsaAssist.CheckSign(signContent, sign);
                    }

                    if (checkSignRes) return;

                    t.ret = (int) ResultTypes.UnAuthorize;
                    t.message = "当前签名非法！";
                }

            }
            catch (Exception e)
            {
                t.ret = (int) ResultTypes.InnerError;
                t.message = "解密签名过程中出错，详情请查看日志";
                LogUtil.Info(
                    $"解密签名过程中出错，解密内容：{signContent}, 待验证签名：{sign}, 签名类型：{ApiConfig.SignType},  错误信息：{e.Message}",
                    "CheckSign", ModuleNames.PayCenter);
#if DEBUG
                throw e;
#endif
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
            int startIndex = contentStr.IndexOf(respColumnName, StringComparison.Ordinal) + respColumnName.Length + 2;
            int endIndex = contentStr.LastIndexOf(',');
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
        protected internal ResultMo<IDictionary<string, string>> GetReqBodyDics<T>(string method, T req)
            where T : ZPayBaseReq
        {
            SortedDictionary<string, string> dirs = new SortedDictionary<string, string>();
            try
            {
                SetDefaultPropertyFormat(dirs, "app_id", ApiConfig.AppId);
                SetDefaultPropertyFormat(dirs, "charset", ApiConfig.Charset);
                SetDefaultPropertyFormat(dirs, "method", method);
                SetDefaultPropertyFormat(dirs, "notify_url", req.GetNotifyUrl());
                SetDefaultPropertyFormat(dirs, "return_url", req.GetReturnUrl());

                SetDefaultPropertyFormat(dirs, "sign_type", ApiConfig.SignType);
                dirs.Add("timestamp", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                SetDefaultPropertyFormat(dirs, "format", ApiConfig.Format);
                SetDefaultPropertyFormat(dirs, "version", ApiConfig.Version);

                SetDefaultPropertyFormat(dirs, "biz_content",
                    JsonConvert.SerializeObject(req, Formatting.None, new JsonSerializerSettings()
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        DefaultValueHandling = DefaultValueHandling.Ignore,

                    }));

                if (req.auth_token != null)
                    SetDefaultPropertyFormat(dirs, "app_auth_token", req.auth_token.app_auth_token);

                //  签名
                string signContent = string.Join("&", dirs.Select(d => string.Concat(d.Key, "=", d.Value)));
                string sign = m_RsaAssist.GenerateSign(signContent);
                dirs.Add("sign", sign);
            }
            catch (Exception e)
            {
                LogUtil.Error(string.Concat("处理签名字典出错，详细信息：", e.Message), "Z_GetReqBodyDics", ModuleNames.PayCenter);
                return new ResultMo<IDictionary<string, string>>((int) ResultTypes.InnerError, "处理签名字典出错，详细信息请查看日志");
            }
            return new ResultMo<IDictionary<string, string>>(dirs);
        }

        private void SetDefaultPropertyFormat(SortedDictionary<string, string> dirs, string key, string value)
        {
            if (!dirs.ContainsKey(key)
                && !string.IsNullOrEmpty(value))
                dirs.Add(key, value);
        }

        protected static string ConvertDicToEncodeReqBody(IDictionary<string, string> dics)
        {
            return string.Join("&", dics.Select(d => string.Concat(d.Key, "=", d.Value.UrlEncode())));
        }

        #endregion

    }

}
