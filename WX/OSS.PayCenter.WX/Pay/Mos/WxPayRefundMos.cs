#region Copyright (C) 2017 Kevin (OSS开源作坊) 公众号：osscoder

/***************************************************************************
*　　	文件功能描述：微信支付模快 —— 退款对应实体
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*    	创建日期：2017-2-26
*       
*****************************************************************************/

#endregion

using System.Collections.Generic;
using OSS.Common.Extention;

namespace OSS.PayCenter.WX.Pay.Mos
{
    #region  退款实体
    /// <summary>
    ///  请求退款实体
    /// </summary>
    public class WxPayRefundReq : WxPayBaseReq
    {
        /// <summary>   
        ///    设备号 可空 String(32) 终端设备号
        /// </summary>  
        public string device_info { get; set; }

        /// <summary>   
        ///    微信订单号 和商户订单号二选一 String(28) 微信生成的订单号，在支付通知中有返回
        /// </summary>  
        public string transaction_id { get; set; }

        /// <summary>   
        ///    商户订单号 String(32) 
        /// </summary>
        public string out_trade_no { get; set; }

        /// <summary>   
        ///    商户退款单号 必填 String(32) 商户系统内部的退款单号，商户系统内部唯一，同一退款单号多次请求只退一笔
        /// </summary>  
        public string out_refund_no { get; set; }

        /// <summary>   
        ///    订单金额 必填 Int 订单总金额，单位为分，只能为整数，详见支付金额
        /// </summary>  
        public int total_fee { get; set; }

        /// <summary>   
        ///    退款金额 必填 Int 退款总金额，订单总金额，单位为分，只能为整数，详见支付金额
        /// </summary>  
        public int refund_fee { get; set; }

        /// <summary>   
        ///    货币种类 可空 String(8) 货币类型，符合ISO 4217标准的三位字母代码，默认人民币：CNY，其他值列表详见货币类型
        /// </summary>  
        public string refund_fee_type { get; set; }

        /// <summary>   
        ///    操作员 必填 String(32) 操作员帐号, 默认为商户号
        /// </summary>  
        public string op_user_id { get; set; }

        /// <summary>   
        ///    退款资金来源 可空 String(30) 仅针对老资金流商户使用 REFUND_SOURCE_UNSETTLED_FUNDS---未结算资金退款（默认使用未结算资金退款）REFUND_SOURCE_RECHARGE_FUNDS---可用余额退款
        /// </summary>  
        public string refund_account { get; set; }

        protected override void SetSignDics()
        {
            SetDicItem("device_info", device_info);
            SetDicItem("transaction_id", transaction_id);
            SetDicItem("out_trade_no", out_trade_no);
            SetDicItem("out_refund_no", out_refund_no);
            SetDicItem("total_fee", total_fee);

            SetDicItem("refund_fee", refund_fee);
            SetDicItem("refund_fee_type", refund_fee_type);
            SetDicItem("op_user_id", op_user_id);
            SetDicItem("refund_account", refund_account);


        }
    }

    public class WxPayRefundResp : WxPayBaseResp
    {
        /// <summary>   
        ///    设备号 可空 String(32) 终端设备号
        /// </summary>  
        public string device_info { get; set; }

        /// <summary>   
        ///    微信订单号 必填 String(28) 微信订单号
        /// </summary>  
        public string transaction_id { get; set; }

        /// <summary>   
        ///    商户订单号 必填 String(32) 商户系统内部的订单号
        /// </summary>  
        public string out_trade_no { get; set; }

        /// <summary>   
        ///    商户退款单号 必填 String(32) 商户退款单号
        /// </summary>  
        public string out_refund_no { get; set; }

        /// <summary>   
        ///    微信退款单号 必填 String(28) 微信退款单号
        /// </summary>  
        public string refund_id { get; set; }

        /// <summary>   
        ///    退款渠道 可空 String(16) ORIGINAL—原路退款 BALANCE—退回到余额
        /// </summary>  
        public string refund_channel { get; set; }

