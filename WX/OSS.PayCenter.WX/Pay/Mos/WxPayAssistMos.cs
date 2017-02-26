#region Copyright (C) 2017 Kevin (OSS开源作坊) 公众号：osscoder

/***************************************************************************
*　　	文件功能描述：微信支付模快 —— 辅助功能实体
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2017-2-26
*       
*****************************************************************************/

#endregion
namespace OSS.PayCenter.WX.Pay.Mos
{
    /// <summary>
    ///  获取短链请求实体
    /// </summary>
    public class WxPayGetShortUrlReq:WxPayBaseReq
    {
        /// <summary>   
        ///    URL链接 必填 String(512、 需要转换的URL，签名用原串，传输需URLencode
        /// </summary>  
        public string long_url { get; set; }

        protected override void SetSignDics()
        {
            base.SetSignDics();
            SetDicItem("long_url", long_url);
        }
    }
    /// <summary>
    ///  获取短链请求实体
    /// </summary>
    public class WxPayGetShortUrlResp : WxPayBaseResp
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


}
