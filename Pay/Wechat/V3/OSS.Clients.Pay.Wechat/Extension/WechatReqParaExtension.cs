using System.Collections.Generic;

namespace OSS.Clients.Pay.Wechat
{
    /// <summary>
    ///  请求参数扩展
    /// </summary>
    public static class WechatReqParaExtension
    {
        /// <summary>
        ///  设置当前请求对应的支付配置信息
        ///     仅当前请求下有效，如果配置全局信息，请设置     WechatPayConfigProvider.config  
        /// </summary>
        /// <param name="req"></param>
        /// <param name="payConfig"></param>
        /// <returns></returns>
        public static TReq SetContextConfig<TReq>(this TReq req, WechatPayConfig payConfig)
            where TReq : WechatBaseReq
        {
            req.pay_config = payConfig;
            return req;
        }

        /// <summary>
        /// 如果是服务商
        ///  设置服务下子子商户信息
        /// </summary>
        /// <param name="req"></param>
        /// <param name="subAppId"></param>
        /// <param name="subMchId"></param>
        /// <returns></returns>
        public static TReq UsePartnerWithSubMch<TReq>(this TReq req, string subAppId, string subMchId)
            where TReq : WechatBaseReq
        {
            req.IsSpPartnerReq = true;

            req.sub_app_id = subAppId;
            req.sub_mch_id = subMchId;

            return req;
        }


        /// <summary>
        ///  添加参数
        /// </summary>
        /// <param name="req"></param>
        /// <param name="paraName"></param>
        /// <param name="value"></param>
        public static TReq AddBodyPara<TReq>(this TReq req, string paraName, object value)
            where TReq: WechatBaseReq
        {
            if (req.ParaDics == null)
                req.ParaDics = new Dictionary<string, object>();

            if (string.IsNullOrEmpty(paraName) || string.IsNullOrEmpty(value?.ToString()))
                return req;

            req.ParaDics[paraName] = value;
            return req;
        }

        /// <summary>
        ///  添加敏感需要加密参数
        /// </summary>
        /// <param name="req"></param>
        /// <param name="paraName"></param>
        /// <param name="value"></param>
        public static TReq AddEncryptBodyPara<TReq>(this TReq req, string paraName, string value)
            where TReq : WechatBaseReq
        {
            if (req.EncryptParaDics == null)
            {
                req.EncryptParaDics = new Dictionary<string, string>();
            }

            if (string.IsNullOrEmpty(paraName) || string.IsNullOrEmpty(value?.ToString()))
                return req;

            req.EncryptParaDics[paraName] = value;
            return req;
        }
    }
}
