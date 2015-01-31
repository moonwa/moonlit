using System;

namespace Moonlit.ObjectConverts.ObjectConverters
{
    internal class Int32ObjectConverter : StructObjectConverter<Int32>
    {
        protected override Int32 ConvertCore(object value)
        {
            return Convert.ToInt32(value);
        }
    }
}