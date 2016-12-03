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
    /// ֧������
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
    /// ͳһ�µ��ӿ�
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
        /// ΢�ŷ���Ĺ����˺�ID����ҵ��corpid��Ϊ��appId��
        /// </summary>
        [XmlElement("appid")]
        public string AppId { get; set; }
        /// <summary>
        /// ΢��֧��������̻���
        /// </summary>
        [XmlElement("mch_id")]
        public string MerchantName { get; set; }
        /// <summary>
        /// �ն��豸��(�ŵ�Ż������豸ID)��ע�⣺PC��ҳ���ں���֧���봫"WEB"
        /// </summary>
        [XmlElement("device_info")]
        public string DeviceInfo { get; set; }
        /// <summary>
        /// ǩ��
        /// </summary>
        [XmlElement("sign")]
        public string Sign { get; set; }
        /// <summary>
        /// ��Ʒ��֧������Ҫ����
        /// </summary>
        [XmlElement("body")]
        public string Body { get; set; }
        /// <summary>
        /// ��Ʒ������ϸ�б�
        /// </summary>
        [XmlElement("detail")]
        public string Detail { get; set; }
        /// <summary>
        /// �������ݣ��ڲ�ѯAPI��֧��֪ͨ��ԭ�����أ����ֶ���Ҫ�����̻�Я���������Զ�������
        /// </summary>
        [XmlElement("attach")]
        public string Attach { get; set; }
        /// <summary>
        /// �̻�ϵͳ�ڲ��Ķ�����,32���ַ��ڡ��ɰ�����ĸ
        /// </summary>
        [XmlElement("out_trade_no")]
        public string OutTradeNo { get; set; }
        /// <summary>
        /// ��������, ����ISO 4217��׼����λ��ĸ���룬Ĭ������ң�CNY
        /// </summary>
        [XmlElement("fee_type")]
        public FeeType FeeType { get; set; }
        /// <summary>
        /// �ܽ��,�����ܽ���λΪ��
        /// </summary>
        [XmlElement("total_fee")]
        public int TotalFee { get; set; }
        /// <summary>
        /// �ն�IP,APP����ҳ֧���ύ�û���ip��Native֧�������΢��֧��API�Ļ���IP��
        /// </summary>
        [XmlElement("spbill_create_ip")]
        public string BillCreatedIp { get; set; }
        /// <summary>
        /// ������ʼʱ��, ��������ʱ�䣬��ʽΪyyyyMMddHHmmss����2009��12��25��9��10��10���ʾΪ20091225091010���������ʱ�����
        /// </summary>
        [XmlElement("time_start")]
        public DateTime TimeStart { get; set; }
        /// <summary>
        /// ������ʼʱ��, ��������ʱ�䣬��ʽΪyyyyMMddHHmmss����2009��12��25��9��10��10���ʾΪ20091225091010���������ʱ�����
        /// </summary>
        [XmlElement("time_expire")]
        public DateTime TimeExpire { get; set; }
        /// <summary>
        /// ��Ʒ���, ��Ʒ��ǣ�����ȯ�������Żݹ��ܵĲ���
        /// </summary>
        [XmlElement("goods_tag")]
        public string GoodsTag { get; set; }
        /// <summary>
        /// ��Ʒ���, ����΢��֧���첽֪ͨ�ص���ַ��֪ͨurl����Ϊֱ�ӿɷ��ʵ�url������Я������
        /// </summary>
        [XmlElement("notify_url")]
        public string NotifyUrl { get; set; }
        /// <summary>
        /// ȡֵ���£�JSAPI��NATIVE��APP��
        /// </summary>
        [XmlElement("trade_type")]
        public TradeType TradeType { get; set; }
        /// <summary>
        /// ��ƷID,trade_type=NATIVE���˲����ش�����idΪ��ά���а�������ƷID���̻����ж��塣
        /// </summary>
        [XmlElement("product_id")]
        public string ProductId { get; set; }
        /// <summary>
        /// ָ��֧����ʽ,no_credit--ָ������ʹ�����ÿ�֧��
        /// </summary>
        [XmlElement("limit_pay")]
        public string LimitPay { get; set; }
        /// <summary>
        /// �û���ʶ, trade_type=JSAPI���˲����ش����û����̻�appid�µ�Ψһ��ʶ��openid��λ�ȡ
        /// </summary>
        [XmlElement("openid")]
        public string OpenId { get; set; }
    }

    /*
    NOAUTH	�̻��޴˽ӿ�Ȩ��	�̻�δ��ͨ�˽ӿ�Ȩ��	���̻�ǰ������˽ӿ�Ȩ��
NOTENOUGH	����	�û��ʺ�����	�û��ʺ����㣬���û���ֵ�����֧��������֧��
ORDERPAID	�̻�������֧��	�̻�������֧���������ظ�����	�̻�������֧��������������
ORDERCLOSED	�����ѹر�	��ǰ�����ѹرգ��޷�֧��	��ǰ�����ѹرգ��������µ�
SYSTEMERROR	ϵͳ����	ϵͳ��ʱ	ϵͳ�쳣��������ͬ�������µ���
APPID_NOT_EXIST	APPID������	������ȱ��APPID	����APPID�Ƿ���ȷ
MCHID_NOT_EXIST	MCHID������	������ȱ��MCHID	����MCHID�Ƿ���ȷ
APPID_MCHID_NOT_MATCH	appid��mch_id��ƥ��	appid��mch_id��ƥ��	��ȷ��appid��mch_id�Ƿ�ƥ��
LACK_PARAMS	ȱ�ٲ���	ȱ�ٱ�Ҫ���������	��������Ƿ���ȫ
OUT_TRADE_NO_USED	�̻��������ظ�	ͬһ�ʽ��ײ��ܶ���ύ	���ʵ�̻��������Ƿ��ظ��ύ
SIGNERROR	ǩ������	����ǩ���������ȷ	����ǩ�������ͷ����Ƿ񶼷���ǩ���㷨Ҫ��
XML_FORMAT_ERROR	XML��ʽ����	XML��ʽ����	����XML������ʽ�Ƿ���ȷ
REQUIRE_POST_METHOD	��ʹ��post����	δʹ��post���ݲ��� 	������������Ƿ�ͨ��post�����ύ
POST_DATA_EMPTY	post����Ϊ��	post���ݲ���Ϊ��	����post�����Ƿ�Ϊ��
NOT_UTF8	�����ʽ����	δʹ��ָ�������ʽ	��ʹ��NOT_UTF8�����ʽ
*/
}
