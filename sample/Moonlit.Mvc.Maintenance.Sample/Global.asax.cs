using System;
using System.Collections;
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
            ngmaModule.AddTheme(new DojoTheme(), true);
            builder.RegisterModule(ngmaModule);
            builder.RegisterType<ReflectionDashboardIconLoader>().As<IDashboardIconLoader>();

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            MoonlitDependencyResolver.SetResolver(new AutofacMoonlitDependencyResolver(container));
            container.Resolve<ModuleConfiguration>().Configure(container);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }

    public class AutofacMoonlitDependencyResolver : IDependencyResolver
    {
        private readonly IContainer _container;

        public AutofacMoonlitDependencyResolver(IContainer container)
        {
            _container = container;
        }

        public object Resolve(Type serviceType)
        {
            return _container.Resolve(serviceType);
        }

        public object Resolve(Type serviceType, string key)
        {
            return _container.ResolveKeyed(key, serviceType);
        }

        public IEnumerable<object> ResolveAll(Type serviceType)
        {
            return ((IEnumerable)_container.Resolve(typeof(IEnumerable<>).MakeGenericType(serviceType))).Cast<object>();
        }

        public void Release(object service)
        {
            using (service as IDisposable)
            {

            }
        }
    }
}
