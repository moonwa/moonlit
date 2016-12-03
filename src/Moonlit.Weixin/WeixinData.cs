using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;

namespace Moonlit.Weixin
{
    public class WeixinData
    {
        Dictionary<string, string> _data = new Dictionary<string, string>();
        public string this[string index]
        {
            get
            {
                string s;
                if (_data.TryGetValue(index, out s))
                {
                    return s;
                }
                return default(string);
            }
            set { _data[index] = value; }
        }

        public string SignJsapi()
        {
            var fields = CombineFields();
            var sha1 = SHA1.Create();
            var bytes = sha1.ComputeHash(Encoding.UTF8.GetBytes(fields));
            return bytes.ToHexString().ToLower();
        }

        private string CombineFields()
        {
            return string.Join("&", _data.Keys.OrderBy(x => x).Select(k => $"{k}={_data[k]}"));
        }

        public T GetEnum<T>(string key, T defaultValue) where T : struct
        {
            if (this[key] == null)
            {
                return defaultValue;
            }
            T t;
            if (Enum.TryParse(this[key], true, out t))
            {
                return t;
            }
            return defaultValue;
        }
        public bool GetBoolean(string key, bool defaultValue)
        {
            if (this[key] == null)
            {
                return defaultValue;
            }
            bool t;
            if (bool.TryParse(this[key], out t))
            {
                return t;
            }
            return defaultValue;
        }

        public int GetInt32(string key, int defaultValue)
        {
            if (this[key] == null)
            {
                return defaultValue;
            }
            int t;
            if (int.TryParse(this[key], out t))
            {
                return t;
            }
            return defaultValue;
        }

        public DateTime GetDateTime(string key, DateTime defaultValue)
        {
            if (this[key] == null)
            {
                return defaultValue;
            }
            DateTime t;
            var s = this[key];
            if (DateTime.TryParse($"{s.Substring(0, 4)}-{s.Substring(4, 2)}-{s.Substring(6, 2)} {s.Substring(8, 2)}:{s.Substring(10, 2)}:{s.Substring(12, 2)}", out t))
            {
                return t;
            }
            return defaultValue;
        }

        public static WeixinData LoadFromXml(string text)
        {
            WeixinData data = new WeixinData();

            XElement xele = XElement.Parse(text);
            foreach (var ele in xele.Elements())
            {
                data[ele.Name.ToString()] = ele.Value;
            }
            return data;
        }

        public bool VerifySign(string key)
        {
            return string.Equals(Sign(key), this._data["sign"], StringComparison.OrdinalIgnoreCase);
        }

        public string Sign(string key)
        {
            var keyValues = _data.Where(x => !x.Key.EqualsIgnoreCase("sign"))
                .OrderBy(x => x.Key).Union(new[] {new KeyValuePair<string, string>("key", key),});
            var data = string.Join("&", keyValues.Select(x =>
            {
                var value = x.Value;
                //var value = HttpUtility.UrlEncode(x.Value);
                return $"{x.Key}={value}";
            }));

            var sign = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(data)).ToHexString().ToUpper();
            return sign;
        }
    }
}