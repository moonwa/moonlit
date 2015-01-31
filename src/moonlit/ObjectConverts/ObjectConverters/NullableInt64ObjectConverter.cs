using System;

namespace Moonlit.ObjectConverts.ObjectConverters
{
    internal class NullableInt64ObjectConverter : NullableObjectConverter<long>
    {
        protected override Int64 ConvertCore(object value)
        {
            return Convert.ToInt64(value);
        }
    }
}