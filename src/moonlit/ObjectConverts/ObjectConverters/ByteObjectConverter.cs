using System;

namespace Moonlit.ObjectConverts.ObjectConverters
{
    internal class ByteObjectConverter : StructObjectConverter<byte>
    {
        protected override byte ConvertCore(object value)
        {
            return Convert.ToByte(value);
        }
    }
}