#region Copyright (C) 2021 Kevin (OSS开源实验室) 公众号：osscore

/***************************************************************************
*　　	文件功能描述：微信支付模快 —— 用户扫码支付请求
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2021-7-21
*       
*****************************************************************************/

#endregion

using System.Net.Http;

namespace OSS.Clients.Pay.Wechat.Basic
{
    public class NativePayReq:BaseReq<NativePayReq, NativePayResp>
    {
        public NativePayReq() : base("/v3/pay/transactions/native", HttpMethod.Post)
        {
        }



    }

    public class NativePayResp : BaseResp
    {

    }
}
