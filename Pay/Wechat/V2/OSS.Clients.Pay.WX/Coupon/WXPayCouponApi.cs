#region Copyright (C) 2017 Kevin (OSS开源作坊) 公众号：osscore

/***************************************************************************
*　　	文件功能描述：微信支付模快 —— 支付优惠券模快
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2017-2-23
*       
*****************************************************************************/

#endregion

using System.Threading.Tasks;
using OSS.Clients.Pay.WX.Coupon.Mos;

namespace OSS.Clients.Pay.WX.Coupon
{
    public class WXPayCouponApi : WXPayBaseApi
    {
        #region  初始化构造函数，全局错误处理

      

        public WXPayCouponApi(WXPayCenterConfig config = null) : base(config)
        {
        }

        #endregion

        /// <summary>
        ///  发送代金券接口
        /// </summary>
        /// <param name="conpouReq"></param>
        /// <returns></returns>
        public async Task<WXPaySendConpouResp> SendConpouAsync(WXPaySendConpouReq conpouReq)
        {
            var urlStr = string.Concat(m_ApiUrl, "/mmpaymkttransfers/send_coupon");

            return await PostApiAsync<WXPaySendConpouResp>(urlStr, conpouReq.GetDics(), null, true);
        }

        /// <summary>
        ///  查询代金券批次接口
        /// </summary>
        /// <param name="stockReq"></param>
        /// <returns></returns>
        public async Task<WXPayQueryConpouStockResp> QueryConpouStockAsync(WXPayQueryConpouStockReq stockReq )
        {
            var urlStr = string.Concat(m_ApiUrl, "/mmpaymkttransfers/query_coupon_stock");

            return await PostApiAsync<WXPayQueryConpouStockResp>(urlStr, stockReq.GetDics());
        }


        /// <summary>
        ///  查询代金券接口
        /// </summary>
        /// <param name="conpouReq"></param>
        /// <returns></returns>
        public async Task<WXPayQueryConpouStockResp> QueryConpouDetailAsync(WXPayQueryConpouReq conpouReq)
        {
            var urlStr = string.Concat(m_ApiUrl, "/mmpaymkttransfers/querycouponsinfo");

            return await PostApiAsync<WXPayQueryConpouStockResp>(urlStr, conpouReq.GetDics());
        }
    }
}
