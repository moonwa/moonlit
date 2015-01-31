using System;
using System.Collections.Generic;
using System.IO;

namespace Moonlit
{
    /// <summary>
    /// 数据块操作类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class Buffer<T> where T : IComparable<T>
    {
        /// <summary>
        /// 数据比较
        /// </summary>
        /// <param name="src">比较源</param>
        /// <param name="dst">比较目标</param>
        /// <returns>比较结果</returns>
        public static int BlockCompare(T[] src, T[] dst)
        {
            if (src == dst) return 0;
            if (src == null) return -1;
            if (dst == null) return 1;
            int length = Math.Min(src.Length, dst.Length);
            var ret = BlockCompare(src, 0, dst, 0, length);
            if (ret != 0) return ret;

            if (src.Length == dst.Length) return 0;
            if (src.Length > dst.Length) return 1;
            return -1;
        }

        /// <summary>
        /// 数据块比较类
        /// </summary>
        /// <param name="src">比较源</param>
        /// <param name="srcOffset">偏移量</param>
        /// <param name="dst">比较目标</param>
        /// <param name="dstOffset">目标偏移量</param>
        /// <param name="length">比较长度</param>
        /// <returns></returns>
        public static int BlockCompare(T[] src, int srcOffset, T[] dst, int dstOffset, int length)
        {
            for (int i = 0; i < length; i++)
            {
                int ret = src[i + srcOffset].CompareTo(dst[i + dstOffset]);
                if (ret == 0) continue;
                if (ret > 0) return 1;
                return -1;
            }
            return 0;
        }

        /// <summary>
        /// Combines the specified arrs.
        /// </summary>
        /// <param name="arrs">The arrs.</param>
        /// <returns></returns>
        public static T[] Combine(params T[][] arrs)
        {
            var arr = new List<T>();
            foreach (var item in arrs)
            {
                arr.AddRange(item);
            }
            return arr.ToArray();
        }

        #region 缓存大小

        private const int BufSize = 1024*1024;

        #endregion

        #region Copy

        /// <summary>
        /// 将一个数据流中的数据复制到另一个数据流中
        /// </summary>
        /// <param name="src">源</param>
        /// <param name="dst">目标流</param>
        /// <param name="enableFlush">是否需要调用flush</param>
        public static void Copy(Stream src, Stream dst, bool enableFlush)
        {
            var bytes = new byte[BufSize];

            int readCnt = 100;
            while (readCnt > 0)
            {
                readCnt = src.Read(bytes, 0, bytes.Length);
                dst.Write(bytes, 0, readCnt);
            }
            if (enableFlush)
            {
                dst.Flush();
            }
        }

        #endregion

        #region FromFile

        /// <summary>
        /// 从文件中提取流
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static MemoryStream FromFile(string filename)
        {
            var ms = new MemoryStream();
            using (var fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                Copy(fs, ms, false);
            }
            return ms;
        }

        #endregion

        #region ConvertBitArray2MemoryStream

        /// <summary>
        /// 从 <see cref="byte"/> 转换到 <see cref="MemoryStream"/>
        /// </summary>
        /// <param name="bytes">包含消息的字节数组</param>
        /// <returns>从字节<paramref name="bytes"/>中读取的消息</returns>
        public static MemoryStream ToStream(byte[] bytes)
        {
            var ms = new MemoryStream();
            ms.Write(bytes, 0, bytes.Length);
            return ms;
        }

        #endregion

        #region ToString

        /// <summary>
        /// 从 <see cref="MemoryStream"/> 转换到 <see cref="string"/>
        /// </summary>
        /// <param name="ms">包含消息的内存流</param>
        /// <returns>从流<paramref name="ms"/>中读取的消息</returns>
        public static string ToString(MemoryStream ms)
        {
            ms.Position = 0;
            var reader = new StreamReader(ms);
            return reader.ReadToEnd();
        }

        #endregion

        #region ToStream

        /// <summary>
        /// 从 <see cref="string"/> 转换到 <see cref="MemoryStream"/>
        /// </summary>
        /// <param name="msg">被转换的消息</param>
        /// <returns>包含该消息的内存流</returns>
        public static MemoryStream ToStream(string msg)
        {
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);

            writer.Write(msg);
            writer.Flush();
            return ms;
        }

        #endregion

        #region Clone

        /// <summary>
        /// 将流<paramref name="ms"/>复制<paramref name="time"/>次
        /// </summary>
        /// <param name="ms">被复制的流</param>
        /// <param name="time">复制次数</param>
        /// <returns>复制结果</returns>
        public static MemoryStream Clone(MemoryStream ms, int time)
        {
            byte[] bytes = ms.ToArray();
            var newStream = new MemoryStream();
            for (int i = 0; i < time; i++)
            {
                newStream.Write(bytes, 0, bytes.Length);
            }
            return newStream;
        }

        #endregion
    }
}