using System.Diagnostics;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Moonlit.Mvc
{
    [DebuggerDisplay("{Name}")]
    public class RequestMapping
    {
        internal string Name { get; set; }

        public static RequestMapping Current
        {
            get { return HttpContext.Current.GetObject<RequestMapping>(); }
            set { HttpContext.Current.SetObject(value); }
        }

        public string MakeUrl(UrlHelper urlHelper, object routeData)
        {
            return urlHelper.RouteUrl(this.Name, HtmlHelper.AnonymousObjectToHtmlAttributes(routeData));
        }
    }
}