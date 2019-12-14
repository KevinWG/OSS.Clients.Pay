#region Copyright (C) 2017 Kevin (OSS开源作坊) 公众号：osscore

/***************************************************************************
*　　	文件功能描述：微信支付模快 —— 小额现金模快
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2017-2-23
*       
*****************************************************************************/

#endregion

using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using OSS.Clients.Pay.WX.Cash.Mos;
using OSS.Clients.Pay.WX.Helpers;
using OSS.Tools.Http.Mos;

namespace OSS.Clients.Pay.WX.Cash
{
    public class WXPayCashApi:WXPayBaseApi
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="config">配置信息，如果这里为空，请在程序入口处 设置WXPayBaseApi.DefaultConfig的值</param>
        public WXPayCashApi(WXPayCenterConfig config = null) : base(config)
        {
        }

        /// <summary>
        ///  企业付款接口
        /// </summary>
        /// <param name="cashReq"></param>
        /// <returns></returns>
        public async Task<WXPayTransferCashResp> TransferCashAsync(WXPayTransferCashReq cashReq)
        {
            var addressUrl = string.Concat(m_ApiUrl, "/mmpaymkttransfers/promotion/transfers");
            var dics = cashReq.GetDics();

            dics.Add("mch_appid", ApiConfig.AppId);
            dics.Add("mchid", ApiConfig.MchId);

            CompleteDicSign(dics);

            var req = new OssHttpRequest
            {
                HttpMethod = HttpMethod.Post,
                AddressUrl = addressUrl,
                CustomBody = dics.ProduceXml()
            };

            return await RestCommonAsync<WXPayTransferCashResp>(req, null, true,false);
        }


        /// <summary>
        /// 获取企业付款信息
        /// </summary>
        /// <param name="partner_trade_no"></param>
        /// <returns></returns>
        public async Task<WXPayGetTransferCashResp> GetTransferCashAsync(string partner_trade_no)
        {
            var urlStr = string.Concat(m_ApiUrl, "/mmpaymkttransfers/gettransferinfo");

            var dics = new SortedDictionary<string, object>
            {
                ["nonce_str"] = WXXmlHelper.GenerateNonceStr(),
                ["partner_trade_no"] = partner_trade_no
            };

            return await PostApiAsync<WXPayGetTransferCashResp>(urlStr, dics,null, true,false);
        }
    }
}
