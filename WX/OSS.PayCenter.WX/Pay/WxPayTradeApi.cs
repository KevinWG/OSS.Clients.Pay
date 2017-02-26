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

using System.Threading.Tasks;
using OSS.Common.ComModels;
using OSS.PayCenter.WX.Pay.Mos;
using OSS.PayCenter.WX.SysTools;

namespace OSS.PayCenter.WX.Pay
{
    public class WxPayTradeApi : WxPayBaseApi
    {
        #region   全局错误码注册

        static WxPayTradeApi()
        {
            RegisteErrorCode("ordernotexist", "此交易订单号不存在");
        }

        #endregion


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="config">配置信息，如果这里为空，请在程序入口处 设置WxPayBaseApi.DefaultConfig的值</param>
        public WxPayTradeApi(WxPayCenterConfig config = null) : base(config)
        {
        }

        #region  下单接口
        /// <summary>
        ///   统一下单接口
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public async Task<WxAddPayUniOrderResp> AddUniOrder(WxAddPayUniOrderReq order)
        {
            var dics = order.GetDics();
            dics["notify_url"] = ApiConfig.NotifyUrl;

            string addressUrl = string.Concat(m_ApiUrl, "/pay/unifiedorder");

            return await PostPaySortDics<WxAddPayUniOrderResp>(addressUrl, dics);
        }



        /// <summary>
        ///   扫码下单接口
        /// 提交支付请求后微信会同步返回支付结果。当返回结果为“系统错误（err_code=SYSTEMERROR）”时，商户系统等待5秒后调用【查询订单API】，查询支付实际交易结果；
        /// 当返回结果为“USERPAYING”时，商户系统可设置间隔时间(建议10秒)重新查询支付结果，直到支付成功或超时(建议30秒)
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public async Task<WxPayOrderTradeResp> AddMicroPayOrder(WxAddMicroPayOrderReq order)
        {
            var dics = order.GetDics();
            dics["notify_url"] = ApiConfig.NotifyUrl;

            string addressUrl = string.Concat(m_ApiUrl, "/pay/micropay");

            return await PostPaySortDics<WxPayOrderTradeResp>(addressUrl, dics);
        }
        #endregion

        /// <summary>
        ///  查询订单接口
        /// </summary>
        /// <param name="queryReq"></param>
        /// <returns></returns>
        public async Task<WxPayQueryOrderResp> QueryOrder(WxPayQueryOrderReq queryReq)
        {
            var dics = queryReq.GetDics();
            var addressUrl = string.Concat(m_ApiUrl, "/pay/orderquery");

            return await PostPaySortDics<WxPayQueryOrderResp>(addressUrl, dics);
        }

        /// <summary>
        ///  关闭订单
        /// </summary>
        /// <param name="closeReq"></param>
        /// <returns></returns>
        public async Task<WxPayBaseResp> CloseOrder(WxPayCloseOrderReq closeReq)
        {
            var dics = closeReq.GetDics();
            var addressUrl = string.Concat(m_ApiUrl, "/pay/closeorder");

            return await PostPaySortDics<WxPayQueryOrderResp>(addressUrl, dics);
        }

        /// <summary>
        ///  订单通知结果解析，并完成验证
        /// </summary>
        /// <returns>如果签名验证不通过，Ret=310</returns>
        public WxPayOrderTradeResp DecryptTradeResult(string contentXmlStr)
        {
            var dics = XmlDicHelper.ChangXmlToDir(contentXmlStr);

            var res = new WxPayOrderTradeResp();

            res.SetResultDirs(dics);
            CheckResultSign(dics, res);

            return res;
        }


        /// <summary>
        ///   接受微信支付通知后需要返回的信息
        /// </summary>
        /// <param name="res"></param>
        /// <returns></returns>
        public string GetTradeSendXml(ResultMo res)
        {
            return
                $"<xml><return_code><![CDATA[{(res.IsSuccess ? "Success" : "FAIL")}]]></return_code><return_msg><![CDATA[{res.Message}]]></return_msg></xml>";
        }
    }
}
