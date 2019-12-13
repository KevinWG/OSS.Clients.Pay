#region Copyright (C) 2017 Kevin (OSS开源作坊) 公众号：osscore

/***************************************************************************
*　　	文件功能描述：微信支付模快 —— 微信支付的配置信息
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2017-9-6
*       
*****************************************************************************/

#endregion

using System;
using System.Net.Http;
using OSS.Common.ComModels;
using OSS.Common.Plugs;

namespace OSS.PaySdk.Wx
{
    /// <summary>
    /// 微信支付的配置信息
    /// </summary>
    public static class WxPayConfigProvider
    {
        /// <summary>
        ///  模块名称
        /// </summary>
        public static string ModuleName { get; set; } = "oss_pay";

        /// <summary>
        ///  默认的Key配置信息
        /// </summary>
        public static WxPayCenterConfig DefaultConfig { get; set; }

        ///// <summary>
        ///// 设置上下文配置信息
        ///// </summary>
        ///// <param name="config"></param>
        //public static void SetContextConfig(WxPayCenterConfig config)
        //{
        //    WxPayBaseApi.SetContextConfig(config);
        //}


        /// <summary>
        /// 自定义底层HttpClient的实现，不设置则使用默认实现
        ///   1. 如果是非证书请求，使用底层OSS.Http的默认实现
        ///   2. 如果当前接口需要使用证书验证且配置为Instance模式，会在当前实例下使用单独 Httpclient
        ///   3. 如果当前接口需要使用证书验证且配置为Context模式，每次接口请求都会创建新的 Httpclient
        ///  
        ///   (配置信息，是否需要证书)： HttpClient
        /// </summary>
        public static Func<WxPayCenterConfig,bool, HttpClient>  HttpClientProvider { get; set; }

    }
}
