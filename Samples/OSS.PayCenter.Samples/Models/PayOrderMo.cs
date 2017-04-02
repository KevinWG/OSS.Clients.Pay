using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OSS.PayCenter.Samples.Models
{
    public class PayOrderMo
    {
        public string openid { get; set; }

        public string order_name { get; set; }
        public decimal order_price { get; set; }
        public int pay_channel { get; set; }
    }
}
