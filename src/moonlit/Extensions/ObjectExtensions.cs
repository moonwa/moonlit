using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moonlit
{
    public static class ObjectExtensions
    {
        public static TResult IfNotNull<TResult>(this object obj, Func<object, TResult> func) where TResult : class
        {
            return obj == null ? default(TResult) : func(obj);
        }
    }
}
