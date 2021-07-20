
using System.Net.Http;

namespace OSS.Clients.Pay.Wechat
{
    /// <summary>
    ///  请求基类
    /// </summary>
    public abstract class BaseReq
    {
        public BaseReq(string apiUrl, HttpMethod method)
        {
            this.method = method;

            api_url    = apiUrl;
            pay_config = WechatPayConfigProvider.config;
        }
        
        /// <summary>
        ///  接口请求地址
        /// </summary>
        internal string api_url { get; set; }

        /// <summary>
        ///  请求方法
        /// </summary>
        internal HttpMethod method { get; set; }

        /// <summary>
        ///  支付信息
        /// </summary>
        internal WechatPayConfig pay_config { get; set; }
    }

    /// <summary>
    ///  响应基类
    /// </summary>
    public class BaseResp
    {

    }
}
