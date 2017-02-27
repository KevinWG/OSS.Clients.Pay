#region Copyright (C) 2017 Kevin (OSS开源作坊) 公众号：osscoder

/***************************************************************************
*　　	文件功能描述：微信支付模快 —— 代金券相关实体
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2017-2-27
*       
*****************************************************************************/

#endregion

using OSS.Common.Extention;

namespace OSS.PayCenter.WX.Coupon.Mos
{
    #region 发送代金券实体
    /// <summary>
    ///  发送代金券请求实体
    /// </summary>
    public class WxPaySendConpouReq:WxPayBaseReq
    {
        /// <summary>   
        ///    代金券批次id 必填 String 代金券批次id
        /// </summary>  
        public string coupon_stock_id { get; set; }

        /// <summary>   
        ///    openid记录数 必填 int openid记录数（目前支持num=1）
        /// </summary>  
        public int openid_count { get; set; } = 1;

        /// <summary>   
        ///    商户单据号 必填 String 商户此次发放凭据号（格式：商户id+日期+流水号），商户侧需保持唯一性
        /// </summary>  
        public string partner_trade_no { get; set; }

        /// <summary>   
        ///    用户openid 必填 String Openid信息
        /// </summary>  
        public string openid { get; set; }

        /// <summary>   
        ///    操作员 可空 String(32) 操作员帐号, 默认为商户号 可在商户平台配置操作员对应的api权限
        /// </summary>  
        public string op_user_id { get; set; }

        /// <summary>   
        ///    设备号 可空 String(32) 微信支付分配的终端设备号
        /// </summary>  
        public string device_info { get; set; }
        
        protected override void SetSignDics()
        {
            base.SetSignDics();
            SetDicItem("coupon_stock_id", coupon_stock_id);
            SetDicItem("openid_count", openid_count);
            SetDicItem("partner_trade_no", partner_trade_no);
            SetDicItem("openid", openid);
            SetDicItem("op_user_id", op_user_id);
            SetDicItem("device_info", device_info);
        }
    }

    /// <summary>
    ///   发送代金券响应实体
    /// </summary>
    public class WxPaySendConpouResp : WxPayBaseResp
    {
        /// <summary>   
        ///    设备号 可空 String(32) 微信支付分配的终端设备号
        /// </summary>  
        public string device_info { get; set; }

        /// <summary>   
        ///    代金券批次id 必填 String 用户在商户appid下的唯一标识
        /// </summary>  
        public string coupon_stock_id { get; set; }

        /// <summary>   
        ///    返回记录数 必填 Int 返回记录数
        /// </summary>  
        public int resp_count { get; set; }

        /// <summary>   
        ///    成功记录数 必填 Int 成功记录数
        /// </summary>  
        public int success_count { get; set; }

        /// <summary>   
        ///    失败记录数 必填 Int 失败记录数
        /// </summary>  
        public int failed_count { get; set; }

        /// <summary>   
        ///    用户标识 必填 String 用户在商户appid下的唯一标识
        /// </summary>  
        public string openid { get; set; }

        /// <summary>   
        ///    返回码 必填 String 返回码，SUCCESS/FAILED
        /// </summary>  
        public string ret_code { get; set; }

        /// <summary>   
        ///    代金券id 必填 String 对一个用户成功发放代金券则返回代金券id，即ret_code为SUCCESS的时候； 如果ret_code为FAILED则填写空串""
        /// </summary>  
        public string coupon_id { get; set; }

        /// <summary>   
        ///    返回信息 必填 String 返回信息，当返回码是FAILED的时候填写，否则填空串“”
        /// </summary>  
        public string ret_msg { get; set; }


        protected override void FormatPropertiesFromMsg()
        {
            base.FormatPropertiesFromMsg();
            device_info = this["device_info"];
            coupon_stock_id = this["coupon_stock_id"];
            resp_count = this["resp_count"].ToInt32();
            success_count = this["success_count"].ToInt32();
            failed_count = this["failed_count"].ToInt32();


            openid = this["openid"];
            ret_code = this["ret_code"];
            coupon_id = this["coupon_id"];
            ret_msg = this["ret_msg"];
        }
    }
    #endregion


    #region 查询代金券批次实体 
    /// <summary>
    ///   查询代金券批次请求实体
    /// </summary>
    public class WxPayQueryConpouStockReq : WxPayBaseReq
    {
        /// <summary>   
        ///    代金券批次id 必填 String 代金券批次id
        /// </summary>  
        public string coupon_stock_id { get; set; }

