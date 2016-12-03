using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Xml.Linq;
using System.Xml.Serialization;
using log4net;
using Moonlit.Collections;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Moonlit.Weixin
{
    public abstract class PaymentObject
    { 
        public JObject ToJson(string key)
        {
            var xmlSerialier = new XmlSerializer(GetType());
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms, Encoding.UTF8);
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();

            ns.Add("", "");
            xmlSerialier.Serialize(writer, this, ns);
            ms.Position = 0;
            StreamReader reader = new StreamReader(ms, Encoding.UTF8);
            //var text = reader.ReadToEnd();
            var xele = XElement.Load(reader);
            Serialize(xele);

            Sign(key, xele);
            JObject jobj = new JObject();
            foreach (var child in xele.Elements())
            {
                jobj[child.Name.ToString()] = child.Value;
            }
            return jobj;
        }

        protected virtual void Sign(string key, XElement xele)
        {
            xele.Element("sign")?.Remove();
            xele.Add(new XElement("sign", DoSign(xele, key)));
        }

        public string ToXml(string key)
        {
            var xmlSerialier = new XmlSerializer(GetType());
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms, Encoding.UTF8);
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();

            ns.Add("", "");
            xmlSerialier.Serialize(writer, this, ns);
            ms.Position = 0;
            StreamReader reader = new StreamReader(ms, Encoding.UTF8);
            //var text = reader.ReadToEnd();
            var xele = XElement.Load(reader);
            Serialize(xele);

            xele.Element("sign")?.Remove();
            xele.Add(new XElement("sign", DoSign(xele, key)));
            //格式为yyyyMMddHHmmss
            return xele.ToString(SaveOptions.None);
        }

        protected virtual void Serialize(XElement xelement) { }

        protected string DoSign(XElement xelement, string key)
        {
            var keyValues = xelement.Elements()
                .OfType<XElement>()
                .Where(x => !string.Equals(x.Name.ToString(), "sign", StringComparison.OrdinalIgnoreCase))
                .OrderBy(x => x.Name.ToString()).Union(new[] { new XElement("key", key) });
            var data = string.Join("&", keyValues.Select(x =>
             {
                 var value = x.Value;
                 //var value = HttpUtility.UrlEncode(x.Value);
                 return $"{x.Name}={value}";
             }));
            var sign = DoSign(data);
            LogManager.GetLogger(this.GetType()).Debug($"signin data : {data}");
            LogManager.GetLogger(this.GetType()).Debug($"signin value : {sign}");
            return sign;
        }

        public static string DoSign(string value)
        {
            return MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(value)).ToHexString().ToUpper();
        }

        public static T Deserialize<T>(string text, string key) where T :  IDeseriableWeixinObject, new()
        {
            var data = WeixinData.LoadFromXml(text);
            if (typeof(ISingable).IsAssignableFrom(typeof(T)))
            {
                if (!data.VerifySign(key))
                {
                    return default(T);
                }
            }
            T t = new T();
            t.DeserializeObject(data);
            return t;
        }

      
    }

    public interface ISingable
    {
    }

    public interface IDeseriableWeixinObject
    {
          void DeserializeObject(WeixinData data);

    }
}