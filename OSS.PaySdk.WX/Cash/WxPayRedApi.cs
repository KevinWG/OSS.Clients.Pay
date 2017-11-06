#region Copyright (C) 2017 Kevin (OSS开源作坊) 公众号：osscoder

/***************************************************************************
*　　	文件功能描述：微信支付模快 —— 红包模块接口
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2017-2-27
*       
*****************************************************************************/

#endregion

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OSS.Http.Mos;
using OSS.PaySdk.Wx.Cash.Mos;
using OSS.PaySdk.Wx.SysTools;

namespace OSS.PaySdk.Wx.Cash
{
    public class WxPayRedApi : WxPayBaseApi
    {
        #region  构造函数 

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="config"></param>
        public WxPayRedApi(WxPayCenterConfig config = null) : base(config)
        {
        }

        #endregion
        
        #region  发送红包接口

        /// <summary>
        ///  发送普通红包
        /// </summary>
        /// <param name="redReq"></param>
        /// <returns></returns>
        public async Task<WxPaySendRedResp> SendRedPackageAsync(WxPaySendRedReq redReq)
        {
            var urlStr = string.Concat(m_ApiUrl, "/mmpaymkttransfers/sendredpack");

            return await SendRedPackageAsync<WxPaySendRedResp>(urlStr, redReq.GetDics());
        }


        /// <summary>
        ///  发送裂变红包
        /// </summary>
        /// <param name="redReq"></param>
        /// <returns></returns>
        public async Task<WxPaySendRedResp> SendGroupRedPackageAsync(WxPaySendGroupRedReq redReq)
        {
            var urlStr = string.Concat(m_ApiUrl, "/mmpaymkttransfers/sendgroupredpack");

            return await SendRedPackageAsync<WxPaySendRedResp>(urlStr, redReq.GetDics());
        }

        /// <summary>
        ///   post 支付接口相关请求
        /// </summary>
        /// <typeparam name="T">返回参数类型</typeparam>
        /// <param name="addressUrl">接口地址</param>
        /// <param name="xmlDirs">请求参数的排序字典（不包括：appid,mch_id,sign。 会自动补充）</param>
        /// <returns></returns>
        private async Task<T> SendRedPackageAsync<T>(string addressUrl, SortedDictionary<string, object> xmlDirs)
            where T : WxPayBaseResp, new()
        {
            xmlDirs.Add("wxappid", ApiConfig.AppId);
            xmlDirs.Add("mch_id", ApiConfig.MchId);

            CompleteDicSign(xmlDirs);

            var req = new OsHttpRequest();
            req.HttpMothed = HttpMothed.POST;
            req.AddressUrl = addressUrl;
            req.CustomBody = xmlDirs.ProduceXml();

            return await RestCommonAsync<T>(req, null, GetCertHttpClient());
        }
        #endregion

        /// <summary>
        ///  查询红包
        /// </summary>
        /// <param name="mch_billno">商户订单号</param>
        /// <param name="bill_type">订单类型</param>
        /// <returns></returns>
        public async Task<WxPayQueryRedResp> QueryRedAsync(string mch_billno, string bill_type = "MCHT")
        {
            var urlStr = string.Concat(m_ApiUrl, "/mmpaymkttransfers/gethbinfo");

            var dics = new SortedDictionary<string, object>
            {
                ["nonce_str"] = SysUtil.GenerateNonceStr(),
                ["mch_billno"] = mch_billno,
                ["bill_type"] = bill_type
            };

            return await PostApiAsync<WxPayQueryRedResp>(urlStr, dics,null,GetCertHttpClient());
        }
    }
}
