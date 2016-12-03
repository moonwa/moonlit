using System;
using System.Xml.Serialization;

namespace Moonlit.Weixin
{
    [Serializable]
    [XmlRoot("xml")]
    public class UnifiedOrderResponse
    {
        /// <summary>
        /// ����״̬��, SUCCESS/FAIL ���ֶ���ͨ�ű�ʶ���ǽ��ױ�ʶ�������Ƿ�ɹ���Ҫ�鿴result_code���ж�
        /// </summary>
        [XmlElement("return_code")]
        public string ReturnCode { get; set; }
        /// <summary>
        /// ������Ϣ
        /// </summary>
        [XmlElement("return_msg")]
        public string ReturnMsg { get; set; }
        /* �����ֶ���return_codeΪSUCCESS��ʱ���з��� */
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
        /// ����ַ�����������32λ��
        /// </summary>
        [XmlElement("nonce_str")]
        public string Nonce { get; set; }
        /// <summary>
        /// ǩ��
        /// </summary>
        [XmlElement("sign")]
        public string Sign { get; set; }
        /// <summary>
        /// SUCCESS/FAIL
        /// </summary>
        [XmlElement("result_code")]
        public string ResultCode { get; set; }
        /// <summary>
        /// �������
        /// </summary>
        [XmlElement("err_code")]
        public string ErrorCode { get; set; }
        /// <summary>
        /// �����������
        /// </summary>
        [XmlElement("err_code_des")]
        public string ErrorCodeDesc { get; set; }

        /* �����ֶ���return_code ��result_code��ΪSUCCESS��ʱ���з��� */

        /// <summary>
        /// ��������, ���ýӿ��ύ�Ľ������ͣ�ȡֵ���£�JSAPI��NATIVE��APP
        /// </summary>
        [XmlElement("trade_type")]
        public string TradeType { get; set; }
        /// <summary>
        /// Ԥ֧�����׻Ự��ʶ, ΢�����ɵ�Ԥ֧���ػ���ʶ�����ں����ӿڵ�����ʹ�ã���ֵ��Ч��Ϊ2Сʱ
        /// </summary>
        [XmlElement("prepay_id")]
        public string PrepayId { get; set; }
        /// <summary>
        /// ��ά������, trade_typeΪNATIVE���з��أ��ɽ��ò���ֵ���ɶ�ά��չʾ��������ɨ��֧��
        /// </summary>
        [XmlElement("code_url")]
        public string CodeUrl { get; set; }
    }
}