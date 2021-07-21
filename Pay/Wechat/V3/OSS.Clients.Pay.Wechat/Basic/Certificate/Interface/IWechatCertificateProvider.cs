
namespace OSS.Clients.Pay.Wechat.Basic.Certificate.Interface
{
    public interface IWechatCertificateProvider
    {
        WechatCertificateGetResp GetCertificates(WechatPayConfig payConfig);
    }
}
