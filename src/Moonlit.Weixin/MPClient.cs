using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using log4net;
using Moonlit.Validations;
using Newtonsoft.Json;

namespace Moonlit.Weixin
{
    public class MPClient
    {
        private readonly string _appId;
        private readonly string _secret;
        private readonly string _token;
        public WeixinProxy ProxyApiHttps { get; set; }
        public WeixinProxy ProxyApiHttp { get; set; }
        public string AppId { get { return _appId; } }

        public string Secret
        {
            get { return _secret; }
        }

        public string Token
        {
            get { return _token; }
        }

        private static ILog Log = LogManager.GetLogger(typeof(MPClient));
        public MPClient(string appId, string secret, string token)
        {
            Log.Debug($"MPClient created: #{appId}");
            _appId = appId;
            _secret = secret;
            _token = token;
            ProxyApiHttps = new WeixinProxy(new WebClientWeixinProxy("https://api.weixin.qq.com"));
            ProxyApiHttp = new WeixinProxy(new WebClientWeixinProxy("http://api.weixin.qq.com"));
        }
        private AccessToken _accessToken;
        private JsapiTicket _jsapiTicket;
        public async Task<AccessToken> RequiredAccessTokenAsync(bool force = false)
        {
            if (_accessToken != null && !_accessToken.IsExpired() && !force)
            {
                return _accessToken;
            }

            // https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=APPID&secret=APPSECRET
            string url = $"cgi-bin/token?grant_type=client_credential&appid={_appId}&secret={_secret}";
            _accessToken = await ProxyApiHttps.GetAsync<AccessToken>(url);
            return _accessToken;
        }
        public async Task<JsapiTicket> RequiredJsapiTicketAsync(bool force = false)
        {
            if (_jsapiTicket != null && !_jsapiTicket.IsExpired() && !force)
            {
                return _jsapiTicket;
            }
            var accessToken = await RequiredAccessTokenAsync(force);
            string url = $"cgi-bin/ticket/getticket?access_token={accessToken.Token}&type=jsapi";
            _jsapiTicket = await ProxyApiHttps.GetAsync<JsapiTicket>(url);
            return _jsapiTicket;
        }


