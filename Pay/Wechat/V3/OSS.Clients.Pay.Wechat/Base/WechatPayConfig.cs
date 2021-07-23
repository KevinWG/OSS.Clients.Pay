#region Copyright (C) 2021 Kevin (OSS开源实验室) 公众号：osscore

/***************************************************************************
*　　	文件功能描述：微信支付模快 —— 支付配置信息
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2021-7-21
*       
*****************************************************************************/

#endregion

namespace OSS.Clients.Pay.Wechat
{
    public class WechatPayConfig
    {
        /// <summary>
        ///  应用（公众号，小程序，App）AppId。
        ///  如果是服务商，则为服务商 （公众号）appid
        /// </summary>
        public string app_id { get; set; } 

        /// <summary>
        /// 商户号id
        ///  如果为服务商，则为服务商商户id
        /// </summary>
        public string mch_id { get; set; }
        
        /// <summary>
        /// 签名秘钥
        ///     调用API时，需用API密钥生成签名，从而界定你的身份及防止他人篡改数据，需妥善保管防止泄露(API密钥的设置与修改不影响APIv3密钥)
        /// </summary>
        public string api_key { get; set; }

        /// <summary>
        /// 证书解密秘钥
        ///     调用APIv3的下载平台证书接口、处理回调通知中报文时，要通过该密钥来解密信息，防止数据信息泄露(APIv3密钥的设置与修改不影响API密钥)
        /// </summary>
        public string api_v3_key { get; set; }
        
        /// <summary>
        /// 证书路径,  请填写绝对路径，为了安全，请不要将证书放在网站目录下
        /// </summary>
        public string cert_path { get; set; }

        /// <summary>
        ///  证书密码
        /// </summary>
        public string cert_password { get; set; }
    }
}
