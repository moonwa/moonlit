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
        public static T GetObject<T>(this HttpContext httpContext ) where T : class 
        {
            return httpContext.Items[typeof(T).FullName] as T;
        }
        public static void SetObject<T>(this HttpContext httpContext, T target) 
        {
            httpContext.Items[typeof(T).FullName] = target; 
        }
    }
}
