#region Copyright (C) 2017 Kevin (OSS开源作坊) 公众号：osscoder

/***************************************************************************
*　　	文件功能描述：微信支付模快 —— 统一下单实体
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2017-2-23
*       
*****************************************************************************/

#endregion

using OSS.Common.ComModels;

namespace OSS.PaySdk.Wx.Pay.Mos
{
    #region   统一下单接口

    /// <summary>
    ///  统一下单请求实体
    /// </summary>
    public class WxAddPayUniOrderReq  : WxAddSmallAppOrderReq
    {
        /// <summary>
        /// H5场景值 【必填】
        ///   【IOS 或 Android】 【不建议】 {"h5_info": {"type":"IOS/Android","app_name": "王者荣耀","bundle_id": "com.tencent.wzryIOS"}}
        ///   【 WAP站点 】{"h5_info": {"type":"Wap","wap_url": "https://m.jd.com","wap_name": "京东官网"}}
        /// 
        /// 公众号和扫码支付 场景值 【选填】
        ///    {"store_info":{"id": "门店ID","name": "名称","area_code": "编码","address": "地址" }} 
        /// 
        /// App 场景值 【选填】 
        ///    { "store_id": "门店唯一标识 选填", "store_name":"门店名称 选填”}
        /// </summary>
        public string scene_info { get; set; }

        /// <inheritdoc />
        protected override void SetSignDics()
        {
            base.SetSignDics();
            SetDicItem("scene_info", scene_info);
        }
    }


    /// <summary>
    ///   小程序的统一下单
    /// </summary>
    public class WxAddSmallAppOrderReq : WxAddPayOrderBaseReq
    {
        /// <summary>   
        ///    交易起始时间 可空 String(14) 订单生成时间，格式为yyyyMMddHHmmss，如2009年12月25日9点10分10秒表示为20091225091010。其他详见时间规则===
        /// </summary>  
        public string time_start { get; set; }

        /// <summary>   
        ///    交易结束时间 可空 String(14) 订单失效时间，格式为yyyyMMddHHmmss，如2009年12月27日9点10分10秒表示为20091227091010。其他详见时间规则 注意：最短失效时间间隔必须大于5分钟===
        /// </summary>  
        public string time_expire { get; set; }

        /// <summary>   
        ///  交易类型 必填 String(16) 取值如下：JSAPI，NATIVE，APP, MWEB
        ///    H5支付的交易类型为 MWEB
        ///    小程序取值如：JSAPI
        /// </summary>  
        public string trade_type { get; set; }

        /// <summary>   
        ///    商品ID 可空 String(32) trade_type=NATIVE时（即扫码支付），此参数必传。
        ///    此参数为二维码中包含的商品ID，商户自行定义。
        /// </summary>  
        public string product_id { get; set; }

        /// <summary>   
        ///    用户标识 可空 String(128) trade_type=JSAPI时（即公众号支付），此参数必传，此参数为微信用户在商户对应appid下的唯一标识。openid如何获取，可参考【获取openid】。企业号请使用【企业号OAuth2.0接口】获取企业号内成员userid，再调用【企业号userid转openid接口】进行转换
        /// </summary>
        public string openid { get; set; }


        /// <inheritdoc />
        protected override void SetSignDics()
        {
            base.SetSignDics();
            SetDicItem("time_start", time_start);
            SetDicItem("time_expire", time_expire);
            SetDicItem("trade_type", trade_type);
            SetDicItem("product_id", product_id);
            SetDicItem("openid", openid);
        }
        
    }


    /// <summary>
    /// 统一支付下单响应
    /// </summary>
    public class WxAddPayOrderResp : WxAddPayOrderBaseResp
    {
        /// <summary>   
        ///    预支付交易会话标识 必填 String(64) 微信生成的预支付回话标识，用于后续接口调用中使用，该值有效期为2小时
        /// </summary>  
        public string prepay_id { get; set; }

        /// <summary>   
        ///    二维码链接 可空 String(64) trade_type为NATIVE时有返回，用于生成二维码，展示给用户进行扫码支付
        /// </summary>  
        public string code_url { get; set; }
         
        /// <summary>
        /// 支付跳转链接【H5支付使用】
        /// mweb_url为拉起微信支付收银台的中间页面，可通过访问该url来拉起微信客户端，完成支付,mweb_url的有效期为5分钟
        /// </summary>
        public string mweb_url { get; set; }

        /// <inheritdoc />
        protected override void FormatPropertiesFromMsg()
        {
            prepay_id = this["prepay_id"];
            code_url = this["code_url"];
            mweb_url = this["mweb_url"];
        }
    }

    #endregion

