#region Copyright (C) 2017 Kevin (OSS开源作坊) 公众号：osscore

/***************************************************************************
*　　	文件功能描述：支付宝支付模快 —— 网站端支付
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2017-9-26
*       
*****************************************************************************/

#endregion

using System.Collections.Generic;
using Newtonsoft.Json;

namespace OSS.Clients.Pay.Ali.Pay.Mos
{
    /// <summary>
    ///  PC网站跳转支付宝收银台的支付请求实体
    ///  return_url 和 notify_url 需要同时赋值
    /// </summary>
    public class ZAddPageTradeReq : ZAddPayTradeBaseReq
    {
        /// <summary>
        /// 构造函数
        ///  return_url 和 notify_url 需要同时赋值
        /// </summary>
        public ZAddPageTradeReq() 
        {
            product_code = "FAST_INSTANT_TRADE_PAY";
            qr_pay_mode = 2;
        }

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
        ///    String 必填 长度(64)
        ///   销售产品码，固定值：FAST_INSTANT_TRADE_PAY 
        /// </summary>  
        public string product_code { get; set; }

        /// <summary>   
        ///    String 可选 长度(64) 绝对超时时间，格式为yyyy-MM-dd HH:mm。 注：1）以支付宝系统时间为准；2）如果和timeout_express参数同时传入，以time_expire为准
        /// </summary>  
        public List<ZPayTradeGoodDetailMo> goods_detail { get; set; }
        
        /// <summary>   
        ///    String 可选 长度(64)
        /// </summary>  
        public ZClientTradeExtendParaMo extend_params { get; set; }

        /// <summary>   
        ///    int 可选 长度(64) 商品主类型：0—虚拟类商品，1—实物类商品
        /// 注：虚拟类商品不支持使用花呗渠道
        /// </summary>  
        public int goods_type { get; set; }


        /// <summary>   
        ///    String 可选 长度(64)可用渠道，用户只能在指定渠道范围内支付 当有多个渠道时用“,”分隔
        /// pcredit,moneyFund,debitCardExpress
        /// </summary>  
        public string enable_pay_channels { get; set; }

        /// <summary>   
        ///    String 可选 长度(64)可用渠道，用户不可用指定渠道支付 当有多个渠道时用“,”分隔
        /// pcredit,moneyFund,debitCardExpress
        /// </summary>  
        public string disable_pay_channels { get; set; }

        /// <summary>   
        ///    String 可选 长度(64)针对用户授权接口，获取用户相关数据时，用于标识用户授权关系 
        /// 注：若不属于支付宝业务经理提供签约服务的商户，暂不对外提供该功能，该参数使用无效
        /// </summary>  
        public string auth_token { get; set; }

        /// <summary>   
        ///   默认跳转方式  可选 长度(64) PC扫码支付的方式，支持前置模式和跳转模式。
        /// 前置模式是将二维码前置到商户的订单确认页的模式。需要商户在自己的页面中以iframe方式请求支付宝页面。具体分为以下几种：
        /// 0：订单码-简约前置模式，对应iframe宽度不能小于600px，高度不能小于300px；
        /// 1：订单码-前置模式，对应iframe宽度不能小于300px，高度不能小于600px；
        /// 3：订单码-迷你前置模式，对应iframe宽度不能小于75px，高度不能小于75px；
        /// 4：订单码-可定义宽度的嵌入式二维码，商户可根据需要设定二维码的大小。
        /// 
        /// 跳转模式下，用户的扫码界面是由支付宝生成的，不在商户的域名下。
        /// 2：订单码-跳转模式
        /// </summary>  
        public int qr_pay_mode { get; set; }

        /// <summary>   
        ///    String 可选 长度(64)商户自定义二维码宽度
        /// 注：qr_pay_mode=4时该参数生效
        /// </summary>  
        public string qrcode_width { get; set; }
        
        /// <summary>   
        ///    String 可空 长度(512)  公用回传参数【赋值时需要UrlEncode】，如果请求时传递了该参数，则返回给商户时会回传该参数,支付宝会在异步通知时将该参数原样返回。
        /// 本参数必须进行UrlEncode之后才可以发送给支付宝
        /// </summary>  
        public string passback_params { get; set; }
    }


}
