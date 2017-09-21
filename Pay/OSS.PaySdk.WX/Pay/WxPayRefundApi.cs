#region Copyright (C) 2017 Kevin (OSS开源作坊) 公众号：osscoder

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
using OSS.PaySdk.Wx.Pay.Mos;

namespace OSS.PaySdk.Wx.Pay
{
    public class WxPayRefundApi:WxPayBaseApi
    {
        public WxPayRefundApi(WxPayCenterConfig config) : base(config)
        {
        }

        #region   全局错误码注册

        static WxPayRefundApi()
        {
            #region  申请退款全局错误码
            RegisteErrorCode("ERROR", "业务错误");
            RegisteErrorCode("USER_ACCOUNT_ABNORMAL", "退款请求失败用户帐号注销 此状态代表退款申请失败，商户可自行处理退款。");
            RegisteErrorCode("NOTENOUGH", "余额不足");
            RegisteErrorCode("INVALID_TRANSACTIONID", "无效transaction_id");
            #endregion
        }

        #endregion

        /// <summary>
        ///  申请退款接口 【需要证书】
        /// </summary>
        /// <param name="refundReq"></param>
        /// <returns></returns>
        public async Task<WxPayRefundResp> RefundOrderAsync(WxPayRefundReq refundReq)
        {
            var dics = refundReq.GetDics();
            var url = string.Concat(m_ApiUrl, "/secapi/pay/refund");
            var certClient = GetCertHttpClient();

            return await PostApiAsync<WxPayRefundResp>(url, dics, null, certClient);
        }


        /// <summary>
        /// 退款查询接口
        /// </summary>
        /// <param name="refundReq"></param>
        /// <returns></returns>
        public async Task<WxPayGetRefundResp> GetRefundAsync(WxPayRefundReq refundReq)
        {
            var dics = refundReq.GetDics();
            var url = string.Concat(m_ApiUrl, "/pay/refundquery");

            return await PostApiAsync<WxPayGetRefundResp>(url, dics);
        }
        
        #region  关闭统一下单订单  和   撤销刷卡订单

        /// <summary> 
        ///  关闭统一下单订单
        /// 请不要和扫码撤销订单搞混
        /// </summary>
        /// <param name="out_trade_no"></param>
        /// <returns></returns>
        public async Task<WxPayBaseResp> CloseUniOrderAsync(string out_trade_no)
        {
            var addressUrl = string.Concat(m_ApiUrl, "/pay/closeorder");

            var baseReq = new WxPayBaseReq();
            var dics = baseReq.GetDics();
            dics["out_trade_no"] = out_trade_no;

            return await PostApiAsync<WxPayQueryOrderResp>(addressUrl, dics);
        }

        /// <summary>
        ///  撤销刷卡订单API，【需要证书】
        /// </summary>
        /// <param name="transaction_id">微信订单号 二选一 String(32) 微信的订单号，建议优先使用</param>
        /// <param name="out_trade_no"> 商户订单号 String(32)</param>
        /// <returns></returns>
        public async Task<WxPayResverOrderResp> ReverseMicroOrderAsync(string transaction_id, string out_trade_no)
        {
            var addressUrl = string.Concat(m_ApiUrl, "/secapi/pay/reverse");

            var baseReq = new WxPayBaseReq();
            var dics = baseReq.GetDics();
            dics["out_trade_no"] = out_trade_no;
            dics["transaction_id"] = transaction_id;

            return await PostApiAsync<WxPayResverOrderResp>(addressUrl, dics,null, GetCertHttpClient());
        }

        #endregion

    }
}