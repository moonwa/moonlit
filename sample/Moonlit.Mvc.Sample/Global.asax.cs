using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Moonlit.Mvc.Sample
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            new MoonlitMvcRegister(RouteTable.Routes).Register();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

        }
    }
}