        /// <summary>   
        ///    退款金额 必填 Int 退款总金额,单位为分,可以做部分退款
        /// </summary>  
        public int refund_fee { get; set; }

        /// <summary>   
        ///    应结退款金额 可空 Int 去掉非充值代金券退款金额后的退款金额，退款金额=申请退款金额-非充值代金券退款金额，退款金额<=申请退款金额
        /// </summary>  
        public int settlement_refund_fee { get; set; }

        /// <summary>   
        ///    标价金额 必填 Int 订单总金额，单位为分，只能为整数，详见支付金额
        /// </summary>  
        public int total_fee { get; set; }

        /// <summary>   
        ///    应结订单金额 可空 Int 去掉非充值代金券金额后的订单总金额，应结订单金额=订单金额-非充值代金券金额，应结订单金额<=订单金额。
        /// </summary>  
        public int settlement_total_fee { get; set; }

        /// <summary>   
        ///    标价币种 可空 String(8) 订单金额货币类型，符合ISO 4217标准的三位字母代码，默认人民币：CNY，其他值列表详见货币类型
        /// </summary>  
        public string fee_type { get; set; }

        /// <summary>   
        ///    现金支付金额 必填 Int 现金支付金额，单位为分，只能为整数，详见支付金额
        /// </summary>  
        public string cash_fee { get; set; }

        /// <summary>   
        ///    现金支付币种 可空 String(16) 货币类型，符合ISO 4217标准的三位字母代码，默认人民币：CNY，其他值列表详见货币类型
        /// </summary>  
        public string cash_fee_type { get; set; }

        /// <summary>   
        ///    现金退款金额 可空 Int 现金退款金额，单位为分，只能为整数，详见支付金额
        /// </summary>  
        public string cash_refund_fee { get; set; }

        /// <summary>   
        ///    代金券退款总金额 可空 Int 代金券退款金额小于退款金额，退款金额-代金券或立减优惠退款金额为现金，说明详见代金券或立减优惠
        /// </summary>  
        public string coupon_refund_fee { get; set; }

        /// <summary>   
        ///    退款代金券使用数量 可空 Int 退款代金券使用数量
        /// </summary>  
        public int coupon_refund_count { get; set; }

        /// <summary>
        /// 退款代金券信息
        /// </summary>
        public List<WxPayOrderCouponMo> refund_coupons { get; set; }

        /// <summary>
        /// 格式化自身属性部分
        /// </summary>
        protected override void FormatPropertiesFromMsg()
        {
            device_info = this["device_info"];
            transaction_id = this["transaction_id"];
            out_trade_no = this["out_trade_no"];
            out_refund_no = this["out_refund_no"];
            refund_id = this["refund_id"];

            refund_channel = this["refund_channel"];
            refund_fee = this["refund_fee"].ToInt32();
            settlement_refund_fee = this["settlement_refund_fee"].ToInt32();
            total_fee = this["total_fee"].ToInt32();
            settlement_total_fee = this["settlement_total_fee"].ToInt32();

            fee_type = this["fee_type"];
            cash_fee = this["cash_fee"];
            cash_fee_type = this["cash_fee_type"];
            cash_refund_fee = this["cash_refund_fee"];
            coupon_refund_fee = this["coupon_refund_fee"];

            coupon_refund_count = this["coupon_refund_count"].ToInt32();
            if (coupon_refund_count > 0)
            {
                refund_coupons = new List<WxPayOrderCouponMo>(coupon_refund_count);
                for (int i = 0; i < coupon_refund_count; i++)
                {
                    var coupon = new WxPayOrderCouponMo();
                    coupon.coupon_fee = this["coupon_refund_fee_" + i].ToInt32();
                    coupon.coupon_id = this["coupon_refund_id_" + i];
                    coupon.coupon_type = this["coupon_type_" + i];
                }
            }
        }
    }
    
    #endregion


    public class WxPayGetRefundReq : WxPayBaseReq
    {
        /// <summary>   
        ///    设备号 可空 String(32) 商户自定义的终端设备号，如门店编号、设备的ID等
        /// </summary>  
        public string device_info { get; set; }

