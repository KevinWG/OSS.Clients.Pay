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

namespace OSS.Clients.Pay.WX
{
    /// <summary>
    /// 微信支付的配置信息
    /// </summary>
    public static class WXPayConfigProvider
    {
        /// <summary>
        ///  模块名称
        /// </summary>
        public static string ModuleName { get; set; } = "oss_pay";

        /// <summary>
        ///  默认的Key配置信息
        /// </summary>
        public static WXPayCenterConfig DefaultConfig { get; set; }


        /// <summary>
        /// 自定义底层HttpClient的实现，不设置则使用默认实现
        ///   (参数为当前使用配置信息，以及是否需要证书)
        /// </summary>
        public static Func<WXPayCenterConfig,bool, HttpClient> ClientFactory { get; set; }

    }
}
