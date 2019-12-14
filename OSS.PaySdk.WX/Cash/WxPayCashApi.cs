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
using OSS.PaySdk.Wx.Cash.Mos;
using OSS.PaySdk.Wx.Helpers;
using OSS.Tools.Http.Mos;

namespace OSS.PaySdk.Wx.Cash
{
    public class WxPayCashApi:WxPayBaseApi
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="config">配置信息，如果这里为空，请在程序入口处 设置WxPayBaseApi.DefaultConfig的值</param>
        public WxPayCashApi(WxPayCenterConfig config = null) : base(config)
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
            dics.Add("mchid", ApiConfig.MchId);

            CompleteDicSign(dics);

            var req = new OsHttpRequest
            {
                HttpMethod = HttpMethod.Post,
                AddressUrl = addressUrl,
                CustomBody = dics.ProduceXml()
            };

            return await RestCommonAsync<WxPayTransferCashResp>(req, null, true,false);
        }


        /// <summary>
        /// 获取企业付款信息
        /// </summary>
        /// <param name="partner_trade_no"></param>
        /// <returns></returns>
        public async Task<WxPayGetTransferCashResp> GetTransferCashAsync(string partner_trade_no)
        {
            var urlStr = string.Concat(m_ApiUrl, "/mmpaymkttransfers/gettransferinfo");

            var dics = new SortedDictionary<string, object>
            {
                ["nonce_str"] = WxXmlHelper.GenerateNonceStr(),
                ["partner_trade_no"] = partner_trade_no
            };

            return await PostApiAsync<WxPayGetTransferCashResp>(urlStr, dics,null, true,false);
        }
    }
}
