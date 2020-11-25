#region Copyright (C) 2017 Kevin (OSS开源作坊) 公众号：osscore

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

namespace OSS.Clients.Pay.WX.Coupon.Mos
{
    #region 发送代金券实体
    /// <summary>
    ///  发送代金券请求实体
    /// </summary>
    public class WXPaySendConpouReq:WXPayBaseReq
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
    public class WXPaySendConpouResp : WXPayBaseResp
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
    public class WXPayQueryConpouStockReq : WXPayBaseReq
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
    public class WXPayQueryConpouStockResp : WXPayBaseResp
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


    #region 查询代金券实体
    /// <summary>
    /// 查询代金券请求实体
    /// </summary>
    public class WXPayQueryConpouReq : WXPayBaseReq
    {
        /// <summary>   
        ///    代金券id 必填 String 代金券id
        /// </summary>  
        public string coupon_id { get; set; }

        /// <summary>   
        ///    用户标识 必填 String 用户在商户appid下的唯一标识
        /// </summary>  
        public string openid { get; set; }

        /// <summary>   
        ///    批次号 必填 String(32) 代金劵对应的批次号
        /// </summary>  
        public string stock_id { get; set; }

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
            SetDicItem("device_info", device_info);
            SetDicItem("coupon_id", coupon_id);
            SetDicItem("openid", openid);
            SetDicItem("stock_id", stock_id);
            SetDicItem("op_user_id", op_user_id);
        }
    }

    /// <summary>
    ///  查询代金券响应实体
    /// </summary>
    public class WXPayQueryConpouResp : WXPayBaseResp
    {
        /// <summary>   
        ///    子商户号 可空 String(32) 微信支付分配的子商户号，受理模式下必填
        /// </summary>  
        public string sub_mch_id { get; set; }

        /// <summary>   
        ///    设备号 可空 String(32) 微信支付分配的终端设备号
        /// </summary>  
        public string device_info { get; set; }

        /// <summary>   
        ///    批次ID 必填 String 代金券批次Id
        /// </summary>  
        public string coupon_stock_id { get; set; }

        /// <summary>   
        ///    批次类型 必填 int 批次类型；1-批量型，2-触发型
        /// </summary>  
        public int coupon_stock_type { get; set; }

        /// <summary>   
        ///    代金券id 必填 String 代金券id
        /// </summary>  
        public string coupon_id { get; set; }

        /// <summary>   
        ///    代金券面额 必填 Unsinged int 代金券面值,单位是分
        /// </summary>  
        public int coupon_value { get; set; }

        /// <summary>   
        ///    代金券使用门槛 必填 Unsinged int 代金券使用最低限额,单位是分
        /// </summary>  
        public int coupon_mininum { get; set; }

        /// <summary>   
        ///    代金券名称 必填 String 代金券名称
        /// </summary>  
        public string coupon_name { get; set; }

        /// <summary>   
        ///    代金券状态 必填 int 代金券状态：2-已激活，4-已锁定，8-已实扣
        /// </summary>  
        public int coupon_state { get; set; }

        /// <summary>   
        ///    代金券类型 必填 int 代金券类型：1-代金券无门槛，2-代金券有门槛互斥，3-代金券有门槛叠加，
        /// </summary>  
        public int coupon_type { get; set; }

        /// <summary>   
        ///    代金券描述 必填 String 代金券描述
        /// </summary>  
        public string coupon_desc { get; set; }

        /// <summary>   
        ///    实际优惠金额 必填 Unsinged int 代金券实际使用金额
        /// </summary>  
        public int coupon_use_value { get; set; }

        /// <summary>   
        ///    优惠剩余可用额 必填 Unsinged int 代金券剩余金额：部分使用情况下，可能会存在券剩余金额
        /// </summary>  
        public int coupon_remain_value { get; set; }

        /// <summary>   
        ///    生效开始时间 必填 String 格式为yyyyMMddhhmmss，如2009年12月27日9点10分10秒表示为20091227091010。
        /// </summary>  
        public string begin_time { get; set; }

        /// <summary>   
        ///    生效结束时间 必填 String 格式为yyyyMMddhhmmss，如2009年12月27日9点10分10秒表示为20091227091010。
        /// </summary>  
        public string end_time { get; set; }

        /// <summary>   
        ///    发放时间 必填 String 格式为yyyyMMddhhmmss，如2009年12月27日9点10分10秒表示为20091227091010。
        /// </summary>  
        public string send_time { get; set; }

        /// <summary>   
        ///    使用时间 可空 String 格式为yyyyMMddhhmmss，如2009年12月27日9点10分10秒表示为20091227091010。
        /// </summary>  
        public string use_time { get; set; }

        /// <summary>   
        ///    使用单号 可空 String 代金券使用后，关联的大单收单单号
        /// </summary>  
        public string trade_no { get; set; }

        /// <summary>   
        ///    消耗方商户id 可空 String 代金券使用后，消耗方商户id
        /// </summary>  
        public string consumer_mch_id { get; set; }

        /// <summary>   
        ///    消耗方商户名称 可空 String 代金券使用后，消耗方商户名称
        /// </summary>  
        public string consumer_mch_name { get; set; }

        /// <summary>   
        ///    消耗方商户appid 可空 String 代金券使用后，消耗方商户appid
        /// </summary>  
        public string consumer_mch_appid { get; set; }

        /// <summary>   
        ///    发放来源 必填 String 代金券发放来源:JIFA-即发即用 NORMAL-普通发劵 FULL_SEND-满送活动送劵 SCAN_CODE-扫码领劵 OZ-刮奖领劵 AJUST-对账调账
        /// </summary>  
        public string send_source { get; set; }

        /// <summary>   
        ///    是否允许部分使用 可空 String 该代金券是否允许部分使用标识：1-表示支持部分使用
        /// </summary>  
        public string is_partial_use { get; set; }

        /// <summary>
        /// 格式化自身属性部分
        /// </summary>
        protected override void FormatPropertiesFromMsg()
        {
            base.FormatPropertiesFromMsg();
            sub_mch_id = this["sub_mch_id"];
            device_info = this["device_info"];
            coupon_stock_id = this["coupon_stock_id"];
            coupon_stock_type = this["coupon_stock_type"].ToInt32();
            coupon_id = this["coupon_id"];

            coupon_value = this["coupon_value"].ToInt32();
            coupon_mininum = this["coupon_mininum"].ToInt32();
            coupon_name = this["coupon_name"];
            coupon_state = this["coupon_state"].ToInt32();
            coupon_type = this["coupon_type"].ToInt32();

            coupon_desc = this["coupon_desc"];
            coupon_use_value = this["coupon_use_value"].ToInt32();
            coupon_remain_value = this["coupon_remain_value"].ToInt32();
            begin_time = this["begin_time"];
            end_time = this["end_time"];

            send_time = this["send_time"];
            use_time = this["use_time"];
            trade_no = this["trade_no"];
            consumer_mch_id = this["consumer_mch_id"];
            consumer_mch_name = this["consumer_mch_name"];

            consumer_mch_appid = this["consumer_mch_appid"];
            send_source = this["send_source"];
            is_partial_use = this["is_partial_use"];
        }
    }

    #endregion
}
