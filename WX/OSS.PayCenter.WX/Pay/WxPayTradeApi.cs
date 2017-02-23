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

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
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

        public async Task<WxAddPayTradeOrderResp> AddPayTradeOrder(WxAddPayTradeOrderReq order,
            WxPayTradeDetailMo detail=null)
        {
            var dics = order.GetDics();
       
            if (detail!=null)
                dics.Add("detail",JsonConvert.SerializeObject(detail));

            string addressUrl = string.Concat(m_ApiUrl, "/pay/unifiedorder");
            return await PostPayXml<WxAddPayTradeOrderResp>(addressUrl, dics);
        }
        
    }
}
