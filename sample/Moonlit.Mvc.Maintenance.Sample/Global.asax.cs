using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using Moonlit.Mvc.Dojo;

namespace Moonlit.Mvc.Maintenance.Sample
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var builder = new ContainerBuilder();

            var ngmaModule = new MaintModule();
            ngmaModule.AddTheme(new DojoTheme() , true);
            builder.RegisterModule(ngmaModule);
            builder.RegisterType<ReflectionDashboardIconLoader>().As<IDashboardIconLoader>();
           
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            container.Resolve<ModuleConfiguration>().Configure(container);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}
