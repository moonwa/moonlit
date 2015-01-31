using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moonlit.Media {
    /// <summary>
    /// 
    /// </summary>
    public interface ISubtitle {
        /// <summary>
        /// Builds the specified filename.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <returns></returns>
        SubtitleCollection Build(string filename);
        /// <summary>
        /// Writes the specified filename.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="subs">The subs.</param>
        void Write(string filename, SubtitleCollection subs);
    }
}
