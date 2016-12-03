using System;
using Newtonsoft.Json;

namespace Moonlit.Weixin.JsonConverters
{
    internal class SecondToTimespanConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {

        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return TimeSpan.FromSeconds(Convert.ToInt64(reader.Value));
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(TimeSpan);
        }
    }
}