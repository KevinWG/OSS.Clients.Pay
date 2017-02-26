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
using OSS.PayCenter.WX.Pay.Mos;

namespace OSS.PayCenter.WX.Pay
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
        ///  退款接口
        /// </summary>
        /// <param name="refundReq"></param>
        /// <returns></returns>
        public async Task<WxPayRefundResp> RefundOrder(WxPayRefundReq refundReq)
        {
            var dics = refundReq.GetDics();
            var url = string.Concat(m_ApiUrl, "/secapi/pay/refund");
            var certClient = GetCertHttpClient();

            return await PostPaySortDics<WxPayRefundResp>(url, dics, null, certClient);
        }


        /// <summary>
        /// 退款查询接口
        /// </summary>
        /// <param name="refundReq"></param>
        /// <returns></returns>
        public async Task<WxPayGetRefundResp> GetRefund(WxPayRefundReq refundReq)
        {
            var dics = refundReq.GetDics();
            var url = string.Concat(m_ApiUrl, "/pay/refundquery");

            return await PostPaySortDics<WxPayGetRefundResp>(url, dics);
        }

    }
}