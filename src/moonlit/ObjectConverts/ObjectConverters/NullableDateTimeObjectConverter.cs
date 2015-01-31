using System;

namespace Moonlit.ObjectConverts.ObjectConverters
{
    internal class NullableDateTimeObjectConverter : NullableObjectConverter<DateTime>
    {
        protected override DateTime ConvertCore(object value)
        {
            return Convert.ToDateTime(value);
        }
    }
}