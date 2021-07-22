
using OSS.Clients.Pay.Wechat.Basic;
using System.Threading.Tasks;

namespace OSS.Clients.Pay.Wechat
{
    public interface IWechatCertificateProvider
    {
        Task<WechatCertificateGetResp> GetCertificates(WechatPayConfig payConfig);
    }
}
