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
using System.Xml;
using OSS.Common.ComModels;
using OSS.Common.ComModels.Enums;
using OSS.Common.Encrypt;
using OSS.Common.Plugs;
using OSS.Common.Plugs.LogPlug;
using OSS.Http.Extention;
using OSS.Http.Mos;
using OSS.PaySdk.Wx.SysTools;

namespace OSS.PaySdk.Wx
{
    /// <summary>
    ///  微信支付基类
    /// </summary>
    public abstract class WxPayBaseApi
    {
        /// <summary>
        /// 微信api接口地址
        /// </summary>
        protected const string m_ApiUrl = "https://api.mch.weixin.qq.com";
     
        #region  处理基本配置

        /// <summary>
        ///   默认配置信息，如果实例中的构造函数配置为空可以使用当前全局配置信息
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
        protected async Task<T> RestCommonAsync<T>(OsHttpRequest request,
            Func<HttpResponseMessage, Task<T>> funcFormat = null, HttpClient client = null)
            where T : WxPayBaseResp, new()
        {
            var t = default(T);
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
                        t = GetRespResult<T>(contentStr);
                    }
                }
            }
            catch (Exception ex)
            {
                var errorKey = LogUtil.Error(string.Concat("基类请求出错，错误信息：", ex.Message), "RestCommon", ModuleNames.PayCenter);
                t = new T() { ret = -1, message = string.Concat("当前请求出错，错误码：", errorKey) };
            }
            return t;
        }

        /// <summary>
        /// 获取响应结果
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="contentStr"></param>
        /// <returns></returns>
        protected T GetRespResult<T>(string contentStr) where T : WxPayBaseResp, new()
        {
            XmlDocument resultXml = null;
            var dics = XmlDicHelper.ChangXmlToDir(contentStr, ref resultXml);

            var t = new T {RespXml = resultXml};
            t.FromResContent(dics);

            if (dics.ContainsKey("sign"))
            {
                var encryptStr = string.Join("&", dics.Select(d =>
                {
                    if (d.Key != "sign" && !string.IsNullOrEmpty(d.Value))
                        return string.Concat(d.Key, "=", d.Value);
                    return string.Empty;
                }));
                var signStr = GetSign(encryptStr);

                if (signStr != t.sign)
                {
                    t.ret = (int)ResultTypes.ParaError;
                    t.message = "返回的结果签名（sign）不匹配";
                }
            }

            if (t.return_code.ToUpper() != "SUCCESS")
            {
                //通信结果处理，这个微信做的其实没意义，脱裤子放屁
                t.ret = -1;
                t.message = t.return_msg;
            }
            else if (!t.IsSuccess())
            {
                //  请求数据结果处理
                t.message = GetErrMsg(t.err_code?.ToUpper());
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
        /// <param name="client">自定义请求客户端，当前主要是因为标准库没有提供证书设置选项，所以通过上层运行时传入设置委托，在使用证书的子类中构造客户端传入</param>
        /// <param name="dirformat">生成签名后对字典发送前的操作，例如urlencode操作</param>
        /// <returns></returns>
        protected async Task<T> PostApiAsync<T>(string addressUrl, SortedDictionary<string, object> xmlDirs,
            Func<HttpResponseMessage, Task<T>> funcFormat = null,HttpClient client=null,Action<SortedDictionary<string, object>> dirformat=null) where T : WxPayBaseResp, new()
        {
            xmlDirs.Add("appid", ApiConfig.AppId);
            xmlDirs.Add("mch_id", ApiConfig.MchId);

            CompleteDicSign(xmlDirs);
            dirformat?.Invoke(xmlDirs);

            var req = new OsHttpRequest
            {
                HttpMothed = HttpMothed.POST,
                AddressUrl = addressUrl,
                CustomBody = xmlDirs.ProduceXml()
            };

            return await RestCommonAsync<T>(req, funcFormat,client);
        }

        /// <summary>
        ///  补充完善 字典sign签名
        /// </summary>
        /// <param name="xmlDirs"></param>
        protected internal void CompleteDicSign(SortedDictionary<string, object> xmlDirs)
        {
            var encStr = string.Join("&",
                xmlDirs.Select(
                    k =>
                    {
                        var str = k.Value?.ToString();
                        return string.IsNullOrEmpty(str)
                            ? string.Empty
                            : string.Concat(k.Key, "=", str);
                    }));
            var sign = GetSign(encStr);// Md5.EncryptHexString(string.Concat(encStr, "&key=", ApiConfig.Key)).ToUpper();
            xmlDirs.Add("sign", sign);
        }

        /// <summary>
        /// 生成签名,统一方法
        /// </summary>
        /// <param name="encryptStr">不含key的参与签名串</param>
        /// <returns></returns>
        protected string GetSign(string encryptStr)
        {
            return Md5.EncryptHexString(string.Concat(encryptStr, "&key=", ApiConfig.Key)).ToUpper();
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

            m_DicErrMsg.TryAdd("NOAUTH", "商户无此接口权限 ");
            m_DicErrMsg.TryAdd("NOTENOUGH", "余额不足");
            m_DicErrMsg.TryAdd("ORDERPAID", "商户订单已支付 ");
            m_DicErrMsg.TryAdd("ORDERCLOSED", "订单已关闭 ");
            m_DicErrMsg.TryAdd("SYSTEMERROR", "商户系统接口错误");

            m_DicErrMsg.TryAdd("APPID_NOT_EXIST", "APPID不存在 ");
            m_DicErrMsg.TryAdd("MCHID_NOT_EXIST", "MCHID不存在 ");
            m_DicErrMsg.TryAdd("APPID_MCHID_NOT_MATCH", "appid和mch_id不匹配 ");
            m_DicErrMsg.TryAdd("LACK_PARAMS", "缺少参数");
            m_DicErrMsg.TryAdd("OUT_TRADE_NO_USED", "商户订单号重复 ");

            m_DicErrMsg.TryAdd("SIGNERROR", "签名错误 参数签名结果不正确  ");
            m_DicErrMsg.TryAdd("XML_FORMAT_ERROR", "XML格式错误 ");
            m_DicErrMsg.TryAdd("REQUIRE_POST_METHOD", "请使用post方法 ");
            m_DicErrMsg.TryAdd("POST_DATA_EMPTY", "post数据为空 ");
            m_DicErrMsg.TryAdd("NOT_UTF8", "编码格式错误 ");

            RegisteErrorCode("PARAM_ERROR", "参数错误 请求参数未按指引进行填写");
            RegisteErrorCode("CA_ERROR 证书有误", "确认证书正确，或者联系商户平台更新证书");
            RegisteErrorCode("CA_VERIFY_FAILED", "证书验证失败  检查证书是否正确");
            RegisteErrorCode("REQ_PARAM_XML_ERR", "输入参数xml格式有误 检查入参的xml格式是否正确");
            RegisteErrorCode("MCH_ID_EMPTY", "商户ID为空 确保商户id正确传入");

            RegisteErrorCode("ERR_VERIFY_SSL_SERIAL", "获取客户端证书序列号失败!检查证书是否正确");
            RegisteErrorCode("ERR_VERIFY_SSL_SN", "获取客户端证书特征名称(DN)域失败!检查证书是否正确");
            RegisteErrorCode("NETWORKERROR", "网络环境不佳,请重试");
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
            => m_DicErrMsg.ContainsKey(errCode) ? m_DicErrMsg[errCode] : string.Empty;

        #endregion


        private HttpClient _client;
        /// <summary>
        ///   获取设置了证书的HttpClient
        /// </summary>
        /// <returns></returns>
        protected internal HttpClient GetCertHttpClient()
        {
            if (_client != null) return _client;

            var reqHandler = new HttpClientHandler();
            ApiConfig.SetCertificata?.Invoke(reqHandler,new X509Certificate2(ApiConfig.CertPath,ApiConfig.CertPassword));
            _client = new HttpClient(reqHandler);
            return _client;
        }

    }
}
