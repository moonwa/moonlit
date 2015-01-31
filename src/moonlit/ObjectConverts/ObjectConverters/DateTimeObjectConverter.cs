using System;

namespace Moonlit.ObjectConverts.ObjectConverters
{
    internal class DateTimeObjectConverter : StructObjectConverter<DateTime>
    {
        protected override DateTime ConvertCore(object value)
        {
            return Convert.ToDateTime(value);
        }
    }
}