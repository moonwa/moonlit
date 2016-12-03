using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Moonlit.Weixin
{
    [JsonObject(MemberSerialization.OptIn)]
    public class WeixinMenu
    {
        private readonly int _maxItemsCount;
        [JsonProperty("button")]
        public List<IMenuItem> Items { get; set; }

        public void AddMenuItem(IMenuItem menuItem)
        {
            if (Items.Count >= _maxItemsCount)
            {
                throw new Exception("the item count over the maxItemCount");
            }
            Items.Add(menuItem);
        }
        WeixinMenu(int maxItemsCount)
        {
            _maxItemsCount = maxItemsCount;
            Items = new List<IMenuItem>(maxItemsCount);
        }

        public static WeixinMenu Create()
        {
            return new WeixinMenu(3);
        }

    }
}