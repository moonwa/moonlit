using System;

namespace Moonlit.ObjectConverts.ObjectConverters
{
    internal class NullableBooleanObjectConverter : NullableObjectConverter<bool>
    {
        protected override bool ConvertCore(object value)
        {
            return Convert.ToBoolean(value);
        }
    }
}