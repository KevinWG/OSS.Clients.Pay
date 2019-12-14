#region Copyright (C) 2017 Kevin (OSS开源作坊) 公众号：osscore

/***************************************************************************
*　　	文件功能描述：微信支付模快 —— 支付交易模快
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2017-2-23
*       
*****************************************************************************/

#endregion

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using OSS.Clients.Pay.WX.Helpers;
using OSS.Clients.Pay.WX.Pay.Mos;
using OSS.Common.Extention;
using OSS.Common.Resp;
using OSS.Tools.Http.Extention;
using OSS.Tools.Http.Mos;

namespace OSS.Clients.Pay.WX.Pay
{
    /// <summary>
    ///  发起支付相关API
    /// </summary>
    public class WXPayTradeApi : WXPayBaseApi
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="config">配置信息，如果这里为空，请在程序入口处 设置WXPayBaseApi.DefaultConfig的值</param>
        public WXPayTradeApi(WXPayCenterConfig config = null) : base(config)
        {
        }

        #region  下单接口


        /// <summary>
        ///   统一下单接口
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public async Task<WXAddPayOrderResp> AddUniOrderAsync(WXAddPayUniOrderReq order)
        {
            if (string.IsNullOrEmpty(order.notify_url))
                order.notify_url = ApiConfig.NotifyUrl;
        
            var dics = order.GetDics();
            var addressUrl = string.Concat(m_ApiUrl, "/pay/unifiedorder");

            return await PostApiAsync<WXAddPayOrderResp>(addressUrl, dics);
        }


        /// <summary>
        ///   刷卡下单接口
        /// 提交支付请求后微信会同步返回支付结果。当返回结果为“系统错误（err_code=SYSTEMERROR）”时，商户系统等待5秒后调用【查询订单API】，查询支付实际交易结果；
        /// 当返回结果为“USERPAYING”时，商户系统可设置间隔时间(建议10秒)重新查询支付结果，直到支付成功或超时(建议30秒)
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public async Task<WXAddMicroPayOrderResp> AddMicroPayOrderAsync(WXAddMicroPayOrderReq order)
        {
            var dics = order.GetDics();
            var addressUrl = string.Concat(m_ApiUrl, "/pay/micropay");

            return await PostApiAsync<WXAddMicroPayOrderResp>(addressUrl, dics);
        }

        /// <summary>
        ///  获取js和小程序唤起客户端参数
        /// </summary>
        /// <param name="prepayId">预支付交易Id</param>
        /// <returns></returns>
        public WXGetJsClientParaResp GetJsClientParaResp(string prepayId)
        {
            var jsPara = new WXGetJsClientParaResp
            {
                app_id = ApiConfig.AppId,
                time_stamp = DateTime.Now.ToLocalSeconds().ToString(),
                nonce = WXXmlHelper.GenerateNonceStr(),
                package = string.Concat("prepay_id=", prepayId)
            };

            var dics = new SortedDictionary<string, object>
            {
                ["appId"] = jsPara.app_id,
                ["timeStamp"] = jsPara.time_stamp,
                ["nonceStr"] = jsPara.nonce,
                ["package"] = jsPara.package,
                ["signType"] = jsPara.sign_type
            };
            
            jsPara.sign = GetSign(GetSignContent(dics));

            return jsPara;
        }

        /// <summary>
        /// 获取app唤起客户端参数
        /// </summary>
        /// <param name="prepayId">预支付交易Id</param>
        /// <returns></returns>
        public WXGetAppClientParaResp GetAppClientParaResp(string prepayId)
        {
            var appPara = new WXGetAppClientParaResp
            {
                app_id = ApiConfig.AppId,
                mch_id = ApiConfig.MchId,
                time_stamp = DateTime.Now.ToLocalSeconds().ToString(),
                nonce = WXXmlHelper.GenerateNonceStr(),
                prepay_id = prepayId,

                package = "Sign=WXPay"
            };

            var dics = new SortedDictionary<string, object>
            {
                ["appid"] = appPara.app_id,
                ["partnerid"] = appPara.mch_id,
                ["timestamp"] = appPara.time_stamp,
                ["noncestr"] = appPara.nonce,
                ["package"] = appPara.package,

                ["prepayid"] = appPara.prepay_id
            };
            appPara.sign = GetSign(GetSignContent(dics));

            return appPara;
        }

    

        #endregion

        /// <summary>
        ///  查询订单接口
        /// </summary>
        /// <param name="transaction_id">微信订单号 二选一 String(32) 微信的订单号，建议优先使用</param>
        /// <param name="out_trade_no"> 商户订单号 String(32)</param>
        /// <returns></returns>
        public async Task<WXPayQueryOrderResp> QueryOrderAsync(string transaction_id, string out_trade_no)
        {
            var addressUrl = string.Concat(m_ApiUrl, "/pay/orderquery");

            var dics = new SortedDictionary<string, object>
            {
                ["nonce_str"] = Guid.NewGuid().ToString().Replace("-", ""),
                ["out_trade_no"] = out_trade_no,
                ["transaction_id"] = transaction_id
            };

            return await PostApiAsync<WXPayQueryOrderResp>(addressUrl, dics);
        }
        

