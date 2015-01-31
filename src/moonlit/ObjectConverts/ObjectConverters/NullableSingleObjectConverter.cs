using System;

namespace Moonlit.ObjectConverts.ObjectConverters
{
    internal class NullableSingleObjectConverter : NullableObjectConverter<float>
    {
        protected override Single ConvertCore(object value)
        {
            return Convert.ToSingle(value);
        } 
    }
}