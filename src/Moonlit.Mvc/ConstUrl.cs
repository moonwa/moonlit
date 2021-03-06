using System.Web.Routing;

namespace Moonlit.Mvc
{
    public class ConstUrl : IUrl
    {
        public string Url { get; set; }

        public ConstUrl(string url)
        {
            Url = url;
        }

        public string MakeUrl(RequestContext requestContext)
        {
            return Url;
        }

        public bool IsCurrent(RequestContext requestContext)
        {
            return requestContext.HttpContext.Request.Url.ToString().ToLower().StartsWith(Url.ToLower());
        }
    }
}