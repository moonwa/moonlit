using System;

namespace Moonlit.ObjectConverts.ObjectConverters
{
    internal class NullableDoubleObjectConverter : NullableObjectConverter<double>
    {
        protected override Double ConvertCore(object value)
        {
            return Convert.ToDouble(value);
        } 
    }
}