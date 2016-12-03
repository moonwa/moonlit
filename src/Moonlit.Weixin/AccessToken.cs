using System;
using System.Diagnostics;
using Moonlit.Weixin.JsonConverters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace Moonlit.Weixin
{
    [DebuggerStepThrough]
    [JsonObject(MemberSerialization.OptIn)]
    public class AccessToken
    {
        private DateTime _birthTime;
        [JsonProperty("access_token")]
        public string Token { get; set; }
        [JsonProperty("expires_in")]
        [JsonConverter(typeof(SecondToTimespanConverter))]
        public TimeSpan ExpireIn { get; set; }

        public override string ToString()
        {
            return Token;
        }

        public AccessToken()
        {
            _birthTime = DateTime.Now;
        }
        public DateTime ExpireTime { get { return _birthTime.Add(ExpireIn); } }

        public bool IsExpired()
        {
            return ExpireTime < DateTime.Now.AddMinutes(-1);
        }

        // {"access_token":"ACCESS_TOKEN","expires_in":7200}
    }
    [DebuggerStepThrough]
    [JsonObject(MemberSerialization.OptIn)]
    public class JsapiTicket
    {
        private DateTime _birthTime;
        [JsonProperty("ticket")]
        public string Ticket { get; set; }
        [JsonProperty("expires_in")]
        [JsonConverter(typeof(SecondToTimespanConverter))]
        public TimeSpan ExpireIn { get; set; }

        public override string ToString()
        {
            return Ticket;
        }

        public JsapiTicket()
        {
            _birthTime = DateTime.Now;
        }
        public DateTime ExpireTime { get { return _birthTime.Add(ExpireIn); } }

        public bool IsExpired()
        {
            return ExpireTime < DateTime.Now.AddMinutes(-1);
        }

        // {"access_token":"ACCESS_TOKEN","expires_in":7200}
    }
}