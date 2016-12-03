using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Moonlit.Weixin
{
    public class WeiXinXmlSerializer
    {
        public string Serialize(object message)
        {
            XmlSerializer serializer = new XmlSerializer(message.GetType());
            using (var ms = new MemoryStream())
            {
                StreamWriter writer = new StreamWriter(ms, Encoding.UTF8);
                var ns = new XmlSerializerNamespaces();
                ns.Add("", "");

                serializer.Serialize(writer, message, ns);
                writer.Flush();
                ms.Position = 0;
                StreamReader reader = new StreamReader(ms);
                reader.ReadLine();
                return reader.ReadToEnd();
            }
        }
    }
}