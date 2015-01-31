using System;

namespace Moonlit.ObjectConverts.ObjectConverters
{
    internal class BooleanObjectConverter : StructObjectConverter<bool>
    {
        protected override bool ConvertCore(object value)
        {
            return Convert.ToBoolean(value);
        }
    }
}