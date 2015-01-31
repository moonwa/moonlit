using System;
using System.Collections.Generic;
using System.Text;

namespace Moonlit
{
    public static class ByteArrayHelper
    {
        public static byte ToBcd(this byte value)
        {
            return (byte)(((value / 10) << 4) + value % 10);
        }
        public static byte FromBcd(this byte value)
        {
            return (byte)((value >> 4) * 10 + value % 0x10);
        }
        public static string ToHexString(this byte[] bytes)
        {
            return BitConverter.ToString(bytes).Replace("-", "");
        }

        public static string ToString(this byte[] bytes, Encoding encoding)
        {
            return encoding.GetString(bytes);
        }

        public static byte[] PadRight(this byte[] bytes, int length, byte value = 0)
        {
            var buf = new List<byte>();
            if (bytes != null)
                buf.AddRange(bytes);
            int count = length - buf.Count;
            while (count-- > 0)
            {
                buf.Add(value);
            }
            return buf.ToArray();
        }
        public static byte[] PadLeft(this byte[] bytes, int length, byte value = 0)
        {
            int count = length;
            if (bytes != null)
                count = length - bytes.Length;
            var buf = new List<byte>();
            while (count-- > 0)
            {
                buf.Add(value);
            }
            buf.AddRange(bytes);
            return buf.ToArray();
        }
    }
}