        /// <summary>   
        ///    微信订单号 以下编号四选一 String(32) 微信订单号
        /// </summary>  
        public string transaction_id { get; set; }

        /// <summary>   
        ///    商户订单号 String(32) 1217752501201407033233368018
        /// </summary>  
        public string out_trade_no { get; set; }

        /// <summary>   
        ///    商户退款单号 String(32) 1217752501201407033233368018
        /// </summary>  
        public string out_refund_no { get; set; }

        /// <summary>   
        ///    微信退款单号 String(28) 1217752501201407033233368018
        /// </summary>  
        public string refund_id { get; set; }

        /// <summary>
        ///  设置当前实体中涉及加密的字段
        /// </summary>
        protected override void SetSignDics()
        {
            SetDicItem("device_info", device_info);
            SetDicItem("transaction_id", transaction_id);
            SetDicItem("out_trade_no", out_trade_no);
            SetDicItem("out_refund_no", out_refund_no);
            SetDicItem("refund_id", refund_id);
        }
    }

    /// <summary>
    /// 查询退款响应实体
    /// </summary>
    public class WxPayGetRefundResp : WxPayBaseResp
    {
        /// <summary>   
        ///    设备号 可空 String(32) 终端设备号
        /// </summary>  
        public string device_info { get; set; }

        /// <summary>   
        ///    微信订单号 必填 String(32) 微信订单号
        /// </summary>  
        public string transaction_id { get; set; }

        /// <summary>   
        ///    商户订单号 必填 String(32) 商户系统内部的订单号
        /// </summary>  
        public string out_trade_no { get; set; }

        /// <summary>   
        ///    订单金额 必填 Int 订单总金额，单位为分，只能为整数，详见支付金额
        /// </summary>  
        public int total_fee { get; set; }

        /// <summary>   
        ///    应结订单金额 可空 Int 应结订单金额=订单金额-非充值代金券金额，应结订单金额《=订单金额。
        /// </summary>  
        public int settlement_total_fee { get; set; }

        /// <summary>   
        ///    货币种类 可空 String(8) 订单金额货币类型，符合ISO 4217标准的三位字母代码，默认人民币：CNY，其他值列表详见货币类型
        /// </summary>  
        public string fee_type { get; set; }

        /// <summary>   
        ///    现金支付金额 必填 Int 现金支付金额，单位为分，只能为整数，详见支付金额
        /// </summary>  
        public int cash_fee { get; set; }

        /// <summary>   
        ///    退款笔数 必填 Int 退款记录数
        /// </summary>  
        public int refund_count { get; set; }

        /// <summary>   
        ///    退款资金来源 可空 String(30) REFUND_SOURCE_RECHARGE_FUNDS---可用余额退款/基本账户，REFUND_SOURCE_UNSETTLED_FUNDS---未结算资金退款
        /// </summary>  
        public string refund_account { get; set; }

        /// <summary>
        /// 退款记录信息
        /// </summary>
        public List<WxPayRefundItemMo> refund_items { get; set; }


        /// <summary>
        /// 格式化自身属性部分
        /// </summary>
        protected override void FormatPropertiesFromMsg()
        {
            device_info = this["device_info"];
            transaction_id = this["transaction_id"];
            out_trade_no = this["out_trade_no"];
            total_fee = this["total_fee"].ToInt32();
            settlement_total_fee = this["settlement_total_fee"].ToInt32();

            fee_type = this["fee_type"];
            cash_fee = this["cash_fee"].ToInt32();
            refund_account = this["refund_account"];
            refund_count = this["refund_count"].ToInt32();

            if (refund_count>0)
            {
                var refundItems=new List<WxPayRefundItemMo>(refund_count);
                for (int i = 0; i < refund_count; i++)
                {
                    var item = new WxPayRefundItemMo();
                    item.out_refund_no = this["out_refund_no_" + i];
                    item.refund_id = this["refund_id_" + i];
                    item.refund_channel = this["refund_channel_" + i];
                    item.settlement_refund_fee = this["settlement_refund_fee_" + i].ToInt32();
                    item.coupon_type = this["coupon_type_" + i];

                    item.coupon_refund_fee = this["coupon_refund_fee_" + i].ToInt32();
                    item.coupon_refund_count = this["coupon_refund_count_" + i].ToInt32();
                    item.refund_status = this["refund_status_" + i];
                    item.refund_recv_accout = this["refund_recv_accout_" + i];
                    if (item.coupon_refund_count>0)
                    {
                        var refundCouponItems = new List<WxPayRefundCouponItemMo>(item.coupon_refund_count);
                        for (int j = 0; j < item.coupon_refund_count; j++)
                        {
                            var couponItem = new WxPayRefundCouponItemMo();

                            couponItem.coupon_refund_fee = this[$"coupon_refund_fee_{i}_{j}"].ToInt32();
                            couponItem.coupon_refund_id = this[$"coupon_refund_id_{i}_{j}"];

                            refundCouponItems.Add(couponItem);
                        }
                        item.coupons = refundCouponItems;
                    }
                }
                refund_items = refundItems;
            }

        }
    }

