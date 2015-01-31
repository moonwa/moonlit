using System;

namespace Moonlit.ObjectConverts.ObjectConverters
{
    internal class NullableDecimalObjectConverter : NullableObjectConverter<decimal>
    { 
        protected override Decimal ConvertCore(object value)
        {
            return Convert.ToDecimal(value);
        }
    }
}