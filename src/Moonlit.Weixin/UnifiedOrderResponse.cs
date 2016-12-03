using System;
using System.Xml.Serialization;

namespace Moonlit.Weixin
{
    [Serializable]
    [XmlRoot("xml")]
    public class UnifiedOrderResponse
    {
        /// <summary>
        /// 返回状态码, SUCCESS/FAIL 此字段是通信标识，非交易标识，交易是否成功需要查看result_code来判断
        /// </summary>
        [XmlElement("return_code")]
        public string ReturnCode { get; set; }
        /// <summary>
        /// 返回信息
        /// </summary>
        [XmlElement("return_msg")]
        public string ReturnMsg { get; set; }
        /* 以下字段在return_code为SUCCESS的时候有返回 */
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
        /// 随机字符串，不长于32位。
        /// </summary>
        [XmlElement("nonce_str")]
        public string Nonce { get; set; }
        /// <summary>
        /// 签名
        /// </summary>
        [XmlElement("sign")]
        public string Sign { get; set; }
        /// <summary>
        /// SUCCESS/FAIL
        /// </summary>
        [XmlElement("result_code")]
        public string ResultCode { get; set; }
        /// <summary>
        /// 错误代码
        /// </summary>
        [XmlElement("err_code")]
        public string ErrorCode { get; set; }
        /// <summary>
        /// 错误代码描述
        /// </summary>
        [XmlElement("err_code_des")]
        public string ErrorCodeDesc { get; set; }

        /* 以下字段在return_code 和result_code都为SUCCESS的时候有返回 */

        /// <summary>
        /// 交易类型, 调用接口提交的交易类型，取值如下：JSAPI，NATIVE，APP
        /// </summary>
        [XmlElement("trade_type")]
        public string TradeType { get; set; }
        /// <summary>
        /// 预支付交易会话标识, 微信生成的预支付回话标识，用于后续接口调用中使用，该值有效期为2小时
        /// </summary>
        [XmlElement("prepay_id")]
        public string PrepayId { get; set; }
        /// <summary>
        /// 二维码链接, trade_type为NATIVE是有返回，可将该参数值生成二维码展示出来进行扫码支付
        /// </summary>
        [XmlElement("code_url")]
        public string CodeUrl { get; set; }
    }
}