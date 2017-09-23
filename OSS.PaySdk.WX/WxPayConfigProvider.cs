#region Copyright (C) 2017 Kevin (OSS开源作坊) 公众号：osscoder

/***************************************************************************
*　　	文件功能描述：微信支付模快 —— 微信支付的配置信息
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2017-9-6
*       
*****************************************************************************/

#endregion

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
        public static string ModuleName { get; set; } = ModuleNames.PayCenter;

        /// <summary>
        ///  默认的Key配置信息
        /// </summary>
        public static WxPayCenterConfig DefaultConfig { get; set; }

        /// <summary>
        /// 设置上下文配置信息
        /// </summary>
        /// <param name="config"></param>
        public static void SetContextConfig(WxPayCenterConfig config)
        {
            WxPayBaseApi.SetContextConfig(config);
        }

    }
}
