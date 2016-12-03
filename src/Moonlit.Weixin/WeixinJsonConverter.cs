using System.Collections.Generic;
using System.Diagnostics;
using Newtonsoft.Json;

namespace Moonlit.Weixin
{
    [DebuggerStepThrough]
    internal class WeixinJsonConverter
    {

        private List<JsonConverter> _jsonConverters = new List<JsonConverter>();

        public WeixinJsonConverter()
        {
        }

        public string SerializeObject(object value)
        {
            return JsonConvert.SerializeObject(value, _jsonConverters.ToArray());
        }
        public T DeserializeObject<T>(string value)
        {
            return JsonConvert.DeserializeObject<T>(value, _jsonConverters.ToArray());
        }
    }
}