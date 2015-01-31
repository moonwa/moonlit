using System;
using System.Collections.Generic;
using System.Text;
using Moonlit.Runtime.Serialization;

namespace Moonlit.Web
{
    public class HtmlAttributeBuilder
    {
        Dictionary<string, object> _attrs = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
        public void Add(string key, object value)
        {
            _attrs.Add(key, value);
        }
        public override string ToString()
        {
            StringBuilder buffer = new StringBuilder();
            foreach (var attr in _attrs)
            {
                buffer.AppendFormat(" {0}='{1}' ", attr.Key, ToHtmlString(attr.Value));
            }
            return buffer.ToString();
        }

        public bool Has(string key)
        {
            return _attrs.ContainsKey(key);
        }
        public static string ToHtmlString(object value)
        {
            if (value == null) return "";
            var type = value.GetType();
            var typeCode = Type.GetTypeCode(type);
            switch (typeCode)
            {
                case TypeCode.Boolean:
                    return value.ToString().ToLowerInvariant();
                case TypeCode.Object:
                    return value.SerializeAsJson();
                case TypeCode.Char:
                case TypeCode.SByte:
                case TypeCode.Byte:
                case TypeCode.Int16:
                case TypeCode.UInt16:
                case TypeCode.Int32:
                case TypeCode.UInt32:
                case TypeCode.Int64:
                case TypeCode.UInt64:
                case TypeCode.Single:
                case TypeCode.Double:
                case TypeCode.Decimal:
                case TypeCode.DateTime:
                case TypeCode.String:
                case TypeCode.Empty:
                    return value.ToString();
                case TypeCode.DBNull:
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}