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
using OSS.PaySdk.Wx.Coupon.Mos;

namespace OSS.PaySdk.Wx.Coupon
{
    public class WxPayCouponApi : WxPayBaseApi
    {
        #region  初始化构造函数，全局错误处理

      

        public WxPayCouponApi(WxPayCenterConfig config = null) : base(config)
        {
        }

        #endregion

        /// <summary>
        ///  发送代金券接口
        /// </summary>
        /// <param name="conpouReq"></param>
        /// <returns></returns>
        public async Task<WxPaySendConpouResp> SendConpouAsync(WxPaySendConpouReq conpouReq)
        {
            var urlStr = string.Concat(m_ApiUrl, "/mmpaymkttransfers/send_coupon");

            return await PostApiAsync<WxPaySendConpouResp>(urlStr, conpouReq.GetDics(), null, true);
        }

        /// <summary>
        ///  查询代金券批次接口
        /// </summary>
        /// <param name="stockReq"></param>
        /// <returns></returns>
        public async Task<WxPayQueryConpouStockResp> QueryConpouStockAsync(WxPayQueryConpouStockReq stockReq )
        {
            var urlStr = string.Concat(m_ApiUrl, "/mmpaymkttransfers/query_coupon_stock");

            return await PostApiAsync<WxPayQueryConpouStockResp>(urlStr, stockReq.GetDics());
        }


        /// <summary>
        ///  查询代金券接口
        /// </summary>
        /// <param name="conpouReq"></param>
        /// <returns></returns>
        public async Task<WxPayQueryConpouStockResp> QueryConpouDetailAsync(WxPayQueryConpouReq conpouReq)
        {
            var urlStr = string.Concat(m_ApiUrl, "/mmpaymkttransfers/querycouponsinfo");

            return await PostApiAsync<WxPayQueryConpouStockResp>(urlStr, conpouReq.GetDics());
        }
    }
}
