using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Joinme.Loader;
using Microsoft.Owin.Security.OAuth;

namespace Joinme.Passport.Web
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

            JoinmeLoader loader = new JoinmeLoader();
            loader.ContainerBuilder.RegisterControllers(typeof(WebApiApplication).Assembly);
            loader.Load(JoinmeLoader.Web);

            Application["loader"] = loader;
        }
    }
}
