using System;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Moonlit.Weixin
{
    public enum FeeType
    {
        CNY = 1,
    }

    public enum TradeType
    {
        JSAPI = 1, NATIVE = 2, APP = 3
    }

    /// <summary>
    /// 支付对象
    /// </summary>
    [Serializable]
    [XmlRoot("xml")]
    public class JsapiPay : PaymentObject
    {
        protected override void Sign(string key, XElement xele)
        {
            xele.Element("paySign")?.Remove();
            xele.Add(new XElement("paySign", DoSign(xele, key)));
            
        }

        [XmlElement("appId")]
        public string AppId { get; set; }
        /// <summary>
        /// prepay_id=123456789
        /// </summary>
        [XmlIgnore]
        public string PrepayId { get; set; }
        protected override void Serialize(XElement xelement)
        {
            xelement.Add(new XElement("timeStamp", (int)(DateTime.Now - DateTime.Parse("1970-1-1")).TotalSeconds));
            xelement.Add(new XElement("nonceStr", Guid.NewGuid().ToString("N")));
            xelement.Add(new XElement("paySign", "MD5"));
            if (!string.IsNullOrEmpty(PrepayId))
            {
                xelement.Add(new XElement("package", $"prepay_id={PrepayId}"));
            }
            xelement.Add(new XElement("signType", "MD5"));
        }
    }
    /// <summary>
    /// 统一下单接口
    /// </summary>
    [Serializable]
    [XmlRoot("xml")]
    public class UnifiedOrder : PaymentObject
    {
        public const string DeviceInfoWeb = "WEB";

        public static UnifiedOrder CreateJsapi()
        {
            return new UnifiedOrder
            {
                TradeType = TradeType.JSAPI,
                DeviceInfo = DeviceInfoWeb,
            };
        }

        protected override void Serialize(XElement xelement)
        {
            xelement.Element("time_start").Value = this.TimeStart.ToString("yyyyMMddHHmmss");
            xelement.Element("time_expire").Value = this.TimeExpire.ToString("yyyyMMddHHmmss");
            xelement.Add(new XElement("nonce_str", Guid.NewGuid().ToString("N")));
        }

        /// <summary>
        /// 微信分配的公众账号ID（企业号corpid即为此appId）
        /// </summary>
        [XmlElement("appid")]
        public string AppId { get; set; }
        /// <summary>
        /// 微信支付分配的商户号
        /// </summary>
        [XmlElement("mch_id")]
        public string MerchantName { get; set; }
        /// <summary>
        /// 终端设备号(门店号或收银设备ID)，注意：PC网页或公众号内支付请传"WEB"
        /// </summary>
        [XmlElement("device_info")]
        public string DeviceInfo { get; set; }
        /// <summary>
        /// 签名
        /// </summary>
        [XmlElement("sign")]
        public string Sign { get; set; }
        /// <summary>
        /// 商品或支付单简要描述
        /// </summary>
        [XmlElement("body")]
        public string Body { get; set; }
        /// <summary>
        /// 商品名称明细列表
        /// </summary>
        [XmlElement("detail")]
        public string Detail { get; set; }
        /// <summary>
        /// 附加数据，在查询API和支付通知中原样返回，该字段主要用于商户携带订单的自定义数据
        /// </summary>
        [XmlElement("attach")]
        public string Attach { get; set; }
        /// <summary>
        /// 商户系统内部的订单号,32个字符内、可包含字母
        /// </summary>
        [XmlElement("out_trade_no")]
        public string OutTradeNo { get; set; }
        /// <summary>
        /// 货币类型, 符合ISO 4217标准的三位字母代码，默认人民币：CNY
        /// </summary>
        [XmlElement("fee_type")]
        public FeeType FeeType { get; set; }
        /// <summary>
        /// 总金额,订单总金额，单位为分
        /// </summary>
        [XmlElement("total_fee")]
        public int TotalFee { get; set; }
        /// <summary>
        /// 终端IP,APP和网页支付提交用户端ip，Native支付填调用微信支付API的机器IP。
        /// </summary>
        [XmlElement("spbill_create_ip")]
        public string BillCreatedIp { get; set; }
        /// <summary>
        /// 交易起始时间, 订单生成时间，格式为yyyyMMddHHmmss，如2009年12月25日9点10分10秒表示为20091225091010。其他详见时间规则
        /// </summary>
        [XmlElement("time_start")]
        public DateTime TimeStart { get; set; }
        /// <summary>
        /// 交易起始时间, 订单生成时间，格式为yyyyMMddHHmmss，如2009年12月25日9点10分10秒表示为20091225091010。其他详见时间规则
        /// </summary>
        [XmlElement("time_expire")]
        public DateTime TimeExpire { get; set; }
        /// <summary>
        /// 商品标记, 商品标记，代金券或立减优惠功能的参数
        /// </summary>
        [XmlElement("goods_tag")]
        public string GoodsTag { get; set; }
        /// <summary>
        /// 商品标记, 接收微信支付异步通知回调地址，通知url必须为直接可访问的url，不能携带参数
        /// </summary>
        [XmlElement("notify_url")]
        public string NotifyUrl { get; set; }
        /// <summary>
        /// 取值如下：JSAPI，NATIVE，APP，
        /// </summary>
        [XmlElement("trade_type")]
        public TradeType TradeType { get; set; }
        /// <summary>
        /// 商品ID,trade_type=NATIVE，此参数必传。此id为二维码中包含的商品ID，商户自行定义。
        /// </summary>
        [XmlElement("product_id")]
        public string ProductId { get; set; }
        /// <summary>
        /// 指定支付方式,no_credit--指定不能使用信用卡支付
        /// </summary>
        [XmlElement("limit_pay")]
        public string LimitPay { get; set; }
        /// <summary>
        /// 用户标识, trade_type=JSAPI，此参数必传，用户在商户appid下的唯一标识。openid如何获取
        /// </summary>
        [XmlElement("openid")]
        public string OpenId { get; set; }
    }

    /*
    NOAUTH	商户无此接口权限	商户未开通此接口权限	请商户前往申请此接口权限
NOTENOUGH	余额不足	用户帐号余额不足	用户帐号余额不足，请用户充值或更换支付卡后再支付
ORDERPAID	商户订单已支付	商户订单已支付，无需重复操作	商户订单已支付，无需更多操作
ORDERCLOSED	订单已关闭	当前订单已关闭，无法支付	当前订单已关闭，请重新下单
SYSTEMERROR	系统错误	系统超时	系统异常，请用相同参数重新调用
APPID_NOT_EXIST	APPID不存在	参数中缺少APPID	请检查APPID是否正确
MCHID_NOT_EXIST	MCHID不存在	参数中缺少MCHID	请检查MCHID是否正确
APPID_MCHID_NOT_MATCH	appid和mch_id不匹配	appid和mch_id不匹配	请确认appid和mch_id是否匹配
LACK_PARAMS	缺少参数	缺少必要的请求参数	请检查参数是否齐全
OUT_TRADE_NO_USED	商户订单号重复	同一笔交易不能多次提交	请核实商户订单号是否重复提交
SIGNERROR	签名错误	参数签名结果不正确	请检查签名参数和方法是否都符合签名算法要求
XML_FORMAT_ERROR	XML格式错误	XML格式错误	请检查XML参数格式是否正确
REQUIRE_POST_METHOD	请使用post方法	未使用post传递参数 	请检查请求参数是否通过post方法提交
POST_DATA_EMPTY	post数据为空	post数据不能为空	请检查post数据是否为空
NOT_UTF8	编码格式错误	未使用指定编码格式	请使用NOT_UTF8编码格式
*/
}
