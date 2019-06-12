#region Copyright (C) 2017 Kevin (OSS开源作坊) 公众号：osscoder

/***************************************************************************
*　　	文件功能描述：支付宝支付模快 —— 支付宝支付的相关配置
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2017-3-28
*       
*****************************************************************************/

#endregion

using OSS.Common.Plugs;

namespace OSS.PaySdk.Ali
{
    /// <summary>
    /// 支付宝支付的相关配置
    /// </summary>
    public static class ZPayConfigProvider
    {
        /// <summary>
        ///  模块名称
        /// </summary>
        public static string ModuleName { get; set; } = ModuleNames.PayCenter;

        /// <summary>
        ///  默认的Key配置信息
        /// </summary>
        public static ZPayConfig DefaultConfig { get; set; }

        ///// <summary>
        ///// 设置上下文配置信息
        ///// </summary>
        ///// <param name="config"></param>
        //public static void SetContextConfig(ZPayConfig config)
        //{
        //    ZPayBaseApi.SetContextConfig(config);
        //}
    }
}
