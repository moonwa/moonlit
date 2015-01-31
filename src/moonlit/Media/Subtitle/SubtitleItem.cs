using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moonlit.Media {
    /// <summary>
    /// 字幕基类
    /// </summary>
    public class SubtitleItem {
        /// <summary>
        /// Gets or sets the order.
        /// </summary>
        /// <value>The order.</value>
        public int Order { get; set; }
        /// <summary>
        /// Gets or sets the 开始时间.
        /// </summary>
        /// <value>The start.</value>
        public TimeSpan Start { get; set; }
        /// <summary>
        /// Gets or sets the 结束时间.
        /// </summary>
        /// <value>The end.</value>
        public TimeSpan End { get; set; }
        /// <summary>
        /// Gets or sets the 内容.
        /// </summary>
        /// <value>The content.</value>
        public string Content { get; set; }

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <param name="obj">The <see cref="T:System.Object"/> to compare with the current <see cref="T:System.Object"/>.</param>
        /// <returns>
        /// true if the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>; otherwise, false.
        /// </returns>
        /// <exception cref="T:System.NullReferenceException">The <paramref name="obj"/> parameter is null.</exception>
        public override bool Equals(object obj) {
            if (!(obj is SubtitleItem)) {
                return false;
            }
            SubtitleItem sub = obj as SubtitleItem;
            return this.Order == sub.Order && this.Start == sub.Start && sub.Content == this.Content && sub.End == this.End;
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="sub1">The sub1.</param>
        /// <param name="sub2">The sub2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(SubtitleItem sub1, SubtitleItem sub2) {
            if (object.ReferenceEquals(sub1, null) && object.ReferenceEquals(sub2, null)) return true;
            if (!object.ReferenceEquals(sub1, null) && !object.ReferenceEquals(sub2, null)) {
                return sub1.Equals(sub2);
            }
            return false;
        }
        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="sub1">The sub1.</param>
        /// <param name="sub2">The sub2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(SubtitleItem sub1, SubtitleItem sub2) {
            return !(sub1 == sub2);
        }
        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"/>.
        /// </returns>
        public override int GetHashCode() {
            return this.Order;
        }
    }
}
