#region Copyright (C) 2017 Kevin (OSS开源作坊) 公众号：osscore

/***************************************************************************
*　　	文件功能描述：微信支付模快 —— 支付退款模快
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2017-2-23
*       
*****************************************************************************/

#endregion

using System.Threading.Tasks;
using OSS.Clients.Pay.WX.Pay.Mos;

namespace OSS.Clients.Pay.WX.Pay
{
    public class WXPayRefundApi:WXPayBaseApi
    {
        public WXPayRefundApi(WXPayCenterConfig config=null) : base(config)
        {
        }
        
        /// <summary>
        ///  申请退款接口 【需要证书】
        /// </summary>
        /// <param name="refundReq"></param>
        /// <returns></returns>
        public async Task<WXPayRefundResp> RefundOrderAsync(WXPayRefundReq refundReq)
        {
            var dics = refundReq.GetDics();
            var url = string.Concat(m_ApiUrl, "/secapi/pay/refund");
     

            return await PostApiAsync<WXPayRefundResp>(url, dics, null, true);
        }


        /// <summary>
        /// 退款查询接口
        /// </summary>
        /// <param name="refundReq"></param>
        /// <returns></returns>
        public async Task<WXPayGetRefundResp> QueryRefundAsync(WXPayRefundReq refundReq)
        {
            var dics = refundReq.GetDics();
            var url = string.Concat(m_ApiUrl, "/pay/refundquery");

            return await PostApiAsync<WXPayGetRefundResp>(url, dics);
        }
        
        #region  关闭统一下单订单  和   撤销刷卡订单

        /// <summary> 
        ///  关闭统一下单订单
        /// 请不要和扫码撤销订单搞混
        /// </summary>
        /// <param name="out_trade_no"></param>
        /// <returns></returns>
        public async Task<WXPayBaseResp> CloseUniOrderAsync(string out_trade_no)
        {
            var addressUrl = string.Concat(m_ApiUrl, "/pay/closeorder");

            var baseReq = new WXPayBaseReq();
            var dics = baseReq.GetDics();
            dics["out_trade_no"] = out_trade_no;

            return await PostApiAsync<WXPayQueryOrderResp>(addressUrl, dics);
        }

        /// <summary>
        ///  撤销刷卡订单API，【需要证书】
        /// </summary>
        /// <param name="transaction_id">微信订单号 二选一 String(32) 微信的订单号，建议优先使用</param>
        /// <param name="out_trade_no"> 商户订单号 String(32)</param>
        /// <returns></returns>
        public async Task<WXPayResverOrderResp> ReverseMicroOrderAsync(string transaction_id, string out_trade_no)
        {
            var addressUrl = string.Concat(m_ApiUrl, "/secapi/pay/reverse");

            var baseReq = new WXPayBaseReq();
            var dics = baseReq.GetDics();
            dics["out_trade_no"] = out_trade_no;
            dics["transaction_id"] = transaction_id;

            return await PostApiAsync<WXPayResverOrderResp>(addressUrl, dics,null, true);
        }

        #endregion

    }
}