#region Copyright (C) 2021 Kevin (OSS开源实验室) 公众号：osscore

/***************************************************************************
*　　	文件功能描述：微信支付模快 —— 配置提供者
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2021-7-21
*       
*****************************************************************************/

#endregion

using System.Net.Http;
using System.Threading.Tasks;
using OSS.Clients.Pay.Wechat.Helpers;

namespace OSS.Clients.Pay.Wechat
{
    public static class WechatPayHelper
    {
        /// <summary>
        ///  支付配置信息
        /// </summary>
        public static WechatPayConfig pay_config { get; set; }

        /// <summary>
        ///   【可选】 接口请求域名，默认生产环境：https://api.mch.weixin.qq.com
        ///     如果需要测试，可自行修改为沙箱环境地址
        /// </summary>
        public static string api_domain { get; set; } = "https://api.mch.weixin.qq.com";

        /// <summary>
        ///  【可选】 自定义底层HttpClient的实现，不设置则使用默认实现
        /// </summary>
        public static IHttpClientFactory httpclient_factory { get; set; }

        /// <summary>
        ///  【可选】微信平台放公钥证书提供者
        ///    系统默认实现懒加载（需要验签或者加密时加载内存中并缓存,微信解决方案参考：https://pay.weixin.qq.com/wiki/doc/apiv3/wechatpay/wechatpay5_0.shtml
        /// </summary>
        public static IWechatCertificateProvider WechatPublicCertificateProvider { get; set; }


        /// <summary>
        /// 根据微信商户配置，验证结果签名
        /// </summary>
        /// <param name="payConfig">支付配置</param>
        /// <param name="signature">微信返回头信息中的签名</param>
        /// <param name="serialNo">微信返回头信息中的平台证书编号</param>
        /// <param name="nonce">微信返回头信息中的随机串</param>
        /// <param name="timestamp">微信返回头信息中的时间戳</param>
        /// <param name="respBody">微信返回的内容字符串</param>
        /// <returns></returns>
        public static Task<WechatBaseResp> Verify(WechatPayConfig payConfig, string signature,
                                            string serialNo, string nonce, long timestamp, string respBody)
        {
            return WechatCertificateHelper.Verify(payConfig, signature, serialNo, nonce, timestamp, respBody);
        }
        
    }
}