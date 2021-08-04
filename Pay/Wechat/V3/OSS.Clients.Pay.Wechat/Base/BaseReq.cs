#region Copyright (C) 2021 Kevin (OSS开源实验室) 公众号：osscore

/***************************************************************************
*　　	文件功能描述：微信支付模快 —— 请求基类
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2021-7-21
*       
*****************************************************************************/

#endregion

using System.Collections.Generic;
using System.Net.Http;

namespace OSS.Clients.Pay.Wechat
{
    /// <summary>
    ///  请求基类
    /// </summary>
    public abstract class InternalBaseReq<TReq, TResp>
        where TReq : InternalBaseReq<TReq, TResp>
        where TResp : BaseResp
    {
        /// <summary>
        /// 接口请求
        /// </summary>
        /// <param name="method"></param>
        protected InternalBaseReq(HttpMethod method)
        {
            this.method  = method;
        }
        
        /// <summary>
        ///  请求方法
        /// </summary>
        public HttpMethod method { get; }
      

        #region 服务商子商户

        /// <summary>
        ///  是否是服务商请求
        /// </summary>
        public bool IsSpPartnerReq { get; private set; } = false;
        
        protected string sub_app_id { get; private set; }
        protected string sub_mch_id { get; private set; }

        /// <summary>
        /// 如果是服务商
        ///  设置服务下子子商户信息
        /// </summary>
        /// <param name="payConfig"></param>
        /// <returns></returns>
        public TReq UsePantnerWithSubMch(string subAppId, string subMchId)
        {
            IsSpPartnerReq = true;
            sub_app_id     = subAppId;
            sub_mch_id     = subMchId;
            //pay_config = payConfig;
            return (TReq) this;
        }

        #endregion
        
     
        #region 支付配置

        /// <summary>
        ///  支付信息
        /// </summary>
        protected internal WechatPayConfig pay_config { get; set; }

        /// <summary>
        ///  设置当前请求对应的支付配置信息
        ///     仅当前请求下有效，如果配置全局信息，请设置     WechatPayConfigProvider.config  
        /// </summary>
        /// <param name="payConfig"></param>
        /// <returns></returns>
        public TReq SetContextConfig(WechatPayConfig payConfig)
        {
            pay_config = payConfig;
            return (TReq)this;
        }

        #endregion



        /// <summary>
        ///  获取请求接口路径地址
        /// </summary>
        /// <returns></returns>
        public abstract string GetApiPath();

        // body 普通字段字典
        internal abstract Dictionary<string, object> GetSendParaDics();

        // body 敏感字段字段
        internal abstract Dictionary<string, string> GetSendEncryptParaDics();

    }
}
