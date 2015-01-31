using System;

namespace Moonlit.ObjectConverts.ObjectConverters
{
    internal class NullableInt16ObjectConverter : NullableObjectConverter<short>
    { 
        protected override Int16 ConvertCore(object value)
        {
            return Convert.ToInt16(value);
        }
    }
}