using System.Net;

namespace OSS.Clients.Pay.Wechat
{
    public readonly struct HttpResponseDetail
    {
        public HttpResponseDetail(string reqId, HttpStatusCode statusCode,
                                  bool isSuccessStatusCode,
            string serial_num, string body,
            string signature,
            string nonce, long timestamp)
        {
            this.status_code = statusCode;
            this.signature   = signature;
            
            this.body       = body;
            this.nonce      = nonce;
            this.serial_no = serial_num;

            this.timestamp  = timestamp;
            this.request_id = reqId;

            IsSuccessStatusCode = isSuccessStatusCode;
        }

        public string request_id { get; }

        public string body { get; }
        
        public bool           IsSuccessStatusCode { get; }
        public HttpStatusCode status_code         { get; }

        /// <summary>
        ///  签名
        /// </summary>
        public string signature { get; }

        /// <summary>
        ///  证书编号
        /// </summary>
        public string serial_no { get; }

        public string nonce { get; }

        public long timestamp { get; }
    }
}