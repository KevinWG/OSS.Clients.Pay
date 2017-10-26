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

using Newtonsoft.Json;

namespace OSS.PaySdk.Ali.Pay.Mos
{
    /// <summary>
    ///  wap页面发起的支付请求实体
    ///  return_url 和 notify_url 需要同时赋值
    /// </summary>
    public class ZAddWapTradeReq : ZAddAppTradeReq
    {
        /// <summary>
        ///   回调通知地址
        /// </summary>
        [JsonIgnore]
        public string return_url
        {
            get => returnUrl;
            set => returnUrl = value;
        }

        /// <summary>   
        ///    String 可空 长度(40)  针对用户授权接口，获取用户相关数据时，用于标识用户授权关系
        /// </summary>  
        public string auth_token { get; set; }
        /// <summary>   
        ///    String 可空 长度(40)  添加该参数后在h5支付收银台会出现返回按钮，
        /// 可用于用户付款中途退出并返回到该参数指定的商户网站地址。
        /// 注：该参数对支付宝钱包标准收银台下的跳转不生效。
        /// </summary>  
        public string quit_url { get; set; }
    }

    /// <summary>
    /// 客户端发起支付请求实体
    /// </summary>
    public class ZAddAppTradeReq : ZAddPayTradeBaseReq
    {
        /// <summary>   
        ///    String 必填 长度(64)  销售产品码
        /// App固定值：QUICK_MSECURITY_PAY
        /// Wap 固定值：QUICK_WAP_WAY
        /// 商家和支付宝签约的产品码
        /// </summary>  
        public string product_code { get; set; } 

        /// <summary>   
        ///    String 可空 长度(2)  商品主类型：0—虚拟类商品，1—实物类商品，注：虚拟类商品不支持使用花呗渠道
        /// </summary>  
        public string goods_type { get; set; }
        
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
        ///    String 可空 长度(512)  公用回传参数【赋值时需要UrlEncode】，如果请求时传递了该参数，则返回给商户时会回传该参数,支付宝会在异步通知时将该参数原样返回。
        /// 本参数必须进行UrlEncode之后才可以发送给支付宝
        /// </summary>  
        public string passback_params { get; set; }


    }

    /// <summary>
    /// 主动唤起支付时的扩展参数
    /// </summary>
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

        /// <summary>   
        ///    String 可选 长度(5)  使用花呗分期要进行的分期数
        /// </summary>  
        public string hb_fq_num { get; set; }

        /// <summary>   
        ///    String 可选 长度(3)  使用花呗分期需要卖家承担的手续费比例的百分值，传入100代表100%
        /// </summary>  
        public string hb_fq_seller_percent { get; set; }
    }
}
