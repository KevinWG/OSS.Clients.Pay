
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

using System.Collections.Generic;

namespace OSS.PaySdk.Ali.Pay.Mos
{

    /// <summary>
    ///  预下单，用户扫码付款
    /// </summary>
    public class ZAddPreTradeReq : ZAddPayTradeBaseReq
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="notifyUrl">通知回调地址</param>
        public ZAddPreTradeReq(string notifyUrl)
        {
            notify_url = notifyUrl;
        }

        /// <summary>   
        ///    Price 可选 长度(11)  参与优惠计算的金额，单位为元，精确到小数点后两位，取值范围[0.01,100000000]。 8.88
        /// </summary>  
        public decimal discountable_amount { get; set; }

        /// <summary>   
        ///    String 可选 长度(28)  商户操作员编号
        /// </summary>  
        public string operator_id { get; set; }

        /// <summary>   
        ///    String 可选 长度(32)  商户机具终端编号
        /// </summary>  
        public string terminal_id { get; set; }

        /// <summary>   
        ///    订单包含的商品列表信息
        /// </summary>  
        public List<ZPayTradeGoodDetailMo> goods_detail { get; set; }

        /// <summary>   
        ///    ExtendParams 可选 长度(-)  业务扩展参数
        /// </summary>  
        public ZTradeExtendParaMo extend_params { get; set; }
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
    ///  统一收单业务扩展参数
    /// </summary>
    public class ZTradeExtendParaMo
    {
        /// <summary>   
        ///    String 可选 长度(64)  系统商编号 2088511833207846
        /// </summary>  
        public string sys_service_provider_id { get; set; }
    }


    #region  支付下单基类
    /// <summary>
    ///  统一收单的基类
    /// </summary>
    public class ZAddPayTradeBaseReq : ZPayBaseReq
    {
        /// <summary>   
        ///    String 必填 长度(64)  商户网站唯一订单号
        /// </summary>  
        public string out_trade_no { get; set; }

        /// <summary>   
        ///    String 可空 长度(128)  对一笔交易的具体描述信息。如果是多种商品，请将商品描述字符串累加传给body。 16G
        /// </summary>  
        public string body { get; set; }

        /// <summary>   
        ///    String 必填 长度(256)  商品的标题/交易标题/订单标题/订单关键字等。
        /// </summary>  
        public string subject { get; set; }

        /// <summary>   
        ///    String 可选 长度(28)  如果该值为空，则默认为商户签约账号对应的支付宝用户ID
        /// </summary>  
        public string seller_id { get; set; }

        /// <summary>   
        ///    Price 必须 长度(11)  订单总金额，单位为元，精确到小数点后两位，取值范围[0.01,100000000]。 如果同时传入了【可打折金额】，【不可打折金额】，【订单总金额】三者，则必须满足如下条件：【订单总金额】=【可打折金额】+【不可打折金额】 88.88
        /// </summary>  
        public decimal total_amount { get; set; }

        /// <summary>   
        ///    String 可选 长度(32)  商户门店编号
        /// </summary>  
        public string store_id { get; set; }

        /// <summary>   
        ///    String 可空 长度(6)  设置未付款支付宝交易的超时时间，一旦超时，该笔交易就会自动被关闭。
        ///     当用户进入支付宝收银台页面（不包括登录页面），会触发即刻创建支付宝交易，此时开始计时。
        ///     取值范围：1m～15d。m-分钟，h-小时，d-天，1c-当天（1c-当天的情况下，无论交易何时创建，都在0点关闭）。
        ///  如 1.5h，可转换为 90m。 90m
        /// </summary>  
        public string timeout_express { get; set; }

    }
    
    /// <summary>
    ///  统一收单中的商品详情实体
    /// </summary>
    public class ZPayTradeGoodDetailMo
    {
        /// <summary>   
        ///    String 必填 长度(32)  商品的编号
        /// </summary>  
        public string goods_id { get; set; }

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
    #endregion
}
