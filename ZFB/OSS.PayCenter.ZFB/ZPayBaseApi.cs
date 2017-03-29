
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
using OSS.Http.Mos;
using OSS.Http;
using OSS.Common.ComModels.Enums;
using OSS.Common.Extention;
using OSS.Common.Modules.LogModule;
using OSS.Common.Modules;
using OSS.PayCenter.ZFB.SysTools;

namespace OSS.PayCenter.ZFB
{
    /// <summary>
    ///支付宝接口SDK基类
    /// </summary>
    public abstract class ZPayBaseApi
    {
        #region  配置信息部分
        /// <summary>
        ///   默认配置信息，如果实例中的配置为空会使用当前配置信息
        /// </summary>
        public static ZPayCenterConfig DefaultConfig { get; set; }

        private readonly ZPayCenterConfig _config;

        /// <summary>
        /// 支付宝接口配置
        /// </summary>
        public ZPayCenterConfig ApiConfig => _config ?? DefaultConfig;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="config"></param>
        protected ZPayBaseApi(ZPayCenterConfig config)
        {
            if (config == null && DefaultConfig == null)
                throw new ArgumentNullException(nameof(config),
                    "构造函数中的config 和 全局DefaultConfig 配置信息同时为空，请通过构造函数赋值，或者在程序入口处给 DefaultConfig 赋值！");
            _config = config;
        }

        #endregion

        /// <summary>
        /// 支付宝api接口地址
        /// </summary>
        protected const string m_ApiUrl = "https://openapi.alipaydev.com/gateway.do";

        /// <summary>
        /// 处理远程请求方法，并返回需要的实体
        /// </summary>
        /// <typeparam name="T">需要返回的实体类型</typeparam>
        /// <param name="request">远程请求组件的request基本信息</param>
        /// <param name="respColumnName">响应实体中的内容列表</param>
        /// <param name="funcFormat">获取实体格式化方法</param>
        /// <returns>实体类型</returns>
        public async Task<T> RestCommon<T>(OsHttpRequest request, string respColumnName,
            Func<HttpResponseMessage, Task<T>> funcFormat = null)
            where T : ZPayBaseResp, new()
        {
            var t = default(T);
            try
            {
                request.AddressUrl = string.Concat(m_ApiUrl, "?charset=", ApiConfig.Charset);

                var contentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded"){ CharSet = ApiConfig.Charset };
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
                            return new T(){ Ret = (int) ResultTypes.ObjectStateError,Message = "基础请求响应不正确，请检查地址或者网络是否正常！"};
                        
                        t = resJsonObj[respColumnName].ToObject<T>();
                        if (t.IsSuccess)
                        {
                            var sign = resJsonObj["sign"].ToString();
                            var signContent = GetCehckSignContent<T>(respColumnName, contentStr);

                            CheckSign(signContent, sign, t);
                        }
                        else
                            t.Message = string.Concat(t.msg, "-", t.sub_msg);
                    }
                }
            }
            catch (Exception ex)
            {
                var logCode = LogUtil.Error(string.Concat("基类请求出错，错误信息：", ex.Message), "RestCommon",
                    ModuleNames.SocialCenter);
                t = new T()
                {
                    Ret = (int) ResultTypes.InnerError,
                    Message = string.Concat("基类请求出错，请检查网络是否正常，错误码：", logCode)
                };
            }
            return t;
        }

        /// <summary>
        ///  返回结果验签
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="signContent"></param>
        /// <param name="sign"></param>
        /// <param name="t"></param>
        private void CheckSign<T>(string signContent, string sign, T t) where T : ZPayBaseResp, new()
        {
            var checkSignRes = ZPaySignature.RSACheckContent(signContent, sign, ApiConfig.AppPublicKey,
                ApiConfig.Charset, ApiConfig.SignType);
            if (!checkSignRes)
            {
                if (!string.IsNullOrEmpty(signContent) &&
                    signContent.Contains("\\/"))
                {
                    signContent = signContent.Replace("\\/", "/");
                    // 如果验签不通过，转义字符后再次验签
                    checkSignRes = ZPaySignature.RSACheckContent(signContent, sign,
                        ApiConfig.AppPublicKey, ApiConfig.Charset, ApiConfig.SignType);
                    if (!checkSignRes)
                    {
                        t.Ret = (int) ResultTypes.UnAuthorize;
                        t.Message = "当前签名非法！";
                    }
                }
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

        #region 补充相关属性并签名

        /// <summary>
        /// 补充默认属性并返回请求内容
        /// </summary>
        /// <param name="method">接口方法名</param>
        /// <param name="req">请求实体</param>
        /// <returns>返回最终的内容</returns>
        protected internal IDictionary<string,string> GetReqBodyDics<T>(string method,T req)
            where T:ZPayBaseReq
        {
            SortedDictionary<string, string> dirs = new SortedDictionary<string, string>();

            SetDefaultPropertyFormat(dirs, "app_id", ApiConfig.AppId);
            SetDefaultPropertyFormat(dirs, "charset", ApiConfig.Charset);
            SetDefaultPropertyFormat(dirs, "method", method);
            SetDefaultPropertyFormat(dirs, "notify_url", req.GetNotifyUrl());
            SetDefaultPropertyFormat(dirs, "sign_type", ApiConfig.SignType);

            var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            dirs.Add("timestamp", timestamp);
            SetDefaultPropertyFormat(dirs, "format", ApiConfig.Format);
            SetDefaultPropertyFormat(dirs, "version", ApiConfig.Version);

            SetDefaultPropertyFormat(dirs, "biz_content",
                JsonConvert.SerializeObject(req, Formatting.Indented, new JsonSerializerSettings()
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    DefaultValueHandling = DefaultValueHandling.Ignore,

                }));

            if (req.auth_token!=null)
                SetDefaultPropertyFormat(dirs, "app_auth_token", req.auth_token.app_auth_token);
            
            //  签名
            string signContent = string.Join("&", dirs.Select(d => string.Concat(d.Key, "=", d.Value)));
            string sign = ZPaySignature.RSASignCharSet(signContent, ApiConfig.AppPrivateKey, ApiConfig.Charset,
                ApiConfig.SignType);
            dirs.Add("sign", sign);

            return dirs;
        }

        private void SetDefaultPropertyFormat(SortedDictionary<string, string> dirs, string key, string value)
        {
            if (!dirs.ContainsKey(key)
                && !string.IsNullOrEmpty(value))
                dirs.Add(key, value);
        }

        protected static string ConvertDicToString(IDictionary<string,string> dics )
        {
            return string.Join("&", dics.Select(d => string.Concat(d.Key, "=",d.Value.UrlEncode())));
        }

        #endregion

    }

}
