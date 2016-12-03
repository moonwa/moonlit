using System;
using Newtonsoft.Json;

namespace Moonlit.Weixin.JsonConverters
{
    public static class JsonConverterExtensions
    {
        public static void WriteProperty(this JsonWriter writer, string name, object value, JsonSerializer jsonSerializer)
        {
            writer.WritePropertyName(name);
            jsonSerializer.Serialize(writer, value);
        }
        public static IDisposable WriteObjectScope(this JsonWriter writer)
        {
            writer.WriteStartObject();
            return new WriteObjectScopeHelper(writer);
        }

        public static IDisposable ReadObjectScope(this JsonReader reader)
        {
            if (reader.TokenType != JsonToken.StartObject)
            {
                throw new Exception("reader should is start object token");
            }
            reader.Read();
            return new ReadObjectScopeHelper(reader);
        }

        private class WriteObjectScopeHelper : IDisposable
        {
            private readonly JsonWriter _writer;

            public WriteObjectScopeHelper(JsonWriter writer)
            {
                _writer = writer;
            }

            public void Dispose()
            {
                _writer.WriteEndObject();
            }
        }
        private class ReadObjectScopeHelper : IDisposable
        {
            private readonly JsonReader _reader;

            public ReadObjectScopeHelper(JsonReader reader)
            {
                _reader = reader;
            }

            public void Dispose()
            {
                _reader.Read();
            }
        }
    }
}