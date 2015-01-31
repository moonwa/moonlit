//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace Moonlit.Media.Mpeg
//{
//    public struct MpegTagHeader
//    {
//        public static int[][][] MpegBitrateIndex;
//        public static int[][] MpegSamplingFreq;

//        static MpegTagHeader()
//        {
//            MpegTagHeader.MpegBitrateIndex = new int[2][][];
//            MpegTagHeader.MpegBitrateIndex[Convert.ToInt32(MpegVersion.MpegI)] = new int[4][];
//            MpegTagHeader.MpegBitrateIndex[Convert.ToInt32(MpegVersion.MpegII)] = new int[4][];

//            int[][] versionI = MpegTagHeader.MpegBitrateIndex[Convert.ToInt32(MpegVersion.MpegI)];
//            int[][] versionII = MpegTagHeader.MpegBitrateIndex[Convert.ToInt32(MpegVersion.MpegII)];

//            versionI[Convert.ToInt32(MpegLayer.LayerI)]
//                = new int[] { 0, 32, 64, 96, 128, 160, 192, 224, 256, 288, 320, 352, 384, 416, 448 };
//            versionI[Convert.ToInt32(MpegLayer.LayerII)]
//                = new int[] { 0, 32, 48, 56, 64, 80, 96, 112, 128, 160, 192, 224, 256, 320, 384 };
//            versionI[Convert.ToInt32(MpegLayer.LayerIII)]
//                = new int[] { 0, 32, 40, 48, 56, 64, 80, 96, 112, 128, 160, 192, 224, 256, 320 };

//            versionII[Convert.ToInt32(MpegLayer.LayerI)]
//                = new int[] { 0, 32, 48, 56, 64, 80, 96, 112, 128, 144, 160, 176, 192, 224, 256 };
//            versionII[Convert.ToInt32(MpegLayer.LayerII)]
//                = new int[] { 0, 8, 16, 24, 32, 40, 48, 56, 64, 80, 96, 112, 128, 144, 160 };
//            versionII[Convert.ToInt32(MpegLayer.LayerIII)]
//                = new int[] { 0, 8, 16, 24, 32, 40, 48, 56, 64, 80, 96, 112, 128, 144, 160 };

//            MpegTagHeader.MpegSamplingFreq = new int[3][];
//            MpegTagHeader.MpegSamplingFreq[Convert.ToInt32(MpegVersion.MpegI)] = new int[] { 44100, 48000, 32000 };
//            MpegTagHeader.MpegSamplingFreq[Convert.ToInt32(MpegVersion.MpegII)] = new int[] { 22050, 24000, 16000 };
//        }


//        // 头信息
//        private int bitrateIndex;
//        private int samplerateIndex;

//        /// <summary>
//        /// 版本信息
//        /// </summary>
//        public MpegVersion Version { get; set; }
//        /// <summary>
//        /// 层
//        /// </summary>
//        public MpegLayer Layer { get; set; }
//        /// <summary>
//        /// 立体声模式
//        /// </summary>
//        public MpegChannelMode ChannelMode { get; set; }
//        /// <summary>
//        /// 保留
//        /// </summary>
//        public MpegModeEx ModeEx { get; set; }
//        /// <summary>
//        /// 强调方式
//        /// </summary>
//        public MpegEmphasis Emphasis { get; set; }
//        /// <summary>
//        /// 位率索引
//        /// </summary>
//        public int BitRateIndex
//        {
//            get
//            {
//                return this.bitrateIndex;
//            }
//            set
//            {
//                if (value < 1 || value > 14)
//                    throw new ArgumentOutOfRangeException("BitrateIndex字段只能在1 至 14之间");
//                this.bitrateIndex = value;
//            }
//        }
//        public int BitRate
//        {
//            get { return MpegBitrateIndex[Convert.ToInt32(this.Version)][Convert.ToInt32(this.Layer)][this.BitRateIndex]; }
//        }
//        /// <summary>
//        /// 采样率索引
//        /// </summary>
//        public int SampleRateIndex
//        {
//            get
//            {
//                return this.samplerateIndex;
//            }
//            set
//            {
//                if (value < 0 || value > 2) throw new ArgumentOutOfRangeException("SamplingFreq字段只能在1 至 2之间");
//                this.samplerateIndex = value;
//            }
//        }
//        /// <summary>
//        /// 原始媒体
//        /// </summary>
//        public bool Original { get; set; }
//        /// <summary>
//        /// 版权标志
//        /// </summary>
//        public bool CopyRight { get; set; }
//        /// <summary>
//        /// CRC校正
//        /// </summary>
//        public bool ErrorProtection { get; set; }
//        /// <summary>
//        /// 空白字
//        /// </summary>
//        public bool Padding { get; set; }
//        /// <summary>
//        /// 私有标志
//        /// </summary>
//        public bool Extension { get; set; }
//    }
//}
