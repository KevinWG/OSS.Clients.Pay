﻿
namespace OSS.Clients.Pay.Wechat
{
    public class WechatPayConfig
    {
        /// <summary>
        /// 公众号、移动应用、小程序AppId、企业微信CorpId。
        /// </summary>
        public string appp_id { get; set; }

        /// <summary>
        /// 商户号、服务商户号
        /// </summary>
        public string mch_id { get; set; }

        /// <summary>
        /// 目前仅调用服务商API时使用，子商户的公众号、移动应用AppId。
        /// </summary>
        public string sub_app_id { get; set; }

        /// <summary>
        /// 子商户号、服务商户号
        /// </summary>
        public string sub_mch_id { get; set; }


        /// <summary>
        /// 调用API时，需用API密钥生成签名，从而界定你的身份及防止他人篡改数据，需妥善保管防止泄露(API密钥的设置与修改不影响APIv3密钥)
        /// </summary>
        public string api_Key { get; set; }

        /// <summary>
        /// 调用APIv3的下载平台证书接口、处理回调通知中报文时，要通过该密钥来解密信息，防止数据信息泄露(APIv3密钥的设置与修改不影响API密钥)
        /// </summary>
        public string api_v3_Key { get; set; }
        
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
