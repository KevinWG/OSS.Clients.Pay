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
using OSS.PayCenter.WX.Cash.Mos;
using OSS.PayCenter.WX.SysTools;

namespace OSS.PayCenter.WX.Cash
{
    public class WxPayRedApi : WxPayBaseApi
    {
        #region  构造函数 ,全局错误处理

        static WxPayRedApi()
        {
            RegisteErrorCode("SENDNUM_LIMIT", "该用户今日领取红包个数超过限制,如有需要、请在微信支付商户平台【api安全】中重新配置 【每日同一用户领取本商户红包不允许超过的个数】。");
            RegisteErrorCode("FREQ_LIMIT", "请对请求做频率控制");
            RegisteErrorCode("OPENID_ERROR", "Openid错误");
            RegisteErrorCode("SECOND_OVER_LIMITED", "企业红包的按分钟发放受限");

            RegisteErrorCode("DAY_ OVER_LIMITED", "企业红包的按天日发放受限");
            RegisteErrorCode("MONEY_LIMIT", "红包金额发放限制");
            RegisteErrorCode("SEND_FAILED", "红包发放失败,请更换单号再重试");
            RegisteErrorCode("PROCESSING", "请求已受理，请稍后使用原单号查询发放结果");
        }

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
            var dics = new SortedDictionary<string, object>();
            dics["nonce_str"] = Guid.NewGuid().ToString().Replace("-", "");

            dics["mch_billno"] = mch_billno;
            dics["bill_type"] = bill_type;

            return await PostApiAsync<WxPayQueryRedResp>(urlStr, dics,null,GetCertHttpClient());
        }
    }
}
