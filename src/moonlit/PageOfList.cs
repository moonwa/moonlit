using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Moonlit
{
    [DataContract]
    public class PageOfList<T> : IPageOfList<T>
    {
        public PageOfList()
        {
            Items = new List<T>();
        }
        public PageOfList<T> Clear()
        {
            Items.Clear();
            this.PageCount = 1;
            this.ItemCount = 0;
            return this;
        }
        public IPageOfList<T2> CloneTo<T2>(IPageOfList<T2> items, Func<T, T2> selector)
        {
            items.ItemCount = this.ItemCount;
            items.Items = this.Items.Select(x => selector(x)).ToList();
            items.PageIndex = this.PageIndex;
            items.PageSize = this.PageSize;
            items.PageCount = this.PageCount;
            return items;
        }
        internal static int GetPageCount(int totalItemCount, int pageSize)
        {
            if (pageSize == 0)
                return 1;
            return (int)Math.Ceiling(totalItemCount / (double)pageSize);
        }
        [DataMember]
        public int PageIndex { get; set; }
        [DataMember]
        public int PageSize { get; set; }
        [DataMember]
        public int PageCount { get; set; }
        [DataMember]
        public int ItemCount { get; set; }
        [DataMember]
        public string OrderBy { get; set; }

        [DataMember]
        public List<T> Items { get; set; }
    }
}