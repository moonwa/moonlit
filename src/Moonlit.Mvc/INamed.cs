using System.Web.Routing;

namespace Moonlit.Mvc
{
    public interface IUrl
    {
        string MakeUrl(RequestContext requestContext);
    }
    public interface INamed
    {
        string Name { get; }
    }
}