using System;

namespace Moonlit.ObjectConverts.ObjectConverters
{
    internal class NullableByteObjectConverter : NullableObjectConverter<byte>
    {
        protected override byte ConvertCore(object value)
        {
            return Convert.ToByte(value);
        }
    }
}