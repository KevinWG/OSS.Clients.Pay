using OSS.Common.Resp;

namespace OSS.Clients.Pay.Wechat
{
    /// <summary>
    ///  响应基类
    /// </summary>
    public class WechatBaseResp : Resp
    {
        private string? _code;

        /// <summary>
        ///  返回错误码
        /// </summary>
        public new string code
        {
            get => _code;
            set
            {
                _code = value;
                if (!string.IsNullOrEmpty(_code))
                {
                    base.code = (int) RespCodes.OperateFailed;
                }
            }
        }


        /// <summary>
        ///  返回错误码
        /// </summary>
        public string? message
        {
            get => msg;
            set => msg = value;
        }

        /// <summary>
        ///  请求id
        /// </summary>
        public string request_id { get; set; } = string.Empty;

        /// <summary>
        /// 响应内容
        /// </summary>
        public string response_body { get; set; } = string.Empty;
    }

    internal static class RespMap
    {
        public static TResp ToResp<TResp>(this WechatBaseResp res)
            where TResp : WechatBaseResp, new()
        {
            var newRes = new TResp()
            {
                code          = res.code, message = res.message,
                request_id    = res.request_id,
                response_body = res.response_body
            };

            return newRes;
        }
    }
}