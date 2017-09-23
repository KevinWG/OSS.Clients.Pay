#region Copyright (C) 2017 Kevin (OSS开源作坊) 公众号：osscoder

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

        static WxPayCouponApi()
        {
            RegisteErrorCode("USER_AL_GET_COUPON", "你已领取过该代金券");
            RegisteErrorCode("NETWORK ERROR", "网络环境不佳，请重试");
            RegisteErrorCode("AL_STOCK_OVER", "活动已结束");
            RegisteErrorCode("FREQ_OVER_LIMIT", "超过发放频率限制");
            RegisteErrorCode("COUPON_STOCK_ID_EMPTY", "批次ID为空");

            RegisteErrorCode("CODE_2_ID_ERR", "商户id有误");
            RegisteErrorCode("OPEN_ID_EMPTY", "用户openid为空");
            RegisteErrorCode("STOCK_IS_NOT_VALID", "抱歉，该代金券已失效");
            RegisteErrorCode("COUPON_NOT_FOUND", "券没有查找成功");
            RegisteErrorCode("COUPON_STOCK_ID_NOT_VALID", "批次id不正确");

            RegisteErrorCode("COUPON_STOCK_NOT_FOUND", "批次信息不存在");
        }

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

            return await PostApiAsync<WxPaySendConpouResp>(urlStr, conpouReq.GetDics(), null, GetCertHttpClient());
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
