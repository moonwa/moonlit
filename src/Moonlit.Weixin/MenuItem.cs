using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Moonlit.Weixin
{
    /// <summary>
    /// the button of the menu
    /// </summary> 
    [JsonObject(MemberSerialization.OptIn)]
    public class MenuItem : IMenuItem
    {
        private readonly int _maxItemsCount;
        [JsonProperty("name")]
        public string Text { get; set; }
        [JsonProperty("sub_button")]
        public List<IMenuItem> Items { get; set; }

        public MenuItem(string text)
        {
            Text = text;
            _maxItemsCount = 5;
            Items = new List<IMenuItem>(_maxItemsCount);
        }

        public void AddMenuItem(IMenuItem menuItem)
        {
            if (Items.Count >= _maxItemsCount)
            {
                throw new Exception("the item count over the maxItemCount");
            }
            Items.Add(menuItem);
        }
    }
}