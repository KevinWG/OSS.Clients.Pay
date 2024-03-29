
# 快速了解
    OSS支付SDK，主要打造微信支付，支付宝支付，以及其他银行支付接口标准库项目
	此项目以标准库的形式提供服务，也就是可以同时支持.Net Framework(4.6及以上版本) 和 .Net Core
如果有问题，也可以在公众号(osscore)中提问:

![osscore](http://7xil4i.com1.z0.glb.clouddn.com/osscore_icon.jpg)

# OSS.PayCenter 使用

### 一. 安装使用

      nuget下安装命令：**Install-Package OSS.Clients.Pay.WX**	（微信支付   V2 接口版本

      nuget下安装命令：**Install-Package OSS.Clients.Pay.Wechat**	（微信支付   V3 接口版本

      nuget下安装命令：**Install-Package OSS.Clients.Pay.Ali**	（支付宝支付

支付宝接口建议使用官方 [alipay-easysdk](https://github.com/alipay/alipay-easysdk/tree/master/csharp) ,其实现已经简单方便

### 二. 微信支付接口（V3

V3版接口采用fluent方式简化了调用流程，方便扩展，只需要继承基础类BasePostReq<TReq, TResp>，和 BaseGetReq<TReq, TResp> ，使用时调用SendAsync 扩展方法，内部已经完成了需要的加密验签等处理操作

1. 扫码支付

首先添加全局配置信息
```csharp
    WechatPayHelper.pay_config = new WechatPayConfig()
    {
        app_id        = "xxxxxx",
        mch_id        = "xxxxxx",
        api_key       = "xxxxxx",
        api_v3_key    = "xxxxxx",
        cert_path     = "E:\\xxxxxx\\apiclient_cert.p12",
        cert_password = "xxxxxx"
    };
```
发起Native扫码下单调用

```csharp
   var nResp = await new WechatNativePayReq()
       {
           total        = 1,
           description  = "测试商品",
           out_trade_no = orderId,
           notify_url   = "http://你的回调接收地址(如 ReceivePayResult)"
       }
       // .SetContextConfig(new WechatPayConfig(){})   // 可以设置当前上下文的配置信息，设置后本次请求将使用此配置，方便多应用的用户
       //.AddOptionalBodyPara("attach","附加数据")  // 添加可选参数
       .SendAsync();
```

### 三. 老版本调用示例

1. 微信调用示例：

```csharp
// 声明配置
private static WXPayCenterConfig config= new WXPayCenterConfig()
{
    AppId = "xxxxxxxxxx",
    MchId = "xxxxxxxxxx",
    Key = "xxxxxxxxxx",
    AppSecret = "xxxxxxxxxx",

    CertPassword = "xxxxxxxxxx",
    CertPath = "cert/xxxxxxxxxx.p12"   
};
//  公众号调用示例
private static WXPayTradeApi _api=new WXPayTradeApi(config);

public async Task<IActionResult> GetJsPayInfo()
{
    var order = new WXAddPayUniOrderReq
    {
        notify_url = "http://你的域名/wxpay/receive",
        body = "OSSCoder-测试商品",
        device_info = "WEB",
        openid = "oldRAw-Wu4eOD5CVPWeWVDOvhRbo",
        out_trade_no = "123456789",
        spbill_create_ip = "114.242.25.208",
        total_fee = 1,
        trade_type = "JSAPI"
    };
    
    var orderRes=await _api.AddUniOrderAsync(order);
    if (!orderRes.IsSuccess()) return Json(orderRes);

    var jsPara = _api.GetJsClientParaResp(orderRes.prepay_id);
    return Json(jsPara);
}
public IActionResult receive()
{
    string strPayResult;
    using (var streamReader = new StreamReader(Request.Body))
    {
        strPayResult = streamReader.ReadToEnd();
    }
    var payRes = _api.DecryptTradeResult(strPayResult);
    // to do something with payRes
    var returnXml = _api.GetCallBackReturnXml(new Resp());
    return Content(returnXml);
}
```

2. 支付宝调用示例

```csharp
 private static string privateKey = "自定义私钥";
 private static string publicKey = "支付宝提供的公钥";

protected static ZPayCenterConfig ZPayConfig { get; set; } = new ZPayCenterConfig()
{
    AppSource = "1",
    AppId = "xxxxxxxxxx",
    AppPrivateKey = privateKey,
    AppPublicKey = publicKey
};

protected static ZPayTradeApi zPayApi = new ZPayTradeApi(ZPayConfig);

public async Task<IActionResult> ZPay(PayOrderMo order)
{
    string orderNum = DateTime.Now.ToString("yyyyMMddHHmmss");

    var payReq = new ZAddPreTradeReq("http://pay.sample.osscore.com/base/ZCallBack");

    payReq.body = order.order_name;
    payReq.out_trade_no = orderNum;
    payReq.total_amount = order.order_price;
    payReq.subject = order.order_name;

    var res =await zPayApi.AddPreTradeAsync(payReq);
    return Json(res);
}
```

### 三. 注意事项
	
	因为底层接口使用了Task返回，在.Net Framework MVC项目中如果你的在action中通过 Result或者wait等方式，直接返回ActionResult对象，界面会出现假死状态，解决方案：
	1. action 返回类型 ActionResult 修改为 async Task<ActionResult>, 全程异步
	2. 将异步调用方法 通过 var task=  Task.Run(()=> 异步方法 )返回异步对象，再使用task.Result即可