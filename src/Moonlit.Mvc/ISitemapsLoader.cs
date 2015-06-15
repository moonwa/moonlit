using System.Web.Routing;

namespace Moonlit.Mvc
{
    public interface ISitemapsLoader
    {
        Sitemaps Create(RequestContext requestContext);
    }
}