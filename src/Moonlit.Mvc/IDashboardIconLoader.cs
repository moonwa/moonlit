using System.Web.Routing;

namespace Moonlit.Mvc
{
    public interface IDashboardIconLoader
    {
        DashboardIcons Create(RequestContext requestContext);
    }
}