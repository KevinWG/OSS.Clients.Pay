
#region Copyright (C) 2017 Kevin (OSS开源作坊) 公众号：osscoder

/***************************************************************************
*　　	文件功能描述：支付宝支付模快 —— 统一收单线下交易预创建相关实体
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2017-3-28
*       
*****************************************************************************/

#endregion


namespace OSS.PayCenter.ZFB.Pay.Mos
{
    /// <summary>
    ///   统一收单线下交易预创建
    /// </summary>
    public class ZAddPreTradeReq : ZAddTradeBaseReq
    {
        public ZAddPreTradeReq(string notifyUrl) : base(notifyUrl)
        {
        }

        /// <summary>   
        ///    String 可选 长度(100)  买家支付宝账号
        /// </summary>  
        public string buyer_logon_id { get; set; }

        /// <summary>   
        ///    GoodsDetail [] 长度(可选)  -
        /// </summary>  
        public ZAddTradeGoodDetailMo goods_detail { get; set; }
    }

    /// <summary>
    /// 预下单响应实体
    /// </summary>
    public class ZAddPreTradeResp:ZPayBaseResp
    {
        /// <summary>   
        ///    String 必填 长度(64)  商户的订单号
        /// </summary>  
        public string out_trade_no { get; set; }

        /// <summary>   
        ///    String 必填 长度(1024)  当前预下单请求生成的二维码码串，可以用二维码生成工具根据该码串值生成对应的二维码
        /// </summary>  
        public string qr_code { get; set; }
    }



    /// <summary>
    ///  统一收单中的商品详情实体
    /// </summary>
    public class ZAddTradeGoodDetailMo
    {
        /// <summary>   
        ///    String 必填 长度(32)  商品的编号
        /// </summary>  
        public string goods_id { get; set; }

        /// <summary>   
        ///    String 可选 长度(32)  支付宝定义的统一商品编号
        /// </summary>  
        public string alipay_goods_id { get; set; }

        /// <summary>   
        ///    String 必填 长度(256)  商品名称
        /// </summary>  
        public string goods_name { get; set; }

        /// <summary>   
        ///    Number 必填 长度(10)  商品数量
        /// </summary>  
        public int quantity { get; set; }

        /// <summary>   
        ///    Price 必填 长度(9)  商品单价，单位为元
        /// </summary>  
        public decimal price { get; set; }

        /// <summary>   
        ///    String 可选 长度(24)  商品类目
        /// </summary>  
        public string goods_category { get; set; }

        /// <summary>   
        ///    String 可选 长度(1000)  商品描述信息
        /// </summary>  
        public string body { get; set; }

        /// <summary>   
        ///    String 可选 长度(400)  商品的展示地址
        /// </summary>  
        public string show_url { get; set; }
    }
}
