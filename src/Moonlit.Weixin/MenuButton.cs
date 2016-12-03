using Newtonsoft.Json;

namespace Moonlit.Weixin
{
    /// <summary>
    /// the button of the menu
    /// {	"type":"click", "name":"今日歌曲", "key":"V1001_TODAY_MUSIC" }
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class MenuButton : IMenuItem
    {
        [JsonProperty("type")]
        public string Type { get { return "click"; } }
        [JsonProperty("name")]
        public string Text { get; set; }
        [JsonProperty("key")]
        public string Key { get; set; }

        public MenuButton(string key, string text)
        {
            Key = key;
            Text = text;
        }
    }
}