using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Moonlit.Media.Mpeg
{
    /// <summary>
    /// 
    /// </summary>
    public class MpegInfo
    {
        //private MpegTagHeader _header = new MpegTagHeader();

        //public MpegTagHeader Header
        //{
        //    get { return this._header; }
        //}
        /// <summary>
        /// Gets or sets the I d3 v1.
        /// </summary>
        /// <value>The I d3 v1.</value>
        public MpegID3V1 ID3V1 { get; private set; }
        /// <summary>
        /// Gets or sets the I d3 v2.
        /// </summary>
        /// <value>The I d3 v2.</value>
        public MpegID3V2 ID3V2 { get; set; }
        /// <summary>
        /// Loads the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns></returns>
        public static MpegInfo Load(Stream stream)
        {
            MpegInfo mpeg = new MpegInfo();
            mpeg.ReadMpegInfo(stream);
            return mpeg;
        }
        /// <summary>
        /// bits name              comments
        /// --------------------------------------------------
        /// 12   sync              0xFFF
        /// 1    version           1=mpeg1.0, 0=mpeg2.0
        /// 2    lay               4-lay = layerI, II or III
        /// 1    error protection  0=yes, 1=no
        /// 
        /// 4    bitrate_index     see table below
        /// 2    sampling_freq     see table below
        /// 1    padding
        /// 1    extension         see table below
        /// 
        /// 2    mode              see table below
        /// 2    mode_ext          used with "joint stereo" mode
        /// 1    copyright         0=no 1=yes
        /// 1    original          0=no 1=yes
        /// 2    emphasis          see table below
        /// --------------------------------------------------
        /// </summary>
        private MpegInfo()
        {

            this.ID3V2 = new MpegID3V2();
            this.ID3V1 = new MpegID3V1();
        }

        /// <summary>
        /// Reads the MPEG info.
        /// </summary>
        /// <param name="srcStream">The SRC stream.</param>
        public void ReadMpegInfo(System.IO.Stream srcStream)
        {
            long oldPosition = srcStream.Position;
            //this.ReadHeadInfo(srcStream);
            this.ReadID3V1(srcStream);
            this.ID3V2.Read(srcStream);
            srcStream.Position = oldPosition;
        }

        private void ReadID3V2(Stream srcStream)
        {

        }
        //private void ReadHeadInfo(System.IO.Stream srcStream)
        //{
        //    srcStream.Seek(0, System.IO.SeekOrigin.Begin);
        //    int byte1 = srcStream.ReadByte();
        //    int byte2 = srcStream.ReadByte();
        //    int byte3 = srcStream.ReadByte();
        //    int byte4 = srcStream.ReadByte();

        //    Validator.Validate(byte1 == 0xFF && ((byte)byte2 & 0xE0) == 0xE0, "头11个字节不为 0xFFF");

        //    this._header.Version = (MpegVersion)(byte2 & 0x18);          // 000XX000
        //    this._header.Layer = (MpegLayer)(byte2 & 0x06);                    // 00000XX0
        //    this._header.ErrorProtection = !Convert.ToBoolean(byte2 & 0x01);                 // 0000000X

        //    this._header.BitRateIndex = ((byte3 & 0xF0) >> 4);                               // XXXX0000
        //    this._header.SampleRateIndex = byte3 & 0x0C;                                     // 0000XX00
        //    this._header.Padding = Convert.ToBoolean(byte3 & 0x02);                          // 000000X0
        //    this._header.Extension = Convert.ToBoolean(byte3 & 0x01);                        // 0000000X

        //    this._header.ChannelMode = (MpegChannelMode)(byte4 & 0xC0);        // XX000000
        //    this._header.ModeEx = (MpegModeEx)(byte4 & 0x30);                  // 00XX0000
        //    this._header.CopyRight = Convert.ToBoolean(byte4 & 0x08);                        // 0000X000
        //    this._header.Original = Convert.ToBoolean(byte4 & 0x03);                         // 00000X00
        //    this._header.Emphasis = (MpegEmphasis)(byte4 & 0x03);              // 000000XX
        //}
        private void ReadID3V1(System.IO.Stream srcStream)
        {
            if (srcStream.Length < 128)
                throw new System.IO.FileLoadException("文件大小小于128字节，无法读取Mpeg歌曲信息");
            srcStream.Seek(-128, System.IO.SeekOrigin.End);

            this.ID3V1.Identify = this.ReadID3Field(srcStream, 3);
            this.ID3V1.Title = this.ReadID3Field(srcStream, 30);
            this.ID3V1.Artist = this.ReadID3Field(srcStream, 30);
            this.ID3V1.Album = this.ReadID3Field(srcStream, 30);
            this.ID3V1.Year = this.ReadID3Field(srcStream, 4);
            this.ID3V1.Comment = this.ReadID3Field(srcStream, 30);

            this.ID3V1.Genre = (MpegGenre)Enum.ToObject(typeof(MpegGenre), this.ReadGenre(srcStream));
        }
        private int ReadGenre(System.IO.Stream stream)
        {
            byte[] bytes = new byte[1];
            int readCount = stream.Read(bytes, 0, bytes.Length);
            if (readCount != 1) throw new System.IO.FileLoadException("读取Genre失败");
            return (int)bytes[0];
        }
        private string ReadID3Field(System.IO.Stream stream, int size)
        {
            byte[] bytes = new byte[size];
            int readCount = stream.Read(bytes, 0, bytes.Length);
            if (readCount != size) throw new System.IO.FileLoadException("读取歌曲信息失败");
            return Encoding.Default.GetString(bytes).Trim('\0');
        }
        /// <summary>
        /// Saves the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public void Save(Stream stream)
        {
            this.ID3V2.Write(stream);
            this.WriteID3V1(stream);
        }
        private void WriteID3V1(System.IO.Stream stream)
        {
            stream.Seek(-128, System.IO.SeekOrigin.End);
            this.WriteID3Field(stream, this.ID3V1.Identify, 3);
            this.WriteID3Field(stream, this.ID3V1.Title, 30);
            this.WriteID3Field(stream, this.ID3V1.Artist, 30);
            this.WriteID3Field(stream, this.ID3V1.Album, 30);
            this.WriteID3Field(stream, this.ID3V1.Year, 4);
            this.WriteID3Field(stream, this.ID3V1.Comment, 30);

        }
        private void WriteID3Field(System.IO.Stream stream, string field, int size)
        {
            byte[] bytes = Encoding.Default.GetBytes(field);
            stream.Write(bytes, 0, Math.Min(bytes.Length, size));

            if (bytes.Length < size)
            {
                byte[] pad = new byte[size - bytes.Length];
                for (int i = 0; i < pad.Length; i++)
                {
                    pad[i] = Convert.ToByte('\0');
                }
                stream.Write(pad, 0, pad.Length);
            }
        }
    }
}
