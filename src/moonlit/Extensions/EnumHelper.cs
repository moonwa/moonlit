using System;

namespace Moonlit
{
    public static class EnumHelper
    {
        public static T Parse<T>(string value, bool ignoreCase = false)
        {
            return (T)Enum.Parse(typeof(T), value, ignoreCase);
        }
        public static string ToIntString<T>(this Nullable<T> value) where T : struct
        {
            if (value == null)
            {
                return null;
            }
            return Convert.ToInt32(value).ToString();
        }
    }
}