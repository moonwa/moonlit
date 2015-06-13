using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Moonlit.Mvc
{
    public static class HttpContextExtensions
    {
        public static T GetObject<T>(this HttpContext httpContext, bool autoCreate) where T : class ,new()
        {
            T obj = httpContext.Items[typeof(T).FullName] as T;
            if (obj == null && autoCreate)
            {
                obj = new T();
                httpContext.Items[typeof(T).FullName] = obj;
            }
            return obj;
        }
    }
}
