using System;

namespace Moonlit
{
    public static class EnumHelper
    {
        public static T Parse<T>(string value, bool ignoreCase = false)
        {
            return (T)Enum.Parse(typeof(T), value, ignoreCase);
        } 
    }
}