
#region Copyright (C) 2017 Kevin (OSS开源作坊) 公众号：osscoder

/***************************************************************************
*　　	文件功能描述：支付宝支付模快 —— 支付相关接口
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2017-3-28
*       
*****************************************************************************/

#endregion

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OSS.Common.ComModels;
using OSS.Http.Mos;
using OSS.PayCenter.ZFB.Pay.Mos;

namespace OSS.PayCenter.ZFB.Pay
{
    public class ZPayTradeApi:ZPayBaseApi
    {
        public ZPayTradeApi(ZPayCenterConfig config=null) : base(config)
        {
        }
        
        /// <summary>
        /// 统一预下单（收单）
        /// </summary>
        /// <param name="payReq"></param>
        public async Task<ZAddPreTradeResp> AddPreTrade(ZAddPreTradeReq payReq)
        {
            const string respColumnName = "alipay_trade_precreate_response";
            const string apiMethod = "alipay.trade.precreate";

            var req = new OsHttpRequest();

            req.HttpMothed = HttpMothed.POST;
            req.CustomBody = ConvertDicToString(GetReqBodyDics(apiMethod, payReq)); 
            
            return await RestCommon<ZAddPreTradeResp>(req, respColumnName);
        }

        /// <summary>
        ///  验证回调接口签名
        /// </summary>
        /// <param name="formDics">表单的字典值</param>
        /// <returns></returns>
        public ResultMo CheckCallBackSign(IDictionary<string, string> formDics)
        {
            var sign = formDics["sign"];
            var signType = formDics["sign_type"];

            formDics.Remove("sign");
            formDics.Remove("sign_type");

            var sortDics=new SortedDictionary<string,string>(formDics);
            var checkContent = string.Join("&", sortDics.Select(d => string.Concat(d.Key, "=", d.Value)));

            var  result=new ResultMo();
            CheckSign(checkContent, sign, result, signType);
            return result;
        }
    }
}