        /// <summary>   
        ///    操作员 可空 String(32) 操作员帐号, 默认为商户号可在商户平台配置操作员对应的api权限
        /// </summary>  
        public string op_user_id { get; set; }

        /// <summary>   
        ///    设备号 可空 String(32) 微信支付分配的终端设备号
        /// </summary>  
        public string device_info { get; set; }
    }

    /// <summary>
    ///  查询代金券批次响应实体
    /// </summary>
    public class WxPayQueryConpouStockResp : WxPayBaseResp
    {/// <summary>   
     ///    设备号 可空 String(32) 微信支付分配的终端设备号
     /// </summary>  
        public string device_info { get; set; }

        /// <summary>   
        ///    代金券批次ID 必填 String 代金券批次Id
        /// </summary>  
        public string coupon_stock_id { get; set; }

        /// <summary>   
        ///    代金券名称 可空 String 代金券名称
        /// </summary>  
        public string coupon_name { get; set; }

        /// <summary>   
        ///    代金券面额 必填 Unsinged int 代金券面值,单位是分
        /// </summary>  
        public int coupon_value { get; set; }

        /// <summary>   
        ///    代金券使用最低限额 可空 Unsinged int 代金券使用最低限额,单位是分
        /// </summary>  
        public int coupon_mininumn { get; set; }

        /// <summary>   
        ///    代金券类型 必填 int 代金券类型：1-代金券无门槛，2-代金券有门槛互斥，3-代金券有门槛叠加，
        /// </summary>  
        public int coupon_type { get; set; }

        /// <summary>   
        ///    代金券批次状态 必填 int 批次状态： 1-未激活；2-审批中；4-已激活；8-已作废；16-中止发放；
        /// </summary>  
        public int coupon_stock_status { get; set; }

        /// <summary>   
        ///    代金券数量 必填 Unsigned int 代金券数量
        /// </summary>  
        public int coupon_total { get; set; }

        /// <summary>   
        ///    代金券最大领取数量 可空 Unsigned int 代金券每个人最多能领取的数量, 如果为0，则表示没有限制
        /// </summary>  
        public int max_quota { get; set; }

        /// <summary>   
        ///    代金券锁定数量 可空 Unsigned int 代金券锁定数量
        /// </summary>  
        public int locked_num { get; set; }

        /// <summary>   
        ///    代金券已使用数量 可空 Unsigned int 代金券已使用数量
        /// </summary>  
        public int used_num { get; set; }

        /// <summary>   
        ///    代金券已经发送的数量 可空 Unsigned int 代金券已经发送的数量
        /// </summary>  
        public int is_send_num { get; set; }

        /// <summary>   
        ///    生效开始时间 必填 String 格式为yyyyMMddhhmmss，如2009年12月27日9点10分10秒表示为20091227091010。
        /// </summary>  
        public string begin_time { get; set; }

        /// <summary>   
        ///    生效结束时间 必填 String 格式为yyyyMMddhhmmss，如2009年12月27日9点10分10秒表示为20091227091010。
        /// </summary>  
        public string end_time { get; set; }

        /// <summary>   
        ///    创建时间 必填 String 格式为yyyyMMddhhmmss，如2009年12月27日9点10分10秒表示为20091227091010。
        /// </summary>  
        public string create_time { get; set; }

        /// <summary>   
        ///    代金券预算额度 可空 Unsigned int 代金券预算额度
        /// </summary>  
        public int coupon_budget { get; set; }

        protected override void FormatPropertiesFromMsg()
        {
            base.FormatPropertiesFromMsg();
            device_info = this["device_info"];
            coupon_stock_id = this["device_info"];
            coupon_name = this["coupon_name"];
            coupon_value = this["coupon_value"].ToInt32();
            coupon_mininumn = this["coupon_mininumn"].ToInt32();

            coupon_type = this["coupon_type"].ToInt32();
            coupon_stock_status = this["coupon_stock_status"].ToInt32();
            coupon_total = this["coupon_total"].ToInt32();
            max_quota = this["max_quota"].ToInt32();
            locked_num = this["locked_num"].ToInt32();

            used_num = this["used_num"].ToInt32();
            is_send_num = this["is_send_num"].ToInt32();
            begin_time = this["begin_time"];
            end_time = this["end_time"];
            create_time = this["create_time"];

            coupon_budget = this["coupon_budget"].ToInt32();
        }
    }

    #endregion
}
