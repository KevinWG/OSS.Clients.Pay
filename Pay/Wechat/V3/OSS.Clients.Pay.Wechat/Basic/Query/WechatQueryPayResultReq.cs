using System;
using System.Net.Http;

namespace OSS.Clients.Pay.Wechat.Basic
{
    /// <summary>
    /// 查询支付结果请求
    /// </summary>
    public class WechatQueryPayResultReq : WechatBaseReq<QueryPayResultResp>
    {
        public WechatQueryPayResultReq() : base(HttpMethod.Get)
        {
        }

        /// <summary>   
        ///   微信支付订单号   string[1,32]
        ///     微信支付系统生成的订单号
        /// </summary>  
        public string transaction_id { get; set; }
        
        /// <summary>   
        ///   商户订单号   string[6,32]
        ///      商户系统内部订单号，只能是数字、大小写字母_-*且在同一个商户号下唯一。特殊规则：最小字符长度为6
        /// </summary>  
        public string out_trade_no { get; set; }


        public override string GetApiPath()
        {
            if (string.IsNullOrEmpty(transaction_id)&& string.IsNullOrEmpty(out_trade_no))
                throw new ArgumentException($"{nameof(transaction_id)} 和 {nameof(out_trade_no)} 不能同时为空");
            
            if (IsSpPartnerReq)
            {
                var path = string.IsNullOrEmpty(out_trade_no)
                    ? $"/v3/pay/partner/transactions/id/{transaction_id}"
                    : $"/v3/pay/partner/transactions/out-trade-no/{out_trade_no}";

                return string.Concat(path, $"?sp_mchid={pay_config.mch_id}&sub_mchid={sub_mch_id}");
            }
            else
            {
                var path = string.IsNullOrEmpty(out_trade_no) 
                    ? $"/v3/pay/transactions/id/{transaction_id}" 
                    : $"/v3/pay/transactions/out-trade-no/{out_trade_no}";

                return string.Concat(path, $"?mchid={pay_config.mch_id}");
            }
        }
    }

    public class QueryPayResultResp: WechatPayResultResp
    {
        /// <summary>   
        ///   应用ID   string[1,32]
        ///   直连商户申请的公众号或移动应用appid。
        /// </summary>  
        public string appid { get; set; }

        /// <summary>   
        ///   商户号   string[1,32]
        ///   商户的商户号，由微信支付生成并下发。
        /// </summary>  
        public string mchid { get; set; }

        /// <summary>   
        ///   服务商应用ID   string[1,32]
        ///   服务商申请的公众号或移动应用appid。
        /// </summary>  
        public string sp_appid { get; set; }

        /// <summary>   
        ///   服务商户号   string[1,32]
        ///   服务商户号，由微信支付生成并下发
        /// </summary>  
        public string sp_mchid { get; set; }

        /// <summary>   
        ///   子商户应用ID   string[1,32]
        ///   子商户申请的公众号或移动应用appid。
        /// </summary>  
        public string sub_appid { get; set; }

        /// <summary>   
        ///   子商户号   string[1,32]
        ///   子商户的商户号，由微信支付生成并下发。
        /// </summary>  
        public string sub_mchid { get; set; }

    }
}
