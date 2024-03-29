﻿#region Copyright (C) 2017 Kevin (OSS开源作坊) 公众号：osscore

/***************************************************************************
*　　	文件功能描述：支付宝支付模快 —— 退款接口
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2017-4-3
*       
*****************************************************************************/

#endregion

using OSS.Clients.Pay.Ali.Pay.Mos;

namespace OSS.Clients.Pay.Ali.Pay
{
    /// <summary>
    ///  退款接口
    /// </summary>
    public class ZPayRefundApi :ZPayBaseApi
    {
        /// <summary>
        ///  构造函数
        /// </summary>
        public ZPayRefundApi() //: base(config)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="refundReq"></param>
        public async Task<ZPayRefundResp> RefunPayAsync(ZPayRefundReq refundReq)
        {
            const string respColumnName = "alipay_trade_refund_response";
            const string apiMethod = "alipay.trade.refund";

            return await PostApiAsync<ZPayRefundReq, ZPayRefundResp>(apiMethod, respColumnName, refundReq);
        }

    }
}
