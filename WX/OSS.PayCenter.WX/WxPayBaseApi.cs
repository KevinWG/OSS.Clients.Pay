#region Copyright (C) 2017 Kevin (OSS开源作坊) 公众号：osscoder

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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using OSS.Common.ComModels.Enums;
using OSS.Common.Encrypt;
using OSS.Common.Modules;
using OSS.Common.Modules.LogModule;
using OSS.Http;
using OSS.Http.Mos;
using OSS.PayCenter.WX.SysTools;

namespace OSS.PayCenter.WX
{
    public abstract class WxPayBaseApi
    {

        /// <summary>
        /// 微信api接口地址
        /// </summary>
        protected const string m_ApiUrl = "https://api.weixin.qq.com";

        #region  处理基本配置

        /// <summary>
        ///   默认配置信息，如果实例中的配置为空会使用当前配置信息
        /// </summary>
        public static WxPayCenterConfig DefaultConfig { get; set; }

        private readonly WxPayCenterConfig _config;

        /// <summary>
        /// 微信接口配置
        /// </summary>
        public WxPayCenterConfig ApiConfig
        {
            get { return _config ?? DefaultConfig; }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="config"></param>
        protected WxPayBaseApi(WxPayCenterConfig config)
        {
            if (config == null && DefaultConfig == null)
                throw new ArgumentNullException("config",
                    "构造函数中的config 和 全局DefaultConfig 配置信息同时为空，请通过构造函数赋值，或者在程序入口处给 DefaultConfig 赋值！");
            _config = config;
        }

        #endregion

        #region  调用基础请求方法

        /// <summary>
        /// 处理远程请求方法，并返回需要的实体
        /// </summary>
        /// <typeparam name="T">需要返回的实体类型</typeparam>
        /// <param name="request">远程请求组件的request基本信息</param>
        /// <param name="funcFormat">获取实体格式化方法</param>
        /// <param name="client">自定义请求客户端，当前主要是因为标准库没有提供证书设置选项，所以通过上层运行时传入设置委托，在使用证书的子类中构造客户端传入 </param>
        /// <returns>实体类型</returns>
        protected async Task<T> RestCommon<T>(OsHttpRequest request,
            Func<HttpResponseMessage, Task<T>> funcFormat = null, HttpClient client = null)
            where T : WxPayBaseResp, new()
        {
            T t = default(T);
            string errorKey = null;
            try
            {
                var resp = await request.RestSend(client);
                if (resp.IsSuccessStatusCode)
                {
                    if (funcFormat != null)
                        t = await funcFormat(resp);
                    else
                    {
                        var contentStr = await resp.Content.ReadAsStringAsync();
                        var dics = XmlDicHelper.ChangXmlToDir(contentStr);

                        t=new T();

                        t.SetResultDirs(dics);
                        CheckResultSign(dics, t);
                    }
                    if (t.return_code.ToUpper() != "SUCCESS")
                    {
                        //通信结果处理，这个其实没意义，脱裤子放屁
                        t.Ret = 0;
                        t.Message = t.return_msg;
                    }
                    else if (!t.IsSuccess)
                    {
                        //  请求数据结果处理
                        t.Message = GetErrMsg(t.err_code?.ToUpper());
                    }
                }
            }
            catch (Exception ex)
            {
                t = new T() {Ret = (int) ResultTypes.InnerError, Message = ex.Message};
                errorKey= LogUtil.Error(string.Concat("基类请求出错，错误信息：", ex.Message), "RestCommon", ModuleNames.PayCenter);
            }
            return t ?? new T() {Ret = 0,Message = string.Concat("当前请求出错，错误码：", errorKey) };
        }
        /// <summary>
        ///  检查返回结果的签名sign
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dics"></param>
        /// <param name="t"></param>
        protected void CheckResultSign<T>(SortedDictionary<string, string> dics, T t) where T : WxPayBaseResp, new()
        {
            var encryptStr = string.Join("&", dics.Select(d =>
            {
                if (d.Key != "sign"&&!string.IsNullOrEmpty(d.Value))
                    return string.Concat(d.Key, "=", d.Value);
                return string.Empty;
            }));
            var signStr = Md5.EncryptHexString(string.Concat(encryptStr, "&key=", ApiConfig.Key)).ToUpper();
            if (signStr == t.sign) return;

            t.Ret = (int) ResultTypes.ParaNotMeet;
            t.Message = "返回的结果签名（sign）不匹配";
        }


        /// <summary>
        ///   post 支付接口相关请求
        /// </summary>
        /// <typeparam name="T">返回参数类型</typeparam>
        /// <param name="addressUrl">接口地址</param>
        /// <param name="xmlDirs">请求参数的排序字典（不包括：appid,mch_id,nonce_str,sign_type,key,sign。 会自动补充）</param>
        /// <param name="funcFormat"></param>
        /// <param name="client">自定义请求客户端，当前主要是因为标准库没有提供证书设置选项，所以通过上层运行时传入设置委托，在使用证书的子类中构造客户端传入</param>
        /// <param name="dirformat">生成签名后对字典发送前的操作，例如urlencode操作</param>
        /// <returns></returns>
        protected async Task<T> PostPaySortDics<T>(string addressUrl, SortedDictionary<string, object> xmlDirs,
            Func<HttpResponseMessage, Task<T>> funcFormat = null,HttpClient client=null,Action<SortedDictionary<string, object>> dirformat=null) where T : WxPayBaseResp, new()
        {
            CompleteDictionarys(xmlDirs);
            dirformat?.Invoke(xmlDirs);

            var req = new OsHttpRequest();
            req.HttpMothed = HttpMothed.POST;
            req.AddressUrl = addressUrl;
            req.CustomBody = xmlDirs.ProduceXml();

            return await RestCommon<T>(req, funcFormat,client);
        }

        /// <summary>
        ///  补充完善 字典信息 如 ：appid,mch_id ，以及添加签名sign等信息
        /// </summary>
        /// <param name="xmlDirs"></param>
        protected internal void CompleteDictionarys(SortedDictionary<string, object> xmlDirs)
        {
            xmlDirs.Add("appid", ApiConfig.AppId);
            xmlDirs.Add("mch_id", ApiConfig.MchId);

            string encStr = string.Join("&",
                xmlDirs.Select(
                    k =>
                    {
                        var str = k.Value?.ToString();
                        return string.IsNullOrEmpty(str)
                            ? string.Empty
                            : string.Concat(k.Key, "=", str);
                    }));
            string sign = Md5.EncryptHexString(string.Concat(encStr, "&key=", ApiConfig.Key)).ToUpper();

            xmlDirs.Add("sign", sign);
        }

        #endregion

        #region  全局错误处理

        /// <summary>
        /// 基本错误信息字典，基类中继续完善
        /// </summary>
        protected static ConcurrentDictionary<string, string> m_DicErrMsg = new ConcurrentDictionary<string, string>();

        static WxPayBaseApi()
        {
            InitailGlobalErrorCode();
        }


        private static void InitailGlobalErrorCode()
        {
            #region 错误基本信息

            m_DicErrMsg.TryAdd("noauth", "商户无此接口权限 ");
            m_DicErrMsg.TryAdd("notenough", "余额不足");
            m_DicErrMsg.TryAdd("orderpaid", "商户订单已支付 ");
            m_DicErrMsg.TryAdd("orderclosed", "订单已关闭 ");
            m_DicErrMsg.TryAdd("systemerror", "商户系统接口错误");
            m_DicErrMsg.TryAdd("appid_not_exist", "APPID不存在 ");
            m_DicErrMsg.TryAdd("mchid_not_exist", "MCHID不存在 ");
            m_DicErrMsg.TryAdd("appid_mchid_not_match", "appid和mch_id不匹配 ");
            m_DicErrMsg.TryAdd("lack_params", "缺少参数");
            m_DicErrMsg.TryAdd("out_trade_no_used", "商户订单号重复 ");
            m_DicErrMsg.TryAdd("signerror", "签名错误 参数签名结果不正确  ");
            m_DicErrMsg.TryAdd("xml_format_error", "XML格式错误 ");
            m_DicErrMsg.TryAdd("require_post_method", "请使用post方法 ");
            m_DicErrMsg.TryAdd("post_data_empty", "post数据为空 ");
            m_DicErrMsg.TryAdd("not_utf8", "编码格式错误 ");
            RegisteErrorCode("param_error", "参数错误 请求参数未按指引进行填写");

            #endregion
        }

        /// <summary>
        /// 注册错误码
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        protected static void RegisteErrorCode(string code, string message) => m_DicErrMsg.TryAdd(code, message);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="errCode"></param>
        /// <returns></returns>
        protected static string GetErrMsg(string errCode)
            => m_DicErrMsg.ContainsKey(errCode) ? m_DicErrMsg[errCode.ToLower()] : string.Empty;

        #endregion


        private HttpClient _client;
        /// <summary>
        ///   获取设置了证书的HttpClient
        /// </summary>
        /// <returns></returns>
        protected internal HttpClient GetCertHttpClient()
        {
            if (_client==null)
            {
                var reqHandler = new HttpClientHandler();
                ApiConfig.SetCertificata?.Invoke(reqHandler,new X509Certificate2(ApiConfig.CertPath,ApiConfig.CertPassword));
                _client = new HttpClient(reqHandler);
            }
            return _client;
        }

    }
}
