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

namespace OSS.Clients.Pay.Wechat.Basic
{
    public class NativePayReq:BasePostReq<NativePayReq, NativePayResp>
    {
        /// <summary>
        ///  扫码请求
        /// </summary>
        public NativePayReq() 
        {
        }
        
        /// <summary>   
        ///   总金额 int
        ///   订单总金额，单位为分。
        /// </summary>  
        public int total {get; set;} 
 
        /// <summary>   
        ///   货币类型 string[1,16]
        ///   CNY：人民币，境内商户号仅支持人民币。示例值：CNY
        /// </summary>  
        public string currency {get; set;} 
 
        /// <summary>   
        ///   商品描述 string[1,127]
        ///    商品描述
        /// </summary>  
        public string description {get; set;} 
 
        /// <summary>   
        ///   商户订单号 string[6,32]
        ///    商户系统内部订单号，只能是数字、大小写字母_-*且在同一个商户号下唯一
        /// </summary>  
        public string out_trade_no {get; set;} 
 
        /// <summary>   
        ///   通知地址 string[1,256]
        ///    通知URL必须为直接可访问的URL，不允许携带查询串。格式：URL
        /// </summary>  
        public string notify_url {get; set;}

        public override string GetApiPath()
        {
            return IsSpPartnerReq
                ? "/v3/pay/partner/transactions/native"
                : "/v3/pay/transactions/native";
        }

        /// <inheritdoc />
        protected override void PrepareBodyPara()
        {
            AddBodyPara("description",description);
            AddBodyPara("out_trade_no",out_trade_no);
            AddBodyPara("notify_url",notify_url);
            AddBodyPara("amount",new {total,currency});
        }
    }

    /// <summary>
    ///  扫码支付响应实体
    /// </summary>
    public class NativePayResp : BaseResp
    {
        /// <summary> 
        ///   二维码链接 string[1,512]
        ///   此URL用于生成支付二维码，然后提供给用户扫码支付。注意：code_url并非固定值，使用时按照URL格式转成二维码即可。
        /// </summary>  
        public string code_url {get; set;} 
    }
}
