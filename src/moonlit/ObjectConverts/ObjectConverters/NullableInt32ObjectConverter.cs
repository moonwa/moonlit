using System;

namespace Moonlit.ObjectConverts.ObjectConverters
{
    internal class NullableInt32ObjectConverter : NullableObjectConverter<int>
    {
        protected override Int32 ConvertCore(object value)
        {
            return Convert.ToInt32(value);
        }
    }
}