using System;

namespace Moonlit.ObjectConverts.ObjectConverters
{
    internal class Int16ObjectConverter : StructObjectConverter<short>
    { 
        protected override Int16 ConvertCore(object value)
        {
            return Convert.ToInt16(value);
        }
    }
}