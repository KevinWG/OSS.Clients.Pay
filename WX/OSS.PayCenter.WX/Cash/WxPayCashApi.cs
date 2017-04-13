#region Copyright (C) 2017 Kevin (OSS开源作坊) 公众号：osscoder

/***************************************************************************
*　　	文件功能描述：微信支付模快 —— 小额现金模快
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2017-2-23
*       
*****************************************************************************/

#endregion

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OSS.Http.Mos;
using OSS.PayCenter.WX.Cash.Mos;
using OSS.PayCenter.WX.SysTools;

namespace OSS.PayCenter.WX.Cash
{
    public class WxPayCashApi:WxPayBaseApi
    {
        public WxPayCashApi(WxPayCenterConfig config) : base(config)
        {
        }

        /// <summary>
        ///  企业付款接口
        /// </summary>
        /// <param name="cashReq"></param>
        /// <returns></returns>
        public async Task<WxPayTransferCashResp> TransferCashAsync(WxPayTransferCashReq cashReq)
        {
            var addressUrl = string.Concat(m_ApiUrl, "/mmpaymkttransfers/promotion/transfers");
            var dics = cashReq.GetDics();

            dics.Add("mch_appid", ApiConfig.AppId);
            dics.Add("mch_id", ApiConfig.MchId);

            CompleteDicSign(dics);

            var req = new OsHttpRequest();
            req.HttpMothed = HttpMothed.POST;
            req.AddressUrl = addressUrl;
            req.CustomBody = dics.ProduceXml();

            return await RestCommonAsync<WxPayTransferCashResp>(req, null, GetCertHttpClient());
        }


        /// <summary>
        /// 获取企业付款信息
        /// </summary>
        /// <param name="partner_trade_no"></param>
        /// <returns></returns>
        public async Task<WxPayGetTransferCashResp> GetTransferCashAsync(string partner_trade_no)
        {
            var urlStr = string.Concat(m_ApiUrl, "/mmpaymkttransfers/gettransferinfo");

            var dics = new SortedDictionary<string,object>();
            dics["nonce_str"] = Guid.NewGuid().ToString().Replace("-", "");
            dics["partner_trade_no"] = partner_trade_no;

            return await PostPaySortDicsAsync<WxPayGetTransferCashResp>(urlStr, dics,null,GetCertHttpClient());
        }
    }
}
