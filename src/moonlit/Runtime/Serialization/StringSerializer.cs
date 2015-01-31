using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;

namespace Moonlit.Runtime.Serialization
{
    public static class StringSerializer
    {
        private static readonly JavaScriptSerializer JavaScriptSerializer = new JavaScriptSerializer();
        private static DateTime StartTime = new DateTime(1970, 1, 1);
        static StringSerializer()
        {
            //Register(new DateTimeJavaScriptConverter());
        }

        private static void Register(JavaScriptConverter javaScriptConverter)
        {
            JavaScriptSerializer.RegisterConverters(new[] { javaScriptConverter });
        }

        public static string SerializeAsDataContractJson(this object obj)
        {
            if (obj == null) return "{}";
            var serializer = new DataContractJsonSerializer(obj.GetType());
            var ms = new MemoryStream();
            serializer.WriteObject(ms, obj);
            ms.Position = 0;
            return new StreamReader(ms, Encoding.UTF8).ReadToEnd();
        }

        public static object DeserializeAsDataContractJson(this string text, Type type)
        {
            text = text ?? "{}";
            text = text.Trim();
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(text);
            writer.Flush();
            ms.Position = 0;
            var serializer = new DataContractJsonSerializer(type);
            return serializer.ReadObject(ms);
        }

        public static T DeserializeAsDataContractJson<T>(this string text)
        {
            return (T)DeserializeAsDataContractJson(text, typeof(T));
        }

        public static string SerializeAsJson(this object obj)
        {
            if (obj == null) return "{}";
            return JavaScriptSerializer.Serialize(obj);
        }

        public static object DeserializeAsJson(this string text, Type type)
        {
            if (string.IsNullOrWhiteSpace(text))
                return Activator.CreateInstance(type);
            text = text.Trim();
            return JavaScriptSerializer.Deserialize(text, type);
        }
        public static object DeserializeAsJsonObject(this string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return null;
            text = text.Trim();
            // "\/Date(1359904941134)\/"
            foreach (Match match in Regex.Matches(text, @"new\s+Date\(['""](?<time>.+?)['""]\)"))
            {
                DateTime dt;
                if (DateTime.TryParse(match.Groups["time"].Value, out dt))
                {
                    var time = (long)(dt - StartTime).TotalMilliseconds;
                    text = text.Replace(match.Value, @"""\/Date(" + time + @")\/""");
                }
            }
            return JavaScriptSerializer.DeserializeObject(text);
        }

        public static T DeserializeAsJson<T>(this string text)
        {
            return (T)DeserializeAsJson(text, typeof(T));
        }
    }

    public class DateTimeJavaScriptConverter : JavaScriptConverter
    {
        public override object Deserialize(IDictionary<string, object> dictionary, Type type, JavaScriptSerializer serializer)
        {
            return null;
        }

        public override IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer)
        {
            return null;
        }

        public override IEnumerable<Type> SupportedTypes
        {
            get
            {
                yield return typeof(DateTime?);
                yield return typeof(DateTime);
                yield return typeof(string);
            }
        }
    }
}