        public async Task<object> GetJsapiConfigAsync(string url, bool debug, string[] apiList)
        {
            var jsapiTicket = await RequiredJsapiTicketAsync();
            var nonceStr = Guid.NewGuid().ToString("N");
            var jsapi_ticket = jsapiTicket.Ticket;
            var timestamp = (int)(DateTime.Now - DateTime.Parse("1970-1-1")).TotalSeconds;
            WeixinData data = new WeixinData();
            data["noncestr"] = nonceStr;
            data["jsapi_ticket"] = jsapi_ticket;
            data["url"] = url;
            data["timestamp"] = timestamp.ToString();
            return new
            {
                debug = debug,
                appId = AppId,
                nonceStr = nonceStr,
                jsapi_ticket,
                url,
                timestamp,
                jsApiList = apiList,
                signature = data.SignJsapi(),
            };
        }
        public async Task DeleteMenuAsync()
        {
            await RequiredAccessTokenAsync();
            // https://api.weixin.qq.com/cgi-bin/menu/delete?access_token=ACCESS_TOKEN
            await ProxyApiHttps.GetAsync<object>($"cgi-bin/menu/delete?access_token={_accessToken}&appid={_appId}&secret={_secret}");
        }
        /// <summary>
        /// 获取图片
        /// </summary>
        /// <param name="mediaId"></param>
        /// <returns></returns>
        public async Task<byte[]> GetMediaAsync(string mediaId)
        {
            await RequiredAccessTokenAsync();
            // https://api.weixin.qq.com/cgi-bin/media/get?access_token=ACCESS_TOKEN&media_id=MEDIA_ID
            return await ProxyApiHttps.GetBytesAsync($"cgi-bin/media/get?access_token={_accessToken}&media_id={mediaId}&appid={_appId}&secret={_secret}");
        }
        /// <summary>
        /// 获取素材列表
        /// </summary>
        /// <returns></returns>
        public async Task<MaterialList<MaterialNews>> GetMaterialNewsListAsync(int offset, int count)
        {
            await RequiredAccessTokenAsync();
            // https://api.weixin.qq.com/cgi-bin/material/batchget_material?access_token=ACCESS_TOKEN
            return await ProxyApiHttps.PostAsJsonAsync<MaterialList<MaterialNews>>($"cgi-bin/material/batchget_material?access_token={_accessToken}", new { type = "news", offset = offset, count = count });
        }
        /// <summary>
        /// 获取素材列表
        /// </summary>
        /// <returns></returns>
        public async Task<MaterialList<MaterialNormal>> GetMaterialVideoListAsync(int offset, int count)
        {
            await RequiredAccessTokenAsync();
            // https://api.weixin.qq.com/cgi-bin/material/batchget_material?access_token=ACCESS_TOKEN
            return await ProxyApiHttps.PostAsJsonAsync<MaterialList<MaterialNormal>>($"cgi-bin/material/batchget_material?access_token={_accessToken}", new { type = "video", offset = offset, count = count });
        }
        /// <summary>
        /// 获取素材列表
        /// </summary>
        /// <returns></returns>
        public async Task<MaterialList<MaterialNormal>> GetMaterialVoiceListAsync(int offset, int count)
        {
            await RequiredAccessTokenAsync();
            // https://api.weixin.qq.com/cgi-bin/material/batchget_material?access_token=ACCESS_TOKEN
            return await ProxyApiHttps.PostAsJsonAsync<MaterialList<MaterialNormal>>($"cgi-bin/material/batchget_material?access_token={_accessToken}", new { type = "voice", offset = offset, count = count });
        }
        /// <summary>
        /// 获取素材列表
        /// </summary>
        /// <returns></returns>
        public async Task<MaterialList<MaterialNormal>> GetMaterialImageListAsync(int offset, int count)
        {
            await RequiredAccessTokenAsync();
            // https://api.weixin.qq.com/cgi-bin/material/batchget_material?access_token=ACCESS_TOKEN
            return await ProxyApiHttps.PostAsJsonAsync<MaterialList<MaterialNormal>>($"cgi-bin/material/batchget_material?access_token={_accessToken}", new { type = "image", offset = offset, count = count });
        }
        public async Task<IRequestMessage> ParseMessageAsync(HttpRequestBase request)
        {
            var streamReader = new StreamReader(request.GetBufferedInputStream());
            var text = await streamReader.ReadToEndAsync();
            return Parse(text);
        }
        private static ILog _logger = LogManager.GetLogger("mpclient");
        public static IRequestMessage Parse(string text)
        {
            _logger.Debug("parse message: " + text);
            var element = XElement.Parse(text);
            var msgType = (string)element.Element("MsgType");
            if (msgType == null)
            {
                return null;
            }
            IRequestMessage msg = null;
            switch (msgType.ToLower())
            {
                case "text":
                    msg = new TextMessage();
                    break;
                case "image":
                    msg = new ImageMessage();
                    break;
                case "event":
                    string @event = ((string)element.Element("Event")).ToLowerInvariant();
                    switch (@event)
                    {
                        case "pic_sysphoto":
                            msg = new PhotoEventMessage();
                            break;
                        case "pic_photo_or_album":
                            msg = new PhotoAlbumEventMessage();
                            break;
                        case "location":
                            msg = new LocationEventMessage();
                            break;

                        default:
                            return null;
                    }
                    break;
                default:
                    return null;
            }
            _logger.Debug("parsed message: " + msgType.ToLower() + ",messageType=" + msg.GetType().Name);
            msg.FromXml(element);
            return msg;
        }
        /// <summary>
        /// 验证签名
        /// </summary>
        /// <param name="signature">被验证的签名</param>
        /// <param name="timestamp"></param>
        /// <param name="nonce">挑战值</param>
        /// <returns></returns>
        public Task<bool> VerifySignatureAsync(string signature, string timestamp, string nonce)
        {
            //url=http://weixin.ecard.chihank.com/Weixin?signature=f217d10744b59ed96a2bd7eb5aa30c94133f9a80&echostr=5735577149604085215&timestamp=1445442902&nonce=1564121521,signature=f217d10744b59ed96a2bd7eb5aa30c94133f9a80,echostr=5735577149604085215,timestamp=1445442902,nonce=1564121521
            var keys = new[] { _token, timestamp, nonce }.OrderBy(x => x);
            var s = string.Join("", keys);
            SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();
            var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(s));
            var verifySignature = BitConverter.ToString(hash).Replace("-", "");
            var verified = string.Equals(verifySignature, signature, StringComparison.OrdinalIgnoreCase);
            return Task.FromResult(verified);
        }