    /// <summary>
    ///   下单请求接口基类
    /// </summary>
    public class WxAddPayOrderBaseReq:WxPayBaseReq
    {
        /// <summary>   
        ///    设备号 可空 String(32) 自定义参数，可以为终端设备号(门店号或收银设备ID)，PC网页或公众号内支付可以传"WEB"
        /// </summary>  
        public string device_info { get; set; }

        /// <summary>   
        ///    商品描述 必填 String(128) 商品简单描述，该字段请按照规范传递，具体请见参数规定 https://pay.weixin.qq.com/wiki/doc/api/jsapi.php?chapter=4_2
        /// </summary>  
        public string body { get; set; }

        /// <summary>   
        ///    附加数据 可空 String(127) 附加数据，在查询API和支付通知中原样返回，可作为自定义参数使用。
        /// </summary>  
        public string attach { get; set; }

        /// <summary>   
        ///    商户订单号 必填 String(32) 商户系统内部订单号，要求32个字符内、且在同一个商户号下唯一。 详见商户订单号
        /// </summary>  
        public string out_trade_no { get; set; }

        /// <summary>   
        ///    标价币种 可空 String(16) 符合ISO 4217标准的三位字母代码，默认人民币：CNY，详细列表请参见货币类型
        /// </summary>  
        public string fee_type { get; set; } = "CNY";
        
        /// <summary>   
        ///    标价金额 必填 Int 订单总金额，单位为分，详见支付金额
        /// </summary>  
        public int total_fee { get; set; }

        /// <summary>   
        ///    终端IP 必填 String(16) APP和网页支付提交用户端ip，Native支付填调用微信支付API的机器IP。
        /// </summary>  
        public string spbill_create_ip { get; set; }

        /// <summary>   
        /// 订单优惠标记 可空 String(32) 商品标记，使用代金券或立减优惠功能时需要的参数，说明详见代金券或立减优惠
        /// </summary>  
        public string goods_tag { get; set; }
        
        /// <summary>
        /// 商品条目详情
        /// </summary>
        public string detail { get; set; }

        /// <summary>   
        ///    指定支付方式 可空 String(32) 上传此参数no_credit--可限制用户不能使用信用卡支付
        /// </summary>  
        public string limit_pay { get; set; }
        
        /// <summary>
        ///  设置需要需要运算的字典值
        /// </summary>
        protected override void SetSignDics()
        {
            SetDicItem("device_info", device_info);
            SetDicItem("body", body);
            SetDicItem("attach", attach);
            SetDicItem("out_trade_no", out_trade_no);
            SetDicItem("fee_type", fee_type);

            SetDicItem("total_fee", total_fee);
            SetDicItem("spbill_create_ip", spbill_create_ip);
            SetDicItem("goods_tag", goods_tag);
            SetDicItem("detail", detail);
            SetDicItem("limit_pay", limit_pay);
        }
    }
    
    /// <summary>
    /// 支付下单响应
    /// </summary>
    public class WxAddPayOrderBaseResp : WxPayBaseResp
    {
        /// <summary>   
        ///    设备号 可空 String(32) 自定义参数，可以为请求支付的终端设备号等
        /// </summary>  
        public string device_info { get; set; }

        /// <summary>   
        ///    交易类型 必填 String(16) 交易类型，取值为：JSAPI，NATIVE，APP,H5支付固定传 【MWEB】
        /// </summary>  
        public string trade_type { get; set; }


        /// <inheritdoc />
        protected override void FormatPropertiesFromMsg()
        {
            device_info = this["device_info"];
            trade_type = this["trade_type"];
        }
    }

    /// <summary>
    ///  获取公众号和小程序唤起客户端需要的实体
    /// </summary>
    public class WxGetJsClientParaResp : ResultMo
    {
        /// <summary>
        ///  公众号id
        /// </summary>
        public string app_id { get; set; }

        /// <summary>
        ///  时间戳
        /// </summary>
        public string time_stamp { get; set; }

        /// <summary>
        ///  随机字符串
        /// </summary>
        public string nonce { get; set; }

        /// <summary>
        ///  订单详情扩展字符串
        /// </summary>
        public string package { get; set; }

        /// <summary>
        ///  签名方式
        /// </summary>
        public string sign_type { get; set; } = "MD5";

        /// <summary>
        ///  签名
        /// </summary>
        public string sign { get; set; }
    }

    /// <summary>
    /// 获取App唤起客户端需要的实体
    /// </summary>
    public class WxGetAppClientParaResp : WxGetJsClientParaResp
    {
        /// <summary>
        ///  预支付交易会话ID	
        /// </summary>
        public string prepay_id { get; set; }

        /// <summary>
        ///  商户号Id
        /// </summary>
        public string mch_id { get; set; }
    }
}
