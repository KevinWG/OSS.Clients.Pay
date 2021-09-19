#region Copyright (C) 2021 Kevin (OSS开源实验室) 公众号：osscore

/***************************************************************************
*　　	文件功能描述：微信支付模快 —— 用户扫码支付请求
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2021-7-21
*       
*****************************************************************************/

#endregion

using System.Net.Http;

namespace OSS.Clients.Pay.Wechat.Basic
{
    /// <summary>
    ///  获取支付二维码
    /// </summary>
    public abstract class WechatPayBasicReq<TResp> : WechatBaseReq<TResp>
        where TResp : WechatBaseResp
    {
        /// <summary>
        ///  扫码请求
        /// </summary>
        public WechatPayBasicReq() : base(HttpMethod.Post)
        {
        }
        
        /// <summary>   
        ///   订单金额   object
        ///      订单金额信息
        /// </summary>  
        public WechatPayAmount amount { get; set; }

        /// <summary>   
        ///   商品描述 string[1,127]
        ///    商品描述
        /// </summary>  
        public string description { get; set; }

        /// <summary>   
        ///   商户订单号 string[6,32]
        ///    商户系统内部订单号，只能是数字、大小写字母_-*且在同一个商户号下唯一
        /// </summary>  
        public string out_trade_no { get; set; }

        /// <summary>   
        ///   通知地址 string[1,256]
        ///    通知URL必须为直接可访问的URL，不允许携带查询串。格式：URL
        /// </summary>  
        public string notify_url { get; set; }


        /// <inheritdoc />
        protected override void PrepareBodyPara()
        {
            if (IsSpPartnerReq)
            {
                this.AddBodyPara("sp_appid", pay_config.app_id);
                this.AddBodyPara("sp_mchid", pay_config.mch_id);
                this.AddBodyPara("sub_appid", sub_app_id);
                this.AddBodyPara("sub_mchid", sub_mch_id);
            }
            else
            {
                this.AddBodyPara("appid", pay_config.app_id);
                this.AddBodyPara("mchid", pay_config.mch_id);
            }

            this.AddBodyPara("description", description);
            this.AddBodyPara("out_trade_no", out_trade_no);
            this.AddBodyPara("notify_url", notify_url);
            this.AddBodyPara("amount", amount);
        }
    }


    public class WechatPayAmount
    {
        /// <summary>   
        ///   总金额 int
        ///   订单总金额，单位为分。
        /// </summary>  
        public int amount { get; set; }

        /// <summary>   
        ///   货币类型 string[1,16]
        ///   CNY：人民币，境内商户号仅支持人民币。示例值：CNY
        /// </summary>  
        public string currency { get; set; }
    }
}
