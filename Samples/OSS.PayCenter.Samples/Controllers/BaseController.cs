using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OSS.PayCenter.WX;

namespace OSS.PayCenter.Samples.Controllers
{
    public class BaseController : Controller
    {
        protected static WxPayCenterConfig WxPayConfig { get; set; }

        static BaseController()
        {
            WxPayConfig = new WxPayCenterConfig()
            {
                AppSource = "11",//  用户自定义填写，主要处理
                AppId = "wx9b46f71cd4597f6e",
                MchId = "1233410002",
                Key = "e10adc3849ba56abbe56e056f20f883e",
                AppSecret = "51c56b886b5be869567dd389b3e5d1d6",

                CertPassword = "1233410002",
                CertPath = "cert/apiclient_cert.p12",
                NotifyUrl = "pay.sample.osscoder.com",
                // 设置证书方式
                SetCertificata = (handler, cert) =>
                {
                    handler.ServerCertificateCustomValidationCallback = (msg, c, chain, sslErrors) => true;
                    handler.ClientCertificates.Add(cert);
                }
            };
        }
    }
}