    public class WxPayRefundItemMo
    {

        /// <summary>   
        ///    商户退款单号 必填 String(32) 商户退款单号
        /// </summary>  
        public string out_refund_no { get; set; }

        /// <summary>   
        ///    微信退款单号 必填 String(28) 微信退款单号
        /// </summary>  
        public string refund_id { get; set; }

        /// <summary>   
        ///    退款渠道 可空 String(16) ORIGINAL—原路退款BALANCE—退回到余额申请退款金额 refund_fee_$n 必填 Int 100 退款总金额,单位为分,可以做部分退款
        /// </summary>  
        public string refund_channel { get; set; }

        /// <summary>   
        ///    退款金额 可空 Int 退款金额=申请退款金额-非充值代金券退款金额，退款金额《=申请退款金额
        /// </summary>  
        public int settlement_refund_fee { get; set; }

        /// <summary>   
        ///    代金券类型 可空 CASH--充值代金券 ，NO_CASH---非充值代金券，订单使用代金券时有返回（取值：CASH、NO_CASH）。$n为下标,从0开始编号，举例：coupon_type_$0
        /// </summary>  
        public string coupon_type { get; set; }

        /// <summary>   
        ///    总代金券退款金额 可空 Int 代金券退款金额小于等于退款金额，退款金额-代金券或立减优惠退款金额为现金，说明详见代金券或立减优惠
        /// </summary>  
        public int coupon_refund_fee { get; set; }

        /// <summary>   
        ///    退款代金券使用数量 可空 Int 退款代金券使用数量 ,$n为下标,从0开始编号
        /// </summary>  
        public int coupon_refund_count { get; set; }

        /// <summary>   
        ///    退款状态 必填 String(16) 退款状态：SUCCESS—退款成功，FAIL—退款失败，PROCESSING—退款处理中，CHANGE—转入代发，退款到银行发现用户的卡作废或者冻结了，导致原路退款银行卡失败，资金回流到商户的现金帐号，需要商户人工干预，通过线下或者财付通转账的方式进行退款。
        /// </summary>  
        public string refund_status { get; set; }

        /// <summary>   
        ///    退款入账账户 必填 String(64) 取当前退款单的退款入账方1）退回银行卡：{银行名称}{卡类型}{卡尾号}2）退回支付用户零钱:支付用户零钱
        /// </summary>  
        public string refund_recv_accout { get; set; }

        /// <summary>
        ///  退款记录对应的代金券信息
        /// </summary>
        public List<WxPayRefundCouponItemMo> coupons { get; set; }
    }

    public class WxPayRefundCouponItemMo
    {
        /// <summary>   
        ///    退款代金券ID 可空 String(20) 退款代金券ID, $n为下标，$m为下标，从0开始编号
        /// </summary>  
        public string coupon_refund_id { get; set; }

        /// <summary>   
        ///    单个代金券退款金额 可空 Int 单个退款代金券支付金额, $n为下标，$m为下标，从0开始编号
        /// </summary>  
        public int coupon_refund_fee { get; set; }
    }



}
