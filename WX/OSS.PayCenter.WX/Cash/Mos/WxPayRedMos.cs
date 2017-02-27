#region Copyright (C) 2017 Kevin (OSS开源作坊) 公众号：osscoder

/***************************************************************************
*　　	文件功能描述：微信支付模快 —— 发送红包相关实体
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2017-2-27
*       
*****************************************************************************/

#endregion

using OSS.Common.Extention;

namespace OSS.PayCenter.WX.Cash.Mos
{

    #region 发送普通红包请求实体

    /// <summary>
    ///  发送红包请求请求实体
    /// </summary>
    public class WxPaySendRedReq : WxPayBaseReq
    {
        /// <summary>   
        ///    商户订单号 必填 String(28) 商户订单号（每个订单号必须唯一）组成：mch_id+yyyymmdd+10位一天内不能重复的数字。接口根据商户订单号支持重入，如出现超时可再调用。
        /// </summary>  
        public string mch_billno { get; set; }

        /// <summary>   
        ///    商户名称 必填 String(32) 红包发送者名称
        /// </summary>  
        public string send_name { get; set; }

        /// <summary>   
        ///    用户openid 必填 String(32) 接受红包的用户用户在wxappid下的openid
        /// </summary>  
        public string re_openid { get; set; }

        /// <summary>   
        ///    付款金额 必填 int 付款金额，单位分
        /// </summary>  
        public int total_amount { get; set; }

        /// <summary>   
        ///    红包发放总人数 必填 int 红包发放总人数total_num=1
        /// </summary>  
        public int total_num { get; set; }

        /// <summary>   
        ///    红包祝福语 必填 String(128) 红包祝福语
        /// </summary>  
        public string wishing { get; set; }

        /// <summary>   
        ///    Ip地址 必填 String(15) 调用接口的机器Ip地址
        /// </summary>  
        public string client_ip { get; set; }

        /// <summary>   
        ///    活动名称 必填 String(32) 活动名称
        /// </summary>  
        public string act_name { get; set; }

        /// <summary>   
        ///    备注 必填 String(256) 备注信息
        /// </summary>  
        public string remark { get; set; }

        /// <summary>   
        ///    场景id 可空 String(32) 发放红包使用场景，红包金额大于200时必传，
        /// PRODUCT_1:商品促销，PRODUCT_2:抽奖，
        /// PRODUCT_3:虚拟物品兑奖 ，PRODUCT_4:企业内部福利，
        /// PRODUCT_5:渠道分润，PRODUCT_6:保险回馈，
        /// PRODUCT_7:彩票派奖，PRODUCT_8:税务刮奖
        /// </summary>  
        public string scene_id { get; set; }

        /// <summary>   
        ///    活动信息 可空 String(128)
        /// posttime:用户操作的时间戳，mobile:业务系统账号的手机号，国家代码-手机号。不需要+号，
        /// deviceid :mac 地址或者设备唯一标识 ，clientversion :用户操作的客户端版本，
        /// 把值为非空的信息用key=value进行拼接，再进行urlencode：urlencode(posttime=xx& mobile =xx&deviceid=xx)
        /// </summary>  
        public string risk_info { get; set; }

        /// <summary>   
        ///    资金授权商户号 可空 String(32) 资金授权商户号服务商替特约商户发放时使用
        /// </summary>  
        public string consume_mch_id { get; set; }

        /// <summary>
        ///  设置当前实体中的字段值
        /// </summary>
        protected override void SetSignDics()
        {
            base.SetSignDics();
            SetDicItem("mch_billno", mch_billno);
            SetDicItem("send_name", send_name);
            SetDicItem("re_openid", re_openid);
            SetDicItem("total_amount", total_amount);
            SetDicItem("total_num", total_num);

            SetDicItem("wishing", wishing);
            SetDicItem("client_ip", client_ip);
            SetDicItem("act_name", act_name);
            SetDicItem("remark", remark);
            SetDicItem("scene_id", scene_id);

            SetDicItem("risk_info", risk_info);
            SetDicItem("consume_mch_id", consume_mch_id);
        }
    }

    /// <summary>
    ///   发送红包响应实体
    /// </summary>
    public class WxPaySendRedResp : WxPayBaseResp
    {
        /// <summary>   
        ///    商户订单号 必填 String(28) 商户订单号（每个订单号必须唯一）组成：mch_id+yyyymmdd+10位一天内不能重复的数字
        /// </summary>  
        public string mch_billno { get; set; }

        /// <summary>   
        ///    用户openid 必填 String(32) 接受收红包的用户用户在wxappid下的openid
        /// </summary>  
        public string re_openid { get; set; }

        /// <summary>   
        ///    付款金额 必填 int 付款金额，单位分
        /// </summary>  
        public int total_amount { get; set; }

        /// <summary>   
        ///    微信单号 必填 String(32) 红包订单的微信单号
        /// </summary>  
        public string send_listid { get; set; }

        /// <summary>   
        ///    公众账号appid 必填 String(32) 商户appid，接口传入的所有appid应该为公众号的appid（在mp.weixin.qq.com申请的），不能为APP的appid（在open.weixin.qq.com申请的）。
        /// </summary>  
        public string wxappid { get; set; }

        /// <summary>
        /// 格式化自身属性部分
        /// </summary>
        protected override void FormatPropertiesFromMsg()
        {
            base.FormatPropertiesFromMsg();
            mch_billno = this["mch_billno"];
            re_openid = this["re_openid"];
            total_amount = this["total_amount"].ToInt32();
            send_listid = this["send_listid"];
            wxappid = this["wxappid"];
        }
    }

    #endregion

    #region  发送裂变红包实体

    /// <summary>
    ///  发送裂变红包请求实体
    /// </summary>
    public class WxPaySendGroupRedReq : WxPaySendRedReq
    {
        /// <summary>   
        ///    红包金额设置方式 必填 String(32) 红包金额设置方式ALL_RAND—全部随机,商户指定总金额和红包发放总人数，由微信支付随机计算出各红包金额
        /// </summary>  
        public string amt_type { get; set; }

        /// <summary>
        ///  设置当前实体中的字段值
        /// </summary>
        protected override void SetSignDics()
        {
            base.SetSignDics();
            SetDicItem("amt_type", amt_type);
        }
    }


    #endregion
}