using Newtonsoft.Json;

namespace Moonlit.Weixin
{
    /// <summary>
    /// the view of the menu
    /// {	"type":"click", "name":"ΩÒ»’∏Ë«˙", "url":"http://www.soso.com/" }
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class ViewButton : IMenuItem
    {
        [JsonProperty("type")]
        public string Type { get { return "view"; } }
        [JsonProperty("name")]
        public string Text { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }

        public ViewButton(string url, string text)
        {
            Url = url;
            Text = text;
        }
    }
    /// <summary>
    ///    "type": "media_id",  "name": "Õº∆¨",  "media_id": "MEDIA_ID1"
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class MediaButton : IMenuItem
    {
        [JsonProperty("type")]
        public string Type { get { return "media_id"; } }
        [JsonProperty("name")]
        public string Text { get; set; }
        [JsonProperty("media_id")]
        public string MediaId { get; set; }

        public MediaButton(string mediaId, string text)
        {
            MediaId = mediaId;
            Text = text;
        }
    }
}