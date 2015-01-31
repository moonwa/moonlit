using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moonlit.Media
{
    /// <summary>
    /// 字幕集合
    /// </summary>
    public class SubtitleCollection : List<SubtitleItem>
    {
        class OrderComparer : IComparer<SubtitleItem>
        {
            public OrderComparer()
            {
            }
            #region IComparer<Subtitle> Members

            public int Compare(SubtitleItem x, SubtitleItem y)
            {
                return x.Start.CompareTo(y.Start);
            }

            #endregion
        }
        
        /// <summary>
        /// Sorts the specified sort.
        /// </summary>
        public new void Sort()
        {
            this.Sort(new OrderComparer());
        }
    }
}
