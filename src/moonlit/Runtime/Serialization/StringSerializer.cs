using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;

namespace Moonlit.Runtime.Serialization
{
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