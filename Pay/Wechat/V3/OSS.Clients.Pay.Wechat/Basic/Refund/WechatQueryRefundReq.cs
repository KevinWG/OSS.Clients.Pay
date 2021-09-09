

namespace OSS.Clients.Pay.Wechat.Basic
{
    public class WechatQueryRefundReq:BaseGetReq<WechatQueryRefundReq, WechatRefundResp>
    {
        /// <summary>   
        ///   商户退款单号   string[1,
        ///   是   path商户系统内部的退款单号，商户系统内部唯一，只能是数字、大小写字母_-|*@   ，同一退款单号多次请求只退一笔。
        /// </summary>  
        public string out_refund_no { get; set; }


        public override string GetApiPath()
        {
            var queryPara = IsSpPartnerReq ? $"?sub_mchid={sub_mch_id}" : string.Empty;
            return string.Concat("/v3/refund/domestic/refunds/", out_refund_no, queryPara);
        }
    }
}
