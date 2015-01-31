using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Moonlit.Text
{
    /// <summary>
    /// 
    /// </summary>
    public class EncodingHeader
    {
        private static Dictionary<Encoding, byte[]> encodeHead = new Dictionary<Encoding, byte[]>();
        static EncodingHeader()
        {
            EncodingHeader.encodeHead[Encoding.UTF32] = new byte[] { 0xFF, 0xFE, 0x00, 0x00 };
            EncodingHeader.encodeHead[Encoding.Unicode] = new byte[] { 0xFF, 0xFE };
            EncodingHeader.encodeHead[Encoding.UTF8] = new byte[] { 0xEF, 0xBB, 0xBF };
            EncodingHeader.encodeHead[Encoding.BigEndianUnicode] = new byte[] { 0xFE, 0xFF };
        }
        /// <summary>
        /// 获取流中的编码类型
        /// </summary>
        /// <param name="stm">流</param>
        /// <returns>Encoding对象</returns>
        public static Encoding GetEncoding(Stream stm)
        {
            stm.Seek(0, SeekOrigin.Begin);
            Dictionary<Encoding, byte[]>.Enumerator enumer = EncodingHeader.encodeHead.GetEnumerator();
            byte[] readBytes = new byte[4];
            int readCount = stm.Read(readBytes, 0, 4);
            byte[] cmpBytes = new byte[readCount];
            System.Buffer.BlockCopy(readBytes, 0, cmpBytes, 0, readCount);
            while (enumer.MoveNext())
            {
                byte[] codeHead = enumer.Current.Value;
                if (cmpBytes.Length < codeHead.Length) continue;
                if (Moonlit.Buffer<byte>.BlockCompare(codeHead, 0, cmpBytes, 0, codeHead.Length) == 0)
                    return enumer.Current.Key;
            }
            return Encoding.Default;            
        }
    }
}
