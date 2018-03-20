using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using OSS.Common.ComModels.Enums;
using OSS.Common.Plugs;
using OSS.Common.Plugs.LogPlug;
using OSS.Http.Extention;
using OSS.Http.Mos;

namespace OSS.PaySdk.Wx
{
    /// <summary>
    /// 公众号 OpenId获取
    /// </summary>
    public class MpOpenIdHelper : WxPayBaseApi
    {
        private string _accessToken;
        private DateTime _nextGetTime = DateTime.Parse("2000-1-1");
        private readonly string _appid;
        private readonly string _appSecret;

        /// <summary>
        /// 公众号 OpenId获取
        /// </summary>
        /// <param name="config"></param>
        public MpOpenIdHelper(WxPayCenterConfig config) : base(config)
        {
            _appid = config.AppId;
            _appSecret = config.AppSecret;
        }
        /// <summary>
        /// 获取 AccessToken
        /// </summary>
        /// <returns></returns>
        public string GetAccessToken()
        {
            if (_nextGetTime < DateTime.Now) {
                return GetAccessTokenFormHttp();
            } else {
                return _accessToken;
            }
        }

        private string GetAccessTokenFormHttp()
        {
            var url = string.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}", _appid, _appSecret);
            var json = WebGet(url);
            var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            if (dict.ContainsKey("access_token")) {
                _accessToken = dict["access_token"];
                _nextGetTime = DateTime.Now.AddSeconds(-3).AddSeconds(int.Parse(dict["expires_in"]));
                return _accessToken;
            }
            throw new Exception(dict["errmsg"]);
        }

        /// <summary>
        /// 获取验证地址
        /// </summary>
        /// <param name="redirectUrl"></param>
        /// <param name="state"></param>
        /// <param name="scope"></param>
        /// <param name="responseType"></param>
        /// <returns></returns>
        public string GetAuthorizeUrl(string redirectUrl, string state = "STATE", string scope = "snsapi_base", string responseType = "code")
        {
            var url = string.Format("https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type={2}&scope={3}&state={4}#wechat_redirect",
                _appid, HttpUtility.UrlEncode(redirectUrl), responseType, scope, state);

            /* 这一步发送之后，客户会得到授权页面，无论同意或拒绝，都会返回redirectUrl页面。
             * 如果用户同意授权，页面将跳转至 redirect_uri/?code=CODE&state=STATE。这里的code用于换取access_token（和通用接口的access_token不通用）
             * 若用户禁止授权，则重定向后不会带上code参数，仅会带上state参数redirect_uri?state=STATE
             */
            return url;
        }
        /// <summary>
        /// 这里通过code换取的是一个特殊的网页授权access_token,
        /// 与基础支持中的access_token（该access_token用于调用其他接口）不同。
        /// 公众号可通过下述接口来获取网页授权access_token。
        /// 如果网页授权的作用域为snsapi_base，则本步骤中获取到网页授权access_token的同时，
        /// 也获取到了openid，snsapi_base式的网页授权流程即到此为止。
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public string GetOpenId(string code)
        {
            var url = string.Format("https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code", _appid, _appSecret, code);
            var json = WebGet(url);
            var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

            if (dict.ContainsKey("openid")) {
                return dict["openid"];
            }
            throw new Exception(dict["errmsg"]);
        }

        private string WebGet(string url)
        {
            var t = WebGetTask(url);
            t.Wait();
            return t.Result;
        }

