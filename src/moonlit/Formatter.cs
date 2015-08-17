using System;
using System.Collections.Generic;

namespace Moonlit
{
    public static class Formatter
    {
        static Dictionary<Predicate<Type>, IFormatter> _typeToFormatters = new Dictionary<Predicate<Type>, IFormatter>();
        public static void Register(Predicate<Type> predicate, IFormatter formatter)
        {
            _typeToFormatters[predicate] = formatter;
        }

        public static string Format<T>(this T value, params object[] args)
        {
            return Format<T>(value, null, args);
        }
        public static string Format<T>(this T value, IFormatter formatter, params object[] args)
        {
            formatter = formatter ?? GetFormatter<T>(value);
            return formatter.Format(value, args);
        }

        private static IFormatter GetFormatter<T>(T value)
        {
            foreach (var typeToFormatter in _typeToFormatters)
            {
                if (typeToFormatter.Key(typeof(T)))
                {
                    return typeToFormatter.Value;
                }
            }
            return new DefaultFormatter();
        }
    }
}