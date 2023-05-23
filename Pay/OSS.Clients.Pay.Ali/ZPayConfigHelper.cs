#region Copyright (C) 2017 Kevin (OSS开源作坊) 公众号：osscore

/***************************************************************************
*　　	文件功能描述：支付宝支付模快 —— 支付宝支付的相关配置
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2017-3-28
*       
*****************************************************************************/

#endregion


namespace OSS.Clients.Pay.Ali
{
    /// <summary>
    /// 支付宝支付的相关配置
    /// </summary>
    public static class ZPayConfigHelper
    {
        /// <summary>
        ///  模块名称
        /// </summary>
        public static string ModuleName { get; set; } = "oss_pay";

        /// <summary>
        ///  默认的Key配置信息
        /// </summary>
        public static ZPayConfig DefaultConfig { get; set; }
        
    }
}
