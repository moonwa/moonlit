using System.Web.Routing;

namespace Moonlit.Mvc
{
    public interface IUrl
    {
        string MakeUrl(RequestContext requestContext);
        bool IsCurrent(RequestContext requestContext);
    }
}