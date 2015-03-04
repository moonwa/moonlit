using System.Web.Routing;

namespace Moonlit.Mvc
{
    public class RequestMapping
    {
        internal string Url { get; set; }
        internal string Name { get; set; }

        public string MakeUrl(RequestContext requestContext)
        {
            return this.Url;
        }
    }
}