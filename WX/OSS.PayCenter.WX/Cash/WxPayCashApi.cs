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
        public async Task<WxPayQCashResp> TransferCash(WxPayQCashReq cashReq)
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

            return await RestCommon<WxPayQCashResp>(req, null, GetCertHttpClient());
        }
    }
}
