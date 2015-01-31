using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace Moonlit.Media {
    /// <summary>
    /// 
    /// </summary>
    public class SrtSubtitle : ISubtitle {
        #region ISubtitle Members

        /// <summary>
        /// Builds the specified filename.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <returns></returns>
        public SubtitleCollection Build(string filename) {
            using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read)) {
                StreamReader reader = new StreamReader(fs,  Encoding.Default, true);
                SubtitleCollection subs = new SubtitleCollection();
                SrtSubtitleItem current = new SrtSubtitleItem();
                do {
                    string text = reader.ReadLine();
                    current.Load(text);
                    if (current.Complected) {
                        subs.Add(current);
                        current = new SrtSubtitleItem();
                    }
                } while (!reader.EndOfStream);
                if (!current.Empty) {
                    subs.Add(current);
                }
                return subs;
            }
        }
        /// <summary>
        /// Toes the string.
        /// </summary>
        /// <param name="ts">The ts.</param>
        /// <returns></returns>
        static string ToString(TimeSpan ts) {
            return string.Format("{0}:{1}:{2},{3}", 
                ts.Hours.ToString().PadLeft(2, '0'),
                ts.Minutes.ToString().PadLeft(2, '0'),
                ts.Seconds.ToString().PadLeft(2, '0'),
                ts.Milliseconds.ToString().PadLeft(3, '0'));
        }
        /// <summary>
        /// Writes the specified filename.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="subs">The subs.</param>
        public void Write(string filename, SubtitleCollection subs) {

            using (FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.Write)) {
                StreamWriter writer = new StreamWriter(fs, System.Text.Encoding.Default);

                foreach (SubtitleItem subtitle in subs) {
                    writer.WriteLine(subtitle.Order);
                    writer.WriteLine("{0} --> {1}", ToString(subtitle.Start), ToString(subtitle.End));
                    writer.WriteLine(subtitle.Content);
                    writer.WriteLine();
                }
                writer.Flush();
                fs.Flush();
            }
        }
        #endregion

        #region SrtSubtitle
        /// <summary>
        /// 
        /// </summary>
        class SrtSubtitleItem : SubtitleItem {
            /// <summary>
            /// Initializes a new instance of the <see cref="SrtSubtitleItem"/> class.
            /// </summary>
            internal SrtSubtitleItem() {
                _currentStatus = Status.ReadOrder;
                Complected = false;
            }
            public bool Empty {
                get { return _currentStatus == Status.ReadOrder; }
            }
            public bool Complected { get; set; }
            Regex Regex_TimeFormat = new Regex(@"(?<start1>\d\d:\d\d:\d\d),(?<start2>\d\d\d)\s*-->\s*(?<end1>\d\d:\d\d:\d\d),(?<end2>\d\d\d)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            enum Status {
                ReadOrder, ReadTime, ReadContext
            }
            Status _currentStatus = Status.ReadOrder;
            internal void Load(string text) {
                if (string.IsNullOrEmpty(text)) {
                    if (this._currentStatus == Status.ReadContext) {
                        this.Complected = true;
                    }
                    return;
                }
                switch (_currentStatus) {
                    case Status.ReadOrder:
                        this.Order = Convert.ToInt32(text);
                        _currentStatus = Status.ReadTime;
                        break;
                    case Status.ReadTime:
                        Match match = Regex_TimeFormat.Match(text);
                        if (!match.Success) {
                            return;
                        }

                        this.Start = TimeSpan.Parse(match.Groups["start1"].Value).Add(TimeSpan.FromMilliseconds(Convert.ToDouble(match.Groups["start2"].Value)));
                        this.End = TimeSpan.Parse(match.Groups["end1"].Value).Add(TimeSpan.FromMilliseconds(Convert.ToDouble(match.Groups["end2"].Value)));
                        _currentStatus = Status.ReadContext;
                        break;
                    case Status.ReadContext:
                        this.Content = string.IsNullOrEmpty(this.Content) ? text : Content + Environment.NewLine + text;
                        break;
                    default:
                        break;
                }
            }

        }
        #endregion
    }
}
