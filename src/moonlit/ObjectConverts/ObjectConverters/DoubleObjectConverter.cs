using System;

namespace Moonlit.ObjectConverts.ObjectConverters
{
    internal class DoubleObjectConverter : StructObjectConverter<double>
    {
        protected override Double ConvertCore(object value)
        {
            return Convert.ToDouble(value);
        } 
    }
}