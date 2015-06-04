using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Moonlit.Mvc
{
    public static class HttpContextBaseHelper
    {
        public static void SetObject<T>(this HttpContextBase httpContext, T target)
        {
            httpContext.Items[typeof(T).FullName] = target;
        }

        public static T GetObject<T>(this HttpContextBase httpContext)
        {
            return (T)httpContext.Items[typeof(T).FullName];
        }
    }
}
