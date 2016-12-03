using Newtonsoft.Json;

namespace Moonlit.Weixin
{
    /// <summary>
    /// the button of the pic_photo_or_album
    ///  {
    ///     "type": "pic_photo_or_album", 
    ///     "name": "拍照或者相册发图", 
    ///     "key": "rselfmenu_1_1", 
    ///     "sub_button": [ ]
    /// }
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class PhotoOrAlbumButton : IMenuItem
    {
        [JsonProperty("type")]
        public string Type { get { return "pic_photo_or_album"; } }
        [JsonProperty("name")]
        public string Text { get; set; }
        [JsonProperty("key")]
        public string Key { get; set; }

        public PhotoOrAlbumButton(string key, string text)
        {
            Key = key;
            Text = text;
        }
    }
}