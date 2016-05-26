using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace Moonlit.Mvc.Themes.Dojo
{
    public static class DojoHelperExtensions
    {
        public static DojoHelper GetDojo(this WebViewPage context)
        {
            return (DojoHelper)(context.Context.Items["dojo_helper"] = context.Context.Items["dojo_helper"] ?? new DojoHelper(context.Context));
        }
        public static DojoHelper GetDojo(this HttpContextBase context)
        {
            return (DojoHelper)(context.Items["dojo_helper"] = context.Items["dojo_helper"] ?? new DojoHelper(context));
        }
        public static void AddRequire(this HttpContextBase context, string require)
        {
            List<string> requires = (List<string>)(context.Items["dojo_require"] = context.Items["dojo_require"] ?? new List<string>());
            requires.Add(require);
        }
    }
}