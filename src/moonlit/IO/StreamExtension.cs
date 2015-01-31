using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace Moonlit.IO
{
    /// <summary>
    /// 流扩展方法
    /// </summary>
    public static class StreamExtension
    {
        public static string ReadToEnd(this Stream stream)
        {
            StreamReader reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }
        public static Stream Write(this Stream stream, byte[] bytes)
        {
            stream.Write(bytes, 0, bytes.Length);
            return stream;
        }
        const int BufSize = 1024 * 1024;
        /// <summary>
        /// 转换为字节数组
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns></returns>
        public static byte[] ToArray(this Stream stream)
        {
            if (stream is MemoryStream)
            {
                return ((MemoryStream)stream).ToArray();
            }
            long oldPos = -1;
            if (stream.CanSeek)
                oldPos = stream.Position;

            List<byte> result = new List<byte>();
            byte[] buf = new byte[BufSize];

            int readCnt = -1;
            while (readCnt != 0)
            {
                readCnt = stream.Read(buf, 0, buf.Length);
                result.AddRange(buf.Take(readCnt));
            }
            if (stream.CanSeek)
                stream.Position = oldPos;
            return result.ToArray();
        }
        /// <summary>
        /// Moves the specified target stream.
        /// </summary>
        /// <param name="targetStream">The target stream.</param>
        /// <param name="start">The start.</param>
        /// <param name="offset">The offset.</param>
        public static void Move(this Stream targetStream, int start, int offset)
        {
            MemoryStream buf = new MemoryStream();
            targetStream.Position = start;
            Buffer<Byte>.Copy(targetStream, buf, false);
            targetStream.Position = start + offset;
            buf.Position = 0;
            Buffer<Byte>.Copy(buf, targetStream, true);
        }
    }
}
