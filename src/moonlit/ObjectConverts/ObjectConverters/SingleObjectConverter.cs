using System;

namespace Moonlit.ObjectConverts.ObjectConverters
{
    internal class SingleObjectConverter : StructObjectConverter<float>
    {
        protected override Single ConvertCore(object value)
        {
            return Convert.ToSingle(value);
        } 
    }
}