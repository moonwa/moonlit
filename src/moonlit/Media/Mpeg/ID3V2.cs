using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;
using Moonlit.IO; 

namespace Moonlit.Media.Mpeg
{
    /// <summary>
    /// 
    /// </summary>
    public class MpegID3V2
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MpegID3V2"/> class.
        /// </summary>
        public MpegID3V2()
        {
            this.Tags = new Dictionary<string, TagFrame>();
        }
        const string MagicHeader = "ID3";
        /// <summary>
        /// 相关帧信息
        /// </summary>
        public Dictionary<string, TagFrame> Tags { get; private set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enabled { get; private set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public byte Version { get; private set; }
        /// <summary>
        /// 副版本号
        /// </summary>
        public byte Revision { get; private set; }
        /// <summary>
        /// 标签大小
        /// </summary>
        public int Size { get; private set; }
        #region 标题
        const string Key_Title = "TIT2";
        /// <summary>
        /// 标题
        /// </summary>
        public string Title
        {
            get
            {
                if (this.Tags.ContainsKey(Key_Title))
                    return this.Tags[Key_Title].Value;
                return string.Empty;
            }
            set
            {
                if (!this.Tags.ContainsKey(Key_Title))
                    this.Tags[Key_Title] = new TagFrame() { ID = Key_Title };
                TagFrame tag = this.Tags[Key_Title];
                tag.Value = value;
            }
        }
        #endregion

        #region 歌手
        const string Key_Artist = "TPE1";
        /// <summary>
        /// 歌手
        /// </summary>
        public string Artist
        {
            get
            {
                if (this.Tags.ContainsKey(Key_Artist))
                    return this.Tags[Key_Artist].Value;
                return string.Empty;
            }
            set
            {
                if (!this.Tags.ContainsKey(Key_Artist))
                    this.Tags[Key_Artist] = new TagFrame() { ID = Key_Artist };
                TagFrame tag = this.Tags[Key_Artist];
                tag.Value = value;
            }
        }
        #endregion


        #region 专辑
        const string Key_Album = "TALB";
        /// <summary>
        /// 专辑
        /// </summary>
        public string Album
        {
            get
            {
                if (this.Tags.ContainsKey(Key_Album))
                    return this.Tags[Key_Album].Value;
                return string.Empty;
            }
            set
            {
                if (!this.Tags.ContainsKey(Key_Album))
                    this.Tags[Key_Album] = new TagFrame() { ID = Key_Album };
                TagFrame tag = this.Tags[Key_Album];
                tag.Value = value;
            }
        }
        #endregion


        /// <summary>
        /// 根据4个字节计算标签大小
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static int CalcSize(byte[] bytes)
        {
            Debug.Assert(bytes.Length == 4);
            return (bytes[0] & 0x7F) * 0x200000 + (bytes[1] & 0x7F) * 0x400 + (bytes[2] & 0x7F) * 0x80 + (bytes[3] & 0x7F);
        }
        /// <summary>
        /// Reads the specified SRC stream.
        /// </summary>
        /// <param name="srcStream">The SRC stream.</param>
        internal void Read(System.IO.Stream srcStream)
        {
            srcStream.Seek(0, SeekOrigin.Begin);
            byte[] bytes = new byte[10];
            srcStream.Read(bytes, 0, 10);
            if (!IsValidMagicHeader(bytes))
            {
                return;
            }

            this.Version = bytes[3];
            this.Revision = bytes[4];
            this.Size = CalcSize(bytes.Skip(6).ToArray());
            if (this.Size >= srcStream.Length || this.Size == 0)
            {
                this.Enabled = false;
                return;
            }
            bytes = new byte[this.Size - 10];
            if (srcStream.Length < this.Size)
            {
                throw new InvalidDataException("指定标签长度超过文件长度");
            }
            srcStream.Read(bytes, 0, bytes.Length);
            ReadID3V2(bytes);
        }

        /// <summary>
        /// Reads the I d3 v2.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        private void ReadID3V2(byte[] bytes)
        {
            int idx = 0;
            while (idx < bytes.Length)
            {
                byte[] header = bytes.Skip(idx).Take(10).ToArray();
                string id = Encoding.ASCII.GetString(header, 0, 4);
                int size = (int)(header[4] * 0x100000000 + header[5] * 0x10000 + header[6] * 0x100 + header[7]);
                TagFrameFlag flag = (TagFrameFlag)(header[8] << 8 + header[9]);
                idx += 10;
                byte[] content = bytes.Skip(idx).Take(size).ToArray();
                idx += size;
                if (size == 0)
                {
                    break;
                }
                string s = Encoding.Default.GetString(content, 1, content.Length - 1);
                this.Tags[id] = new TagFrame() { ID = id, Value = s, Flag = flag };
            }
        }
        /// <summary>
        /// Writes the specified target stream.
        /// </summary>
        /// <param name="targetStream">The target stream.</param>
        internal void Write(Stream targetStream)
        {
            targetStream.Seek(0, SeekOrigin.Begin);
            byte[] bytes = new byte[10];
            targetStream.Read(bytes, 0, 10);
            int headSize = 0;
            if (IsValidMagicHeader(bytes))
            {
                headSize = CalcSize(bytes.Skip(6).ToArray());
            }

            MemoryStream tagStream = CreateTagStream();
            int newHeadSize = (int)tagStream.Length + 10;
            if (newHeadSize >= headSize)
            {
                targetStream.Move(headSize, newHeadSize + 4096);
                headSize = newHeadSize + 4096;
            }
            targetStream.Seek(0, SeekOrigin.Begin);
            BinaryWriter writer1 = new BinaryWriter(targetStream);
            writer1.Write(Encoding.ASCII.GetBytes(MagicHeader));
            writer1.Write((char)(this.Version == 0 ? 3 : this.Version));
            writer1.Write(this.Revision);
            writer1.Write(new byte[1]);
            writer1.Write(CalcSize(headSize));

            writer1.Write(tagStream.ToArray());
            writer1.Write(new byte[headSize - targetStream.Position]);
        }

        /// <summary>
        /// Creates the tag stream.
        /// </summary>
        /// <returns></returns>
        private MemoryStream CreateTagStream()
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(ms);
            Encoding encoding = Encoding.Default;
            Encoding ascii = Encoding.ASCII;
            foreach (var item in this.Tags)
            {
                TagFrame tag = item.Value;
                writer.Write(ascii.GetBytes(item.Key));
                byte[] content = encoding.GetBytes(tag.Value);
                int frameSize = content.Length + 1;
                writer.Write(CalcSize(frameSize));
                writer.Write((byte)(((Int16)tag.Flag) & 0xff));
                writer.Write((byte)(((Int16)tag.Flag >> 8) & 0xff));
                writer.Write((byte)0);
                writer.Write(content);
            }
            return ms;
        }

        /// <summary>
        /// Calcs the size.
        /// </summary>
        /// <param name="size">The size.</param>
        /// <returns></returns>
        public static byte[] CalcSize(int size)
        {
            return new byte[]{
                (byte)(size >> 21 & 0x7F),
                (byte)((size >> 14) & 0x7F),
                (byte)((size >> 7) & 0x7F),
                (byte)(size & 0x7F)
            };
        }

        /// <summary>
        /// Determines whether [is valid magic header] [the specified bytes].
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <returns>
        /// 	<c>true</c> if [is valid magic header] [the specified bytes]; otherwise, <c>false</c>.
        /// </returns>
        private bool IsValidMagicHeader(byte[] bytes)
        {
            string magicHeader = Encoding.ASCII.GetString(bytes, 0, 3);
            this.Enabled = (magicHeader == MagicHeader);
            return this.Enabled;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class TagFrame
    {
        /// <summary>
        /// Gets or sets the ID.
        /// </summary>
        /// <value>The ID.</value>
        public string ID { get; set; }
        /// <summary>
        /// Gets or sets the flag.
        /// </summary>
        /// <value>The flag.</value>
        public TagFrameFlag Flag { get; set; }
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public string Value { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    [Flags]
    public enum TagFrameFlag
    {
        /// <summary>
        /// 
        /// </summary>
        TagProtected = 0x8000,
        /// <summary>
        /// 
        /// </summary>
        FileProtected = 0x4000,
        /// <summary>
        /// 
        /// </summary>
        ReadOnly = 0x0200,
        /// <summary>
        /// 
        /// </summary>
        Compressed = 0x80,
        /// <summary>
        /// 
        /// </summary>
        Cryptoed = 0x40,
        /// <summary>
        /// 
        /// </summary>
        Group = 0x20
    }
}
