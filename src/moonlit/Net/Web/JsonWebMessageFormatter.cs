using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;

namespace Moonlit.Net.Web
{
    public class JsonWebMessageFormatter : IWebMessageFormatter
    {
        #region IWebMessageParser Members

        public string CreatePostData(IEnumerable<WebParameter> parameters)
        {
            var builder = new StringBuilder();

            bool first = true;
            foreach (WebParameter arg in parameters.Where(x => x.ParameterType == WebParameterType.PostData))
            {
                var ms = new MemoryStream();
                var writer = new StreamWriter(ms);
                writer.AutoFlush = true;
                if (first)
                {
                    first = false;
                }
                else
                {
                    builder.Append("&");
                }
                WritePostData(arg.Value, writer);
                byte[] b = ms.ToArray();

                string name = HttpUtility.UrlEncode(arg.Name);
                string value = Encoding.UTF8.GetString(b, 0, b.Length);
                value = HttpUtility.UrlEncode(value);
                builder.Append(name + "=" + value);
            }

            return builder.ToString();
        }

        public T Deserialize<T>(byte[] result, Encoding encoding)
        {
            if (result == null || result.Length == 0) return default(T);

            string s = encoding.GetString(result);
            var contractJsonSerializer = new DataContractJsonSerializer(typeof(T));
            using (var ms = new MemoryStream())
            {
                ms.Write(result, 0, result.Length);
                ms.Flush();
                ms.Position = 0;

                return (T)contractJsonSerializer.ReadObject(ms);
            }
        }

        #endregion

        private void WritePostData(object value, StreamWriter streamWriter)
        {
            if (value == null)
            {
                streamWriter.Write("");
                return;
            }
            Type type = value.GetType();

            var serializer = new DataContractJsonSerializer(type);
            if (type == typeof(DateTime))
            {
                var dt = (DateTime)value;
                value = DateTime.SpecifyKind(dt, DateTimeKind.Utc);
            }
            serializer.WriteObject(streamWriter.BaseStream, value);
        }
    }
}