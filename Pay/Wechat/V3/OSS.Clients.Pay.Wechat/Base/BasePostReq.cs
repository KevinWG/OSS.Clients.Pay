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

using System;
using System.Collections.Generic;
using System.Net.Http;

namespace OSS.Clients.Pay.Wechat
{
    /// <summary>
    ///  Post请求基类
    ///    (非文件上传
    /// </summary>
    public abstract class BasePostReq<TReq, TResp>:InternalBaseReq<TReq, TResp>
        where TReq : BasePostReq<TReq, TResp>
        where TResp : BaseResp
    {
        /// <summary>
        /// 接口请求
        /// </summary>
        protected BasePostReq():base(HttpMethod.Post)
        {
        }

        internal override string GetQueryString()
        {
            return String.Empty;
        }


        #region 业务请求Body参数处理

        /// <summary>
        /// 发送之前准备数据，在这里完成参数处理
        /// </summary>
        /// <returns></returns>
        protected abstract void PrepareBodyPara();

        /// <summary>
        ///  准备通用参数
        ///     如：appid,mchid
        /// </summary>
        protected virtual void PrepareCommonBodyPara()
        {
            if (IsSpPartnerReq)
            {
                AddBodyPara("sp_appid", pay_config.app_id);
                AddBodyPara("sp_mchid", pay_config.mch_id);
                AddBodyPara("sub_appid", sub_app_id);
                AddBodyPara("sub_mchid", sub_mch_id);
            }
            else
            {
                AddBodyPara("appid", pay_config.app_id);
                AddBodyPara("mchid", pay_config.mch_id);
            }
        }


        private Dictionary<string, object> _paraDics;

        /// <summary>
        ///  添加参数
        /// </summary>
        /// <param name="paraName"></param>
        /// <param name="value"></param>
        protected internal void AddBodyPara(string paraName, object value)
        {
            if (_paraDics == null)
                _paraDics = new Dictionary<string, object>();

            if (string.IsNullOrEmpty(paraName) || string.IsNullOrEmpty(value?.ToString()))
                return;

            _paraDics[paraName] = value;
        }


        private Dictionary<string, string> _needEncryptParaDics;

        /// <summary>
        ///  添加敏感需要加密参数
        /// </summary>
        /// <param name="paraName"></param>
        /// <param name="value"></param>
        protected void AddEncryptBodyPara(string paraName, string value)
        {
            if (_paraDics == null)
            {
                _needEncryptParaDics = new Dictionary<string, string>();
            }

            _needEncryptParaDics[paraName] = value;
        }


        /// <summary>
        ///  添加可选参数
        /// </summary>
        /// <param name="optionalPara"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public TReq AddOptionalBodyPara(string optionalPara, object value)
        {
            if (!_paraDics.ContainsKey(optionalPara))
                AddBodyPara(optionalPara, value);

            return (TReq)this;
        }
        #endregion
        
        internal override Dictionary<string, object> GetSendParaDics()
        {
            PrepareBodyPara();
            PrepareCommonBodyPara(); // 防止其他参数覆盖
            return _paraDics;
        }

        internal override Dictionary<string, string> GetSendEncryptParaDics()
        {
            return _needEncryptParaDics;
        }

    }
}
