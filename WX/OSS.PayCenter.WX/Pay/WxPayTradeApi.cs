#region Copyright (C) 2017 Kevin (OSS开源作坊) 公众号：osscoder

/***************************************************************************
*　　	文件功能描述：微信支付模快 —— 支付交易模快
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2017-2-23
*       
*****************************************************************************/

#endregion

using System.Net.Http;
using System.Threading.Tasks;
using OSS.PayCenter.WX.Pay.Mos;

namespace OSS.PayCenter.WX.Pay
{
    public class WxPayTradeApi : WxPayBaseApi
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="config">配置信息，如果这里为空，请在程序入口处 设置WxPayBaseApi.DefaultConfig的值</param>
        public WxPayTradeApi(WxPayCenterConfig config = null) : base(config)
        {
        }


        /// <summary>
        ///   统一下单接口
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public async Task<WxAddPayUniOrderResp> AddPayUniOrder(WxAddPayUniOrderReq order)
        {
            var dics = order.GetDics();
            dics["notify_url"] = ApiConfig.NotifyUrl;

            string addressUrl = string.Concat(m_ApiUrl, "/pay/unifiedorder");

            return await PostPaySortDics<WxAddPayUniOrderResp>(addressUrl, dics);
        }

        /// <summary>
        ///  查询订单接口
        /// </summary>
        /// <param name="queryReq"></param>
        /// <returns></returns>
        public async Task<WxQueryOrderResp> QueryOrder(WxQueryOrderReq queryReq)
        {
            var dics = queryReq.GetDics();
            var addressUrl = string.Concat(m_ApiUrl, "/pay/orderquery");

            return await PostPaySortDics<WxQueryOrderResp>(addressUrl, dics);
        }

        /// <summary>
        ///  关闭订单
        /// </summary>
        /// <param name="closeReq"></param>
        /// <returns></returns>
        public async Task<WxPayBaseResp> CloseOrder(WxPayCloseOrderReq closeReq)
        {
            var dics = closeReq.GetDics();
            var addressUrl = string.Concat(m_ApiUrl, "/pay/closeorder");

            return await PostPaySortDics<WxQueryOrderResp>(addressUrl, dics);
        }


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

    }
}
