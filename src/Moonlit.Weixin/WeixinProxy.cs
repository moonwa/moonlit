using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;
using System.Xml.Serialization;
using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Moonlit.Weixin
{
    public class WeixinProxy
    {
        private readonly IWeixinProxy _proxy;
        private WeixinJsonConverter _jsonConverter;

        public WeixinProxy(IWeixinProxy proxy)
        {
            _proxy = proxy;
            _jsonConverter = new WeixinJsonConverter();
        }

        public async Task<T> GetAsync<T>(string url)
        {
            var value = await _proxy.GetAsync(url);
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new Exception("not response from weixin server");
            }
            CheckJsonResponseError(value);
            return _jsonConverter.DeserializeObject<T>(value);
        }
        public async Task<byte[]> GetBytesAsync (string url)
        {
            var value = await _proxy.GetBytesAsync(url);
            //if (string.IsNullOrWhiteSpace(value))
            //{
            //    throw new Exception("not response from weixin server");
            //}
            //CheckJsonResponseError(value);
            //return _jsonConverter.DeserializeObject<T>(value);
            return value;
        }


        private static void CheckJsonResponseError(string value)
        {
            var jobj = JsonConvert.DeserializeObject(value) as JObject;
            if (jobj != null && jobj.Property("errcode") != null && (int)jobj["errcode"] != 0)
            {
                throw new WeixinException((int)jobj["errcode"], (string)jobj["errmsg"]);
            }
        }

        private static void CheckXmlResponseError(string value)
        {
            XElement xele = XElement.Parse(value);
            var returnCode = (string)xele.Element("return_code");
            if (!returnCode.EqualsIgnoreCase("success"))
            {
                throw new WeixinException(0, (string)xele.Element("return_msg"));
            }
        }


        public async Task<T> PostAsXmlAsync<T>(string url, string text)
        {

            var response = await _proxy.PostAsync(url, text);
            CheckXmlResponseError(response);

            var xmlSerialier = new XmlSerializer(typeof(T));
            return (T)xmlSerialier.Deserialize(new StringReader(response));
        }

        public async Task<T> PostAsJsonAsync<T>(string url, object data)
        {
            var response = await _proxy.PostAsync(url, _jsonConverter.SerializeObject(data));
            CheckJsonResponseError(response);
            return _jsonConverter.DeserializeObject<T>(response);
        }



    }
}