        private async Task<string> WebGetTask(string url)
        {
            var request = new OsHttpRequest {
                HttpMethod = HttpMethod.Get,
                AddressUrl = url,
            };
            var client = GetCertHttpClient();

            try {
                var resp = await request.RestSend(client);
                if (resp.IsSuccessStatusCode) {
                    return await resp.Content.ReadAsStringAsync();
                }
            } catch (Exception ex) {
                LogUtil.Error(string.Concat("基类请求出错，错误信息：", ex.Message), "WebGet", ModuleNames.PayCenter);
                throw ex;
            }
            throw new Exception("is error ");
        }
    }
    /// <summary>
    /// 企业号 OpenId获取 
    /// </summary>
    public class CorpOpenIdHelper : WxPayBaseApi
    {
        private string _accessToken;
        private DateTime _nextGetTime = DateTime.Parse("2000-1-1");
        private readonly string _appid;
        private readonly string _appSecret;

        /// <summary>
        /// 企业号 OpenId获取 
        /// </summary>
        /// <param name="config"></param>
        public CorpOpenIdHelper(WxPayCenterConfig config) : base(config)
        {
            _appid = config.AppId;
            _appSecret = config.AppSecret;
        }

        /// <summary>
        /// 获取AccessToken
        /// </summary>
        /// <returns></returns>
        public virtual string GetAccessToken()
        {
            if (_nextGetTime < DateTime.Now) {
                return GetAccessTokenFormHttp();
            } else {
                return _accessToken;
            }
        }


        private string GetAccessTokenFormHttp()
        {
            var url = string.Format("https://qyapi.weixin.qq.com/cgi-bin/gettoken?corpid={0}&corpsecret={1}", _appid, _appSecret);
            var json = WebGet(url);
            var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            if (dict.ContainsKey("access_token")) {
                _accessToken = dict["access_token"];
                _nextGetTime = DateTime.Now.AddSeconds(-3).AddSeconds(int.Parse(dict["expires_in"]));
                return _accessToken;
            }
            throw new Exception(dict["errmsg"]);
        }

        /// <summary>
        /// 企业获取code
        /// </summary>
        /// <param name="redirectUrl">授权后重定向的回调链接地址，请使用urlencode对链接进行处理</param>
        /// <param name="state">重定向后会带上state参数，企业可以填写a-zA-Z0-9的参数值</param>
        /// <param name="responseType">返回类型，此时固定为：code</param>
        /// <param name="scope">应用授权作用域，此时固定为：snsapi_base</param>
        /// #wechat_redirect 微信终端使用此参数判断是否需要带上身份信息
        /// 员工点击后，页面将跳转至 redirect_uri/?code=CODE&state=STATE，企业可根据code参数获得员工的userid。
        /// <returns></returns>
        public string GetAuthorizeUrl(string redirectUrl, string state = "STATE", string responseType = "code", string scope = "snsapi_base")
        {
            return string.Format("https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type={2}&scope={3}&state={4}#wechat_redirect", _appid, redirectUrl, responseType, scope, state);
        }

        /// <summary>
        /// 获取成员信息
        /// </summary>
        /// <param name="code">通过员工授权获取到的code，每次员工授权带上的code将不一样，code只能使用一次，5分钟未被使用自动过期</param>
        /// 权限说明：管理员须拥有agent的使用权限；agentid必须和跳转链接时所在的企业应用ID相同。
        /// <returns></returns>
        public string GetOpenId(string code)
        {
            var accessToken = GetAccessToken();
            var url = string.Format("https://qyapi.weixin.qq.com/cgi-bin/user/getuserinfo?access_token={0}&code={1}", accessToken, code);
            var json = WebGet(url);
            var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            if (dict.ContainsKey("UserId")) {
                return ConvertToOpenId(dict["UserId"]);
            }
            throw new Exception(dict["errmsg"]);
        }

        /// <summary>
        /// userid转换成openid接口
        /// </summary>
        /// <param name="userId">企业号内的成员id</param>
        /// <returns></returns>
        private string ConvertToOpenId(string userId)
        {
            var url = string.Format("https://qyapi.weixin.qq.com/cgi-bin/user/convert_to_openid?access_token={0}", GetAccessToken());
            var data = $"{{\"userid\": \"{userId}\"}}";
            var json = WebPost(url, data);
            var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

            if (dict.ContainsKey("openid")) {
                return dict["openid"];
            }
            throw new Exception(dict["errmsg"]);
        }

        private string WebPost(string url, string data)
        {
            var t = WebPostTask(url, data);
            t.Wait();
            return t.Result;
        }
        private string WebGet(string url)
        {
            var t = WebGetTask(url);
            t.Wait();
            return t.Result;
        }

        private async Task<string> WebGetTask(string url)
        {
            var request = new OsHttpRequest {
                HttpMethod = HttpMethod.Get,
                AddressUrl = url,
            };
            var client = GetCertHttpClient();

            try {
                var resp = await request.RestSend(client);
                if (resp.IsSuccessStatusCode) {
                    return await resp.Content.ReadAsStringAsync();
                }
            } catch (Exception ex) {
                LogUtil.Error(string.Concat("基类请求出错，错误信息：", ex.Message), "WebGetTask", ModuleNames.PayCenter);
                throw ex;
            }
            throw new Exception("is error ");
        }
        private async Task<string> WebPostTask(string url, string data)
        {
            var request = new OsHttpRequest {
                HttpMethod = HttpMethod.Post,
                AddressUrl = url,
                CustomBody = data
            };
            var client = GetCertHttpClient();

            try {
                var resp = await request.RestSend(client);
                if (resp.IsSuccessStatusCode) {
                    return await resp.Content.ReadAsStringAsync();
                }
            } catch (Exception ex) {
                LogUtil.Error(string.Concat("基类请求出错，错误信息：", ex.Message), "WebPostTask", ModuleNames.PayCenter);
                throw ex;
            }
            throw new Exception("is error ");
        }

    }

}
