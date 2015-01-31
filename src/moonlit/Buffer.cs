using System;
using System.Collections.Generic;
using System.IO;

namespace Moonlit
{
    /// <summary>
    /// ���ݿ������
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class Buffer<T> where T : IComparable<T>
    {
        /// <summary>
        /// ���ݱȽ�
        /// </summary>
        /// <param name="src">�Ƚ�Դ</param>
        /// <param name="dst">�Ƚ�Ŀ��</param>
        /// <returns>�ȽϽ��</returns>
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
        /// ���ݿ�Ƚ���
        /// </summary>
        /// <param name="src">�Ƚ�Դ</param>
        /// <param name="srcOffset">ƫ����</param>
        /// <param name="dst">�Ƚ�Ŀ��</param>
        /// <param name="dstOffset">Ŀ��ƫ����</param>
        /// <param name="length">�Ƚϳ���</param>
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

        #region �����С

        private const int BufSize = 1024*1024;

        #endregion

        #region Copy

        /// <summary>
        /// ��һ���������е����ݸ��Ƶ���һ����������
        /// </summary>
        /// <param name="src">Դ</param>
        /// <param name="dst">Ŀ����</param>
        /// <param name="enableFlush">�Ƿ���Ҫ����flush</param>
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
        /// ���ļ�����ȡ��
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
        /// �� <see cref="byte"/> ת���� <see cref="MemoryStream"/>
        /// </summary>
        /// <param name="bytes">������Ϣ���ֽ�����</param>
        /// <returns>���ֽ�<paramref name="bytes"/>�ж�ȡ����Ϣ</returns>
        public static MemoryStream ToStream(byte[] bytes)
        {
            var ms = new MemoryStream();
            ms.Write(bytes, 0, bytes.Length);
            return ms;
        }

        #endregion

        #region ToString

        /// <summary>
        /// �� <see cref="MemoryStream"/> ת���� <see cref="string"/>
        /// </summary>
        /// <param name="ms">������Ϣ���ڴ���</param>
        /// <returns>����<paramref name="ms"/>�ж�ȡ����Ϣ</returns>
        public static string ToString(MemoryStream ms)
        {
            ms.Position = 0;
            var reader = new StreamReader(ms);
            return reader.ReadToEnd();
        }

        #endregion

        #region ToStream

        /// <summary>
        /// �� <see cref="string"/> ת���� <see cref="MemoryStream"/>
        /// </summary>
        /// <param name="msg">��ת������Ϣ</param>
        /// <returns>��������Ϣ���ڴ���</returns>
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
        /// ����<paramref name="ms"/>����<paramref name="time"/>��
        /// </summary>
        /// <param name="ms">�����Ƶ���</param>
        /// <param name="time">���ƴ���</param>
        /// <returns>���ƽ��</returns>
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