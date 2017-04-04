#region Copyright (C) 2017 Kevin (OSS开源作坊) 公众号：osscoder

/***************************************************************************
*　　	文件功能描述：支付宝支付模快 —— 添加客户端发起支付请求接口实体
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2017-4-4
*       
*****************************************************************************/

#endregion

namespace OSS.PayCenter.ZFB.Pay.Mos
{
    /// <summary>
    ///  wap页面发起的支付请求实体
    /// </summary>
    public class ZAddWapTradeReq : ZAddAppTradeReq
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="returnUrl">支付后的跳转地址</param>
        /// <param name="notifyUrl">支付后的异步通知地址</param>
        public ZAddWapTradeReq(string returnUrl,string notifyUrl) : base(notifyUrl)
        {
            return_url = returnUrl;
        }

        /// <summary>   
        ///    String 可空 长度(40)  针对用户授权接口，获取用户相关数据时，用于标识用户授权关系
        /// </summary>  
        public string auth_token { get; set; }
    }

    /// <summary>
    /// 客户端发起支付请求实体
    /// </summary>
    public class ZAddAppTradeReq : ZPayBaseReq
    {
        public ZAddAppTradeReq(string notifyUrl)
        {
            notify_url = notifyUrl;
        }

        /// <summary>   
        ///    String 可空 长度(128)  对一笔交易的具体描述信息。如果是多种商品，请将商品描述字符串累加传给body。 16G
        /// </summary>  
        public string body { get; set; }

        /// <summary>   
        ///    String 必填 长度(256)  商品的标题/交易标题/订单标题/订单关键字等。
        /// </summary>  
        public string subject { get; set; }

        /// <summary>   
        ///    String 必填 长度(64)  商户网站唯一订单号
        /// </summary>  
        public string out_trade_no { get; set; }

        /// <summary>   
        ///    String 可空 长度(6)  设置未付款支付宝交易的超时时间，一旦超时，该笔交易就会自动被关闭。当用户进入支付宝收银台页面（不包括登录页面），会触发即刻创建支付宝交易，此时开始计时。取值范围：1m～15d。m-分钟，h-小时，d-天，1c-当天（1c-当天的情况下，无论交易何时创建，都在0点关闭）。 如 1.5h，可转换为 90m。 90m
        /// </summary>  
        public string timeout_express { get; set; }

        /// <summary>   
        ///    String 必填 长度(9)  订单总金额，单位为元，精确到小数点后两位，取值范围[0.01,100000000]
        /// </summary>  
        public decimal total_amount { get; set; }

        /// <summary>   
        ///    String 可空 长度(16)  收款支付宝用户ID。 2088102147948060
        /// </summary>  
        public string seller_id { get; set; }

        /// <summary>   
        ///    String 必填 长度(64)  销售产品码，商家和支付宝签约的产品码，为固定值QUICK_MSECURITY_PAY
        /// </summary>  
        public string product_code { get; set; }

        /// <summary>   
        ///    String 可空 长度(2)  商品主类型：0—虚拟类商品，1—实物类商品，注：虚拟类商品不支持使用花呗渠道
        /// </summary>  
        public string goods_type { get; set; }

        /// <summary>   
        ///    String 可空 长度(512)  公用回传参数【赋值时需要UrlEncode】，如果请求时传递了该参数，则返回给商户时会回传该参数,支付宝会在异步通知时将该参数原样返回。
        /// 本参数必须进行UrlEncode之后才可以发送给支付宝
        /// </summary>  
        public string passback_params { get; set; }

        /// <summary>   
        ///    String 可空 长度(512)  优惠参数，注：仅与支付宝协商后可用
        /// </summary>  
        public string promo_params { get; set; }

        /// <summary>   
        ///    String 可空 长度()  {"sys_service_provider_id":"2088511833207846"}
        /// </summary>  
        public ZClientTradeExtendParaMo extend_params { get; set; }

        /// <summary>   
        ///    String 可空 长度(128)  可用渠道，用户只能在指定渠道范围内支付，当有多个渠道时用“,”分隔，注：与disable_pay_channels互斥
        /// </summary>  
        public string enable_pay_channels { get; set; }

        /// <summary>   
        ///    String 可空 长度(128)  禁用渠道，用户不可用指定渠道支付，当有多个渠道时用“,”分隔，注：与enable_pay_channels互斥
        ///   渠道说明详见：https://doc.open.alipay.com/doc2/detail.htm?treeId=203&articleId=105463&docType=1
        /// </summary>  
        public string disable_pay_channels { get; set; }

        /// <summary>   
        ///    String 可空 长度(32)  商户门店编号
        /// </summary>  
        public string store_id { get; set; }
    }


    public class ZClientTradeExtendParaMo
    {
        /// <summary>   
        ///    String 可空 长度(64)  系统商编号，该参数作为系统商返佣数据提取的依据，请填写系统商签约协议的PID
        /// </summary>  
        public string sys_service_provider_id { get; set; }

        /// <summary>   
        ///    String 可空 长度(1)  是否发起实名校验，T：发起，F：不发起
        /// </summary>  
        public string needBuyerRealnamed { get; set; }

        /// <summary>   
        ///    String 可空 长度(128)  账务备注，注：该字段显示在离线账单的账务备注中
        /// </summary>  
        public string TRANS_MEMO { get; set; }
    }
}
