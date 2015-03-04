using System.Web.Mvc;
using System.Web.Routing;

namespace Moonlit.Mvc
{
    public class RequestMapping
    {
        internal string Name { get; set; }
        public string MakeUrl(object routeData)
        {
            UrlHelper urlHelper = new UrlHelper();
            return urlHelper.RouteUrl(this.Name, HtmlHelper.AnonymousObjectToHtmlAttributes(routeData));
        }
    }
}