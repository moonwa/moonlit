using System;

namespace Moonlit.ObjectConverts.ObjectConverters
{
    internal class Int64ObjectConverter : StructObjectConverter<long>
    {
        protected override Int64 ConvertCore(object value)
        {
            return Convert.ToInt64(value);
        }
    }
}