        /// <summary>
        /// 获取用户授权TOKEN
        /// </summary>
        /// <param name="code"></param>
        /// <param name="oauthType"></param>
        /// <returns></returns>
        public async Task<OAuthToken> RefreshOAuthTokenAsync(OAuthToken authToken)
        {
            // https://api.weixin.qq.com/sns/oauth2/refresh_token?appid=APPID&grant_type=refresh_token&refresh_token=REFRESH_TOKEN
            string url = $"sns/oauth2/refresh_token?appid={_appId}&grant_type=refresh_token&refresh_token={authToken.RefreshToken}";
            return await ProxyApiHttps.GetAsync<OAuthToken>(url);
        }
        /// <summary>
        /// 获取用户授权TOKEN
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<OAuthToken> RequiredOAuthTokenAsync(string code)
        {
            // https://api.weixin.qq.com/sns/oauth2/access_token?appid=wxd8b64943ac261c4c&secret=6634394c895cce662ca469deda09c30d&code=031abff68d094c625d26ccd4d72b4bfw&grant_type=snsapi_base
            string url = $"sns/oauth2/access_token?appid={_appId}&secret={_secret}&code={code}&grant_type=authorization_code";
            return await ProxyApiHttps.GetAsync<OAuthToken>(url);
        }
        public string MakeOAuth(string returnUrl, WeiXinOAuthType authType, string state)
        {
            //https://open.weixin.qq.com/connect/oauth2/authorize?appid=APPID&redirect_uri=REDIRECT_URI&response_type=code&scope=SCOPE&state=STATE#wechat_redirect
            var encodingUrl = HttpUtility.UrlEncode(returnUrl);
            var url =
                $"https://open.weixin.qq.com/connect/oauth2/authorize" +
                $"?appid={_appId}&redirect_uri={encodingUrl}&response_type=code&scope=snsapi_{authType.ToString().ToLower()}&state={state}#wechat_redirect";
            Log.Debug($"generate oauth url ${url}");
            return url;
        }

        public async Task UpdateMenuAsync(WeixinMenu menu)
        {
            await RequiredAccessTokenAsync();
            await DeleteMenuAsync();
            // POST https://api.weixin.qq.com/cgi-bin/menu/create?access_token=ACCESS_TOKEN

            await ProxyApiHttps.PostAsJsonAsync<object>($"cgi-bin/menu/create?access_token={_accessToken}", menu);
            /*
             {
     "button":[
     {	
          "type":"click",
          "name":"今日歌曲",
          "key":"V1001_TODAY_MUSIC"
      },
      {
           "name":"菜单",
           "sub_button":[
           {	
               "type":"view",
               "name":"搜索",
               "url":"http://www.soso.com/"
            },
            {
               "type":"view",
               "name":"视频",
               "url":"http://v.qq.com/"
            },
            {
               "type":"click",
               "name":"赞一下我们",
               "key":"V1001_GOOD"
            }]
       }]
 }
 */
        }
    }

    public class MaterialList<T>
    {
        [JsonProperty("total_count")]
        public int TotalCount { get; set; }
        [JsonProperty("item_count")]
        public int ItemCount { get; set; }
        [JsonProperty("item")]
        public T[] Items { get; set; }
    }

    public class MaterialNewsContent
    {
        [JsonProperty("news_item")]
        public MaterialNewsItem[] Items { get; set; }
    }

    public class MaterialNewsItem
    {
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("thumb_media_id")]
        public string ThumbMediaId { get; set; }
        [JsonProperty("show_cover_pic")]
        public bool IsShowCoverPic{ get; set; }
        [JsonProperty("author")]
        public string Author { get; set; }
        [JsonProperty("digest")]
        public string Digest { get; set; }
        [JsonProperty("content")]
        public string Content { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("content_source_url")]
        public string ContentSourceUrl { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class MaterialNews
    {
        [JsonProperty("media_id")]
        public string MediaId { get; set; }
        [JsonProperty("content")]
        public MaterialNewsContent Content { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class MaterialNormal
    {
        [JsonProperty("media_id")]
        public string MediaId { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("update_time")]
        public string UpdateTime { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
    }
}
