using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moonlit.WeiXin.Api
{
    public class Menu : Base
    {
        public Menu(string appId, string appsecret)
            : base(appId, appsecret)
        {

        }
        public void Query()
        {
            var token = base.EnsureToken();


        }
    }
    public class Base
    {
        public Base(string appId, string appsecret)
        {
            _appId = appId;
            _appsecret = appsecret;
        }
        protected async Task<string> EnsureToken()
        {
            System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();
            string result = await client.GetStringAsync("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + _appId + "&secret=" + this._appsecret);

            var rsp = ParseResult<TokenResponse>(result);
            return rsp.AccessToken;
        }

        private T1 ParseResult<T1>(string result)
        {
            ErrorResponse error = Newtonsoft.Json.JsonConvert.DeserializeObject<ErrorResponse>(result);
            if (error.ErrorCode != 0)
            {
                throw new Exception(error.ErrorMessage);
            }
            T1 t = Newtonsoft.Json.JsonConvert.DeserializeObject<T1>(result);
            return t;
        }
        private string _appId;
        private string _appsecret;
    }
    public class TokenResponse
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
        [JsonProperty("expires_in")]
        public int ExpiredSeconds { get; set; }
    }
    public class ErrorResponse
    {
        [JsonProperty("errcode")]
        public int ErrorCode { get; set; }
        [JsonProperty("errmsg")]
        public string ErrorMessage { get; set; }
    }
}
