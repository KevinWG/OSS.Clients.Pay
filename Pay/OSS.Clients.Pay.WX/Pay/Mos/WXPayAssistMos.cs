#region Copyright (C) 2017 Kevin (OSS开源作坊) 公众号：osscore

/***************************************************************************
*　　	文件功能描述：微信支付模快 —— 辅助功能实体
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2017-2-26
*       
*****************************************************************************/

#endregion

namespace OSS.Clients.Pay.WX.Pay.Mos
{

#region  短链操作实体

    /// <summary>
    ///  获取短链请求实体
    /// </summary>
    public class WXPayGetShortUrlResp : WXPayBaseResp
    {
        /// <summary>   
        ///    URL链接 必填 String(64) 转换后的URL
        /// </summary>  
        public string short_url { get; set; }

        /// <summary>
        /// 格式化自身属性部分
        /// </summary>
        protected override void FormatPropertiesFromMsg()
        {
            base.FormatPropertiesFromMsg();
            short_url = this["short_url"];
        }
    }

    #endregion


    #region  授权码查询OPENID实体

    /// <summary>
    ///  获取短链请求实体
    /// </summary>
    public class WXPayAuthCodeOpenIdResp : WXPayBaseResp
    {
        /// <summary>   
        ///    用户标识 必填 String(128)
        /// </summary>  
        public string openid { get; set; }

        /// <summary>
        /// 格式化自身属性部分
        /// </summary>
        protected override void FormatPropertiesFromMsg()
        {
            base.FormatPropertiesFromMsg();
            openid = this["openid"];
        }
    }


    #endregion
}
