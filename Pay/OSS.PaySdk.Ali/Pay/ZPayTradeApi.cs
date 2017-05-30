#region Copyright (C) 2017 Kevin (OSS开源作坊) 公众号：osscoder

/***************************************************************************
*　　	文件功能描述：支付宝支付模快 —— 支付接口
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2017-3-28
*       
*****************************************************************************/

#endregion

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OSS.Common.ComModels;
using OSS.Common.Extention;
using OSS.PaySdk.Ali.Pay.Mos;

namespace OSS.PaySdk.Ali.Pay
{
    public class ZPayTradeApi : ZPayBaseApi
    {
        public ZPayTradeApi(ZPayCenterConfig config = null) : base(config)
        {
        }


        #region 发起手动线下收款 （手动扫码

        /// <summary>
        /// 预下单（扫码支付 - 用户扫商家二维码）
        /// </summary>
        /// <param name="payReq"></param>
        public async Task<ZAddPreTradeResp> AddPreTradeAsync(ZAddPreTradeReq payReq)
        {
            const string respColumnName = "alipay_trade_precreate_response";
            const string apiMethod = "alipay.trade.precreate";

            return await PostApiAsync<ZAddPreTradeReq, ZAddPreTradeResp>(apiMethod, respColumnName, payReq);
        }

        /// <summary>
        ///   线下预下单（条码支付- 商家扫用户二维码、读取声波发起支付）
        /// </summary>
        /// <param name="payReq"></param>
        public async Task<ZAddPayTradeResp> AddPayTradeAsync(ZAddPayTradeReq payReq)
        {
            const string respColumnName = "alipay_trade_pay_response";
            const string apiMethod = "alipay.trade.pay";

            return await PostApiAsync<ZAddPayTradeReq, ZAddPayTradeResp>(apiMethod, respColumnName, payReq);
        }

        #endregion


        #region 发起客户端收款（自动唤起

        /// <summary>
        /// 获取客户端App唤起支付请求内容
        /// </summary>
        /// <param name="req"></param>
        public ResultMo<string> GetAppTradeContent(ZAddAppTradeReq req)
        {
            const string apiMethod = "alipay.trade.app.pay";
            var dicsRes = GetReqBodyDics(apiMethod, req);
            if (!dicsRes.IsSuccess())
                return dicsRes.ConvertToResultOnly<string>();
            
            return new ResultMo<string>(ConvertDicToEncodeReqBody(dicsRes.data));
        }

        /// <summary>
        /// 获取客户端Wap唤起支付请求内容
        /// </summary>
        /// <param name="req"></param>
        public ResultMo<string> GetWapTradeContent(ZAddWapTradeReq req)
        {
            const string apiMethod = "alipay.trade.wap.pay";
            var dicsRes = GetReqBodyDics(apiMethod, req);
            if (!dicsRes.IsSuccess())
                return dicsRes.ConvertToResultOnly<string>();

            return new ResultMo<string>(BuildFormHtml(dicsRes.data));
        }

        private  string BuildFormHtml(IDictionary<string, string> dics)
        {
            StringBuilder sbHtml = new StringBuilder();
            sbHtml.Append("<form id='alipaysubmit' name='alipaysubmit' action='" + m_ApiUrl + "?charset=" + ApiConfig.Charset +
                 "' method='POST'>");
            foreach (KeyValuePair<string, string> temp in dics)
            {
                sbHtml.Append($"<input  name='{temp.Key}' value='{temp.Value}'/>");
            }
            //submit按钮控件请不要含有name属性
            sbHtml.Append("<input type='submit' value='submit' style='display:none;'></form>");

            //表单实现自动提交
            sbHtml.Append("<script>document.forms['alipaysubmit'].submit();</script>");
            return sbHtml.ToString();
        }

        #endregion

        #region  订单查询

        /// <summary>
        ///   统一收单线下交易查询
        /// </summary>
        /// <param name="queryReq"></param>
        public async Task<ZQueryTradeResp> QueryTradeAsync(ZQueryTradeReq queryReq)
        {
            const string respColumnName = "alipay_trade_query_response";
            const string apiMethod = "alipay.trade.query";

            return await PostApiAsync<ZQueryTradeReq, ZQueryTradeResp>(apiMethod, respColumnName, queryReq);
        }

        #endregion

        #region  订单取消

        /// <summary>
        ///  撤销交易接口
        /// </summary>
        /// <param name="req"></param>
        public async Task<ZPayCancelResp> CancelTradeAsync(ZPayCancelReq req)
        {
            const string respColumnName = "alipay_trade_cancel_response";
            const string apiMethod = "alipay.trade.cancel";

            return await PostApiAsync<ZPayCancelReq, ZPayCancelResp>(apiMethod, respColumnName, req);
        }

        #endregion



        #region  获取对账单下载地址

        /// <summary>
        ///  获取对账单下载地址
        /// </summary>
        /// <param name="req"></param>
        public async Task<ZGetDownloadUrlResp> GetDownloadUrlAsync(ZGetDownloadUrlReq req)
        {
            const string respColumnName = "alipay_data_dataservice_bill_downloadurl_query_response";
            const string apiMethod = "alipay.data.dataservice.bill.downloadurl.query";

            return await PostApiAsync<ZGetDownloadUrlReq, ZGetDownloadUrlResp>(apiMethod, respColumnName, req);
        }

        #endregion

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

            var sortDics = new SortedDictionary<string, string>(formDics);

            var checkContent = string.Join("&", sortDics.Select(d => string.Concat(d.Key, "=", d.Value.UrlDecode())));

            var result = new ResultMo();
            CheckSign(checkContent, sign, result);
            return result;
        }
    }
}