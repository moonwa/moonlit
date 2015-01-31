using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moonlit.Media.Mpeg
{
    /// <summary>
    /// 
    /// </summary>
    public class MpegID3V1
    {
        private string _identify;            // TAG，三个字节        0
        private string _title;               // 歌曲名,30个字节      3
        private string _artist;              // 歌手名,30个字节      33
        private string _album;               // 所属唱片,30个字节    63
        private string _year;                // 年,4个字符           93
        private string _comment;             // 注释,30个字节        97
        private MpegGenre genre;             // 流派，一个字节       127

        /// <summary>
        /// Trims the specified STR.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns></returns>
        public static string Trim(string str)
        {
            return str.Trim(new char[] { '\0', ' ', '\t' });
        }
        /// <summary>
        /// 标识
        /// </summary>
        public string Identify
        {
            get
            {
                return this._identify;
            }
            set
            {
                this._identify = value.Substring(0, Math.Min(value.Length, 30));
            }
        }
        /// <summary>
        /// 专辑
        /// </summary>
        public string Album
        {
            get
            {
                return this._album;
            }
            set
            {
                this._album = value.Substring(0, Math.Min(value.Length, 30));
            }
        }
        /// <summary>
        /// 歌手
        /// </summary>
        public string Artist
        {
            get
            {
                return this._artist;
            }
            set
            {
                this._artist = value.Substring(0, Math.Min(value.Length, 30));
            }
        }

        /// <summary>
        /// 注释
        /// </summary>
        public string Comment
        {
            get
            {
                return this._comment;
            }
            set
            {
                this._comment = value.Substring(0, Math.Min(value.Length, 30));
            }
        }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title
        {
            get
            {
                return this._title;
            }
            set
            {
                this._title = value.Substring(0, Math.Min(value.Length, 30));

            }
        }
        /// <summary>
        /// 年代
        /// </summary>
        public string Year
        {
            get
            {
                return this._year;
            }
            set
            {
                //if (MpegID3V1.Trim(value) != "")
                //    if (int.Parse(value) < 0)
                //        throw new ArgumentOutOfRangeException("年代不可为负数");
                this._year = value.Substring(0, Math.Min(value.Length, 4));
            }
        }
        /// <summary>
        /// 流派
        /// </summary>
        public MpegGenre Genre
        {
            get
            {
                return this.genre;
            }
            set
            {
                this.genre = value;
            }
        }
    }
}
