namespace Moonlit
{
    public static class IntegerHelper
    {
        public static TValue DefaultValue<TValue>(this TValue? value)
            where TValue : struct
        {
            return DefaultValue(value, default(TValue));
        }
        public static TValue DefaultValue<TValue>(this TValue? value, TValue defaultValue)
            where TValue : struct
        {
            if (value.HasValue)
                return value.Value;
            return defaultValue;
        }
    }
}