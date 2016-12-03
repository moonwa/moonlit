using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using log4net;
using Moonlit.Collections;
using Moonlit.IO;

namespace Moonlit.Weixin
{
    public class PaymentClient
    {
        public string AppId { get; }
        public string ApiKey { get; }
        public WeixinProxy ProxyPayment { get; set; }
        private static ILog Log = LogManager.GetLogger(typeof(PaymentClient));


        public PaymentClient(string appId, string paymentApiKey)
        {
            Log.Debug($"PaymentClient created: #{appId}");
            AppId = appId;
            ApiKey = paymentApiKey;
            ProxyPayment = new WeixinProxy(new WebClientWeixinProxy("https://api.mch.weixin.qq.com"));
        }

        public async Task<UnifiedOrderResponse> UnifiedOrderAsync(UnifiedOrder order)
        {
            order.AppId = AppId;
            var text = order.ToXml(ApiKey);

            return await ProxyPayment.PostAsXmlAsync<UnifiedOrderResponse>("pay/unifiedorder", text);
        }

        public JsapiNotify GetJsapiNotify(HttpRequestBase request)
        {
            var text = request.InputStream.ReadToEnd();
            Log.Debug("GetJsapiNotify parse text: " + text);
            JsapiNotify notify = PaymentObject.Deserialize<JsapiNotify>(text, this.ApiKey);
            if (notify != null)
            {
                Log.Debug($"GetJsapiNotify parsed returnCode={notify.ReturnCode} resultCode={notify.ResultCode}");
            }
            else
            {
                Log.Debug($"GetJsapiNotify parsed null notify");
            }
            return notify;
        }
    }

    public class JsapiNotifyResponse : PaymentObject
    {
        [XmlElement("return_code")]
        public ReturnCode ReturnCode { get; set; }
        [XmlElement("return_msg")]
        public string ReturnMessage { get; set; }

        public static JsapiNotifyResponse Ok()
        {
            return new JsapiNotifyResponse
            {
                ReturnCode = ReturnCode.SUCCESS,
                ReturnMessage = "OK",
            };
        }

        public static JsapiNotifyResponse Fail(string message)
        {
            return new JsapiNotifyResponse
            {
                ReturnCode = ReturnCode.FAIL,
                ReturnMessage = message,
            };
        }
    }
    public class JsapiNotify : IDeseriableWeixinObject
    {
        public void DeserializeObject(WeixinData data)
        {
            this.ReturnCode = data.GetEnum("return_code", ReturnCode.FAIL);
            this.ReturnMessage = data["return_msg"];
            if (ReturnCode == ReturnCode.SUCCESS)
            {
                this.AppId = data["appid"];
                this.MerchantId = data["mch_id"];
                this.DeviceInfo = data["device_info"];
                this.ResultCode = data.GetEnum("result_code", PaymentResult.FAIL);
                this.ErrCode = data["err_code"];
                this.ErrCodeDes = data["err_code_des"];
                this.OpenId = data["openid"];
                this.IsSubscribe = data.GetBoolean("is_subscribe", false);
                this.TradeType = data.GetEnum("trade_type", TradeType.JSAPI);
                this.BankType = data["bank_type"];
                this.TotalFee = data.GetInt32("total_fee", 0) / 100m;
                this.SettlementTotalFee = data.GetInt32("settlement_total_fee", 0) / 100m;
                this.FeeType = data.GetEnum("fee_type", FeeType.CNY);
                this.CashFee = data.GetInt32("cash_fee", 0) / 100m;
                this.CashFeeType = data.GetEnum("cash_fee_type", FeeType.CNY);
                this.CouponFee = data.GetInt32("coupon_fee", 0) / 100m;
                this.CouponCount = data.GetInt32("coupon_count", 0);
                // 以下代码，未测试
                //this.Coupons = new Coupon().Repeat(CouponCount).ToArray();
                //for (int i = 0; i < CouponCount; i++)
                //{
                //    Coupons[i].CouponType = data.GetEnum("coupon_type_$" + i, CouponType.Cash);
                //    Coupons[i].Id = data["coupon_id_$" + i];
                //    Coupons[i].Fee = data.GetInt32("coupon_id_$" + i, 0) / 100m;
                //}
                this.TransactionId = data["transaction_id"];
                this.OutTradeNo = data["out_trade_no"];
                this.Attach = data["attach"];
                this.TimeEnd = data.GetDateTime("time_end", DateTime.Now);
            }
        }
        /// <summary>
        /// 支付完成时间
        /// </summary>
        public DateTime TimeEnd { get; set; }

        /// <summary>
        /// 商家数据包
        /// </summary>
        public string Attach { get; set; }

        /// <summary>
        /// 商户订单号
        /// </summary>
        public string OutTradeNo { get; set; }
        /// <summary>
        /// 微信订单号
        /// </summary>
        public string TransactionId { get; set; }

        public Coupon[] Coupons { get; set; }

        /// <summary>
        /// 代金券使用数量	
        /// </summary>
        public int CouponCount { get; set; }

        /// <summary>
        /// 代金券金额	
        /// </summary>
        public decimal CouponFee { get; set; }

        /// <summary>
        /// 现金支付货币类型
        /// </summary>
        public FeeType CashFeeType { get; set; }

        /// <summary>
        /// 现金支付金额	
        /// </summary>
        public decimal CashFee { get; set; }

        /// <summary>
        /// 币种
        /// </summary>
        public FeeType FeeType { get; set; }

        /// <summary>
        /// 应结订单金额
        /// </summary>
        public decimal SettlementTotalFee { get; set; }
        /// <summary>
        /// 订单金额
        /// </summary>
        public decimal TotalFee { get; set; }
        /// <summary>
        /// 付款银行
        /// </summary>
        public string BankType { get; set; }

        public TradeType TradeType { get; set; }

        public bool IsSubscribe { get; set; }

        public string OpenId { get; set; }

        public string ErrCodeDes { get; set; }

        public string ErrCode { get; set; }

        public PaymentResult ResultCode { get; set; }

        public string MerchantId { get; set; }

        public string DeviceInfo { get; set; }

        public string AppId { get; set; }

        public string ReturnMessage { get; set; }

        public ReturnCode ReturnCode { get; set; }
    }

    public enum CouponType
    {
        /// <summary>
        /// 充值代金券 
        /// </summary>
        Cash,
        /// <summary>
        /// 非充值代金券
        /// </summary>
        NO_CASH
    }

    public class Coupon
    {
        public CouponType CouponType { get; set; }
        public string Id { get; set; }
        public decimal Fee { get; set; }
    }

    public enum PaymentResult
    {

        SUCCESS,
        FAIL
    }
    public enum ReturnCode
    {
        SUCCESS,
        FAIL
    }
}