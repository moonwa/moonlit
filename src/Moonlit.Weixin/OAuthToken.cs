using System;
using Moonlit.Weixin.JsonConverters;
using Newtonsoft.Json;

namespace Moonlit.Weixin
{
    /*
    {
       "access_token":"ACCESS_TOKEN",
       "expires_in":7200,
       "refresh_token":"REFRESH_TOKEN",
       "openid":"OPENID",
       "scope":"SCOPE",
       "unionid": "o6_bmasdasdsad6_2sgVt7hMZOPfL"
    }
    */
    [JsonObject(MemberSerialization.OptIn)]
    public class OAuthToken
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }
        [JsonProperty("openid")]
        public string OpenId { get; set; }
        [JsonProperty("scope")]
        public string Scope { get; set; }
        [JsonProperty("unionid")]
        public string UnionId { get; set; }

        [JsonProperty("expires_in")]
        [JsonConverter(typeof(SecondToTimespanConverter))]
        public TimeSpan ExpiresIn { get; set; }

    }
}