        #region  订单结果通知解析 和 生成返回结果xml方法

        /// <summary>
        ///  订单通知结果解析，并完成验证
        /// </summary>
        /// <param name="contentXmlStr">通知结果内容</param>
        /// <returns>如果签名验证不通过，Ret=310</returns>
        public WXPayOrderTradeResp DecryptPayResult(string contentXmlStr)
        {
            return GetRespResult<WXPayOrderTradeResp>(contentXmlStr);
        }
        
        #endregion

        #region  辅助部分方法

        /// <summary>
        ///  转换短链api
        /// </summary>
        /// <param name="long_url"></param>
        /// <returns></returns>
        public async Task<WXPayGetShortUrlResp> GetShortUrlAsync(string long_url)
        {
            var url = string.Concat(m_ApiUrl, "/tools/shorturl");

            var dics = new SortedDictionary<string, object>
            {
                ["nonce_str"] = WXXmlHelper.GenerateNonceStr(),
                ["long_url"] = long_url
            };

            return await PostApiAsync<WXPayGetShortUrlResp>(url, dics, null, false,true,
                d => d["long_url"] = d["long_url"].UrlEncode());
        }

        /// <summary>
        ///  授权码查询OPENID接口
        /// </summary>
        /// <param name="auth_code"></param>
        /// <returns></returns>
        public async Task<WXPayAuthCodeOpenIdResp> GetAuthCodeOpenIdAsync(string auth_code)
        {
            var url = string.Concat(m_ApiUrl, "/tools/authcodetoopenid");

            var dics = new SortedDictionary<string, object>
            {
                ["nonce_str"] = WXXmlHelper.GenerateNonceStr(),
                ["auth_code"] = auth_code
            };

            return await PostApiAsync<WXPayAuthCodeOpenIdResp>(url, dics);
        }

        #endregion

        #region   扫码支付模式一

        /// <summary>
        /// 生成二维码地址(扫码支付模式一)
        /// </summary>
        /// <param name="product_id"></param>
        /// <returns></returns>
        public string CreateScanCode(string product_id)
        {
            var dics = new SortedDictionary<string, object>
            {
                ["time_stamp"] = DateTime.Now.ToLocalSeconds().ToString(),
                ["nonce_str"] = WXXmlHelper.GenerateNonceStr(),
                ["product_id"] = product_id,
                ["appid"] = ApiConfig.AppId,
                ["mch_id"] = ApiConfig.MchId
            };
            var encStr = GetSignContent(dics);
            var sign = GetSign(encStr);

            return string.Concat("weixin://wxpay/bizpayurl?", encStr, "&sign=", sign);
        }

        /// <summary>
        /// 解析微信扫码回调消息实体（模式一）
        /// </summary>
        /// <param name="contentStr">消息实体</param>
        /// <returns></returns>
        public WXPayScanCallBackMo DecryptScanCallBackMsg(string contentStr)
        {
            return GetRespResult<WXPayScanCallBackMo>(contentStr);
        }

        /// <summary>
        ///  把统一下单结果响应给微信支付系统
        /// </summary>
        /// <param name="uniOrder"></param>
        /// <returns></returns>
        public string GetScanCallBackResponse(WXAddPayOrderResp uniOrder)
        {
            var res = new WXPayScanCallBackResMo
            {
                err_code_des = uniOrder.err_code_des,
                prepay_id = uniOrder.prepay_id,
                result_code = uniOrder.result_code,
                return_code = uniOrder.return_code,
                return_msg = uniOrder.return_msg
            };

            var dics = res.GetDics();
            dics.Add("appid", ApiConfig.AppId);
            dics.Add("mch_id", ApiConfig.MchId);

            CompleteDicSign(dics);
            return dics.ProduceXml();
        }


        #endregion
        
        #region  下载对账单

        /// <summary>
        /// 下载对账单
        /// </summary>
        /// <param name="billReq"></param>
        /// <returns></returns>
        public async Task<Resp<string>> DownloadBillAsync(WXPayDownloadBillReq billReq)
        {
            var dics = billReq.GetDics();

            dics.Add("appid", ApiConfig.AppId);
            dics.Add("mch_id", ApiConfig.MchId);
            CompleteDicSign(dics);

            var req = new OssHttpRequest
            {
                HttpMethod = HttpMethod.Post,
                AddressUrl = string.Concat(m_ApiUrl, "/pay/downloadbill"),
                CustomBody = dics.ProduceXml()
            };

            var response = await req.RestSend();
            if (!response.IsSuccessStatusCode) return new Resp<string>() {ret = -1, msg = "当前请求出错！"};

            var content = await response.Content.ReadAsStringAsync();
            return content.StartsWith("<xml>") ? new Resp<string>(content) 
                : new Resp<string>().WithResp(RespTypes.ObjectStateError, content);
        }

        #endregion

    }
}
