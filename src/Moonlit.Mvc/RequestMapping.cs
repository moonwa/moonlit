using System.Diagnostics;
using System.Web.Mvc;
using System.Web.Routing;

namespace Moonlit.Mvc
{
    [DebuggerDisplay("{Name}")]
    public class RequestMapping
    {
        internal string Name { get; set; }
        public string MakeUrl(UrlHelper urlHelper, object routeData)
        { 
            return urlHelper.RouteUrl(this.Name, HtmlHelper.AnonymousObjectToHtmlAttributes(routeData));
        }
    }
}