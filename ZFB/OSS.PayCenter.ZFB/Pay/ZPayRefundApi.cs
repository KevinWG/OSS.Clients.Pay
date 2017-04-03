#region Copyright (C) 2017 Kevin (OSS开源作坊) 公众号：osscoder

/***************************************************************************
*　　	文件功能描述：支付宝支付模快 —— 退款接口
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2017-4-3
*       
*****************************************************************************/

#endregion

using System.Threading.Tasks;
using OSS.Http.Mos;
using OSS.PayCenter.ZFB.Pay.Mos;

namespace OSS.PayCenter.ZFB.Pay
{
    /// <summary>
    ///  退款接口
    /// </summary>
    public class ZPayRefundApi:ZPayBaseApi
    {
        /// <summary>
        ///  构造函数
        /// </summary>
        /// <param name="config"></param>
        public ZPayRefundApi(ZPayCenterConfig config=null) : base(config)
        {
        }

        /// <summary>
        /// 统一预下单（收单）（扫码支付   -  用户扫商家二维码）
        /// </summary>
        /// <param name="refundReq"></param>
        public async Task<ZPayRefundResp> RefunPay(ZPayRefundReq refundReq)
        {
            const string respColumnName = "alipay_trade_refund_response";
            const string apiMethod = "alipay.trade.refund";

            var req = new OsHttpRequest();

            req.HttpMothed = HttpMothed.POST;
            req.CustomBody = ConvertDicToString(GetReqBodyDics(apiMethod, refundReq));

            return await RestCommon<ZPayRefundResp>(req, respColumnName);
        }

    }
}
