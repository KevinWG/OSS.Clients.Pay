namespace OSS.Clients.Pay.Wechat.Basic
{
    public class WechatCloseReq:BasePostReq<WechatCloseReq, CloseResp>
    {
        /// <summary>   
        ///   直连商户号   string[1,32]
        ///      直连商户的商户号，由微信支付生成并下发。
        /// </summary>  
        public string mchid { get; set; }

        /// <summary>   
        ///   商户订单号   string[6,32]
        ///      商户系统内部订单号，只能是数字、大小写字母_-*且在同一个商户号下唯一
        /// </summary>  
        public string out_trade_no { get; set; }

        public override string GetApiPath()
        {
            return IsSpPartnerReq
                ? "/v3/pay/partner/transactions/out-trade-no/{out_trade_no}/close"
                : "/v3/pay/transactions/out-trade-no/{out_trade_no}/close";
        }

        protected override void PrepareBodyPara()
        {
            // 无其他参数
        }

        protected override void PrepareCommonBodyPara()
        {
            if (IsSpPartnerReq)
            {
                AddBodyPara("sp_mchid", pay_config.mch_id);
                AddBodyPara("sub_mchid", sub_mch_id);
            }
            else
            {
                AddBodyPara("mchid", pay_config.mch_id);
            }
        }



    }


    public class CloseResp:BaseResp
    {

    }
}
