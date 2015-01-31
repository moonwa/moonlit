using System;

namespace Moonlit.ObjectConverts.ObjectConverters
{
    internal class DecimalObjectConverter : StructObjectConverter<Decimal>
    { 
        protected override Decimal ConvertCore(object value)
        {
            return Convert.ToDecimal(value);
        }
    }
}