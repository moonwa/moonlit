using System;
using System.Collections;
using System.Collections.Generic;

namespace Moonlit
{
    public static class SpellingExtensions
    {
        public static string ToPlural(this ISpelling spelling, Type type)
        {
            if (type.IsGenericType )
            {
                foreach (var typeArg in type.GetGenericArguments())
                {
                    if(typeof(IEnumerable<>).MakeGenericType(typeArg).IsAssignableFrom(type))
                        return spelling.ToPlural(typeArg.Name);
                }
            }
            return spelling.ToPlural(type.Name);
        }
    }
}
