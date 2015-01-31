using System.Reflection;

namespace Moonlit.ObjectConverts
{
    [System.Diagnostics.DebuggerDisplay("{PropertyInfo.Name}")]
    public class PropertyConversion
    {
        public PropertyInfo PropertyInfo { get; private set; }

        public PropertyConversion(PropertyInfo propertyInfo)
        {
            PropertyInfo = propertyInfo;
        }
    }
}