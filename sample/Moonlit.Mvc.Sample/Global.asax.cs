using System;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Web;
using System.Web.Compilation;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using Moonlit.Caching;
using Moonlit.Mvc.Templates;

namespace Moonlit.Mvc.Sample
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            var builder = new ContainerBuilder();
            builder.RegisterType<MemoryCacheManager>().As<ICacheManager>().SingleInstance();
            builder.RegisterType<SessionCachingFlash>().As<IFlash>();
            builder.RegisterType<TestAuthenticateProvider>().As<IAuthenticateProvider>();
            builder.RegisterType<Authenticate>().AsSelf();


            var controllerAssemblies = new[] { typeof(MvcApplication).Assembly };
            builder.RegisterControllers(controllerAssemblies).PropertiesAutowired();
            // OPTIONAL: Register model binders that require DI.
            builder.RegisterModelBinders(controllerAssemblies);
            builder.RegisterModelBinderProvider();

            // OPTIONAL: Register web abstractions like HttpContextBase.
            builder.RegisterModule<AutofacWebTypesModule>();

            // OPTIONAL: Enable property injection in view pages.
            builder.RegisterSource(new ViewRegistrationSource());

            // OPTIONAL: Enable property injection into action filters.
            builder.RegisterFilterProvider();

            // Set the dependency resolver to be Autofac.


            var clipOneTheme = new ClipOneTheme();
            Themes themes = new Themes();
            themes.Register(clipOneTheme);
            builder.Register(context => new DefaultThemeLoader("clip-one", themes)).As<IThemeLoader>();

            builder.RegisterType<ReflectionSitemapsLoader>().As<ISitemapsLoader>();
            builder.RegisterType<MyTaskLoader>().As<ITaskLoader>() ;

            var container = builder.Build();
            RequestMappings.Current.Register(RouteTable.Routes);
            AuthenticationManager.Current.Register(new Authenticate(container.Resolve<ICacheManager>()), new TestAuthenticateProvider());
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }

    public class TestAuthenticateProvider : IAuthenticateProvider
    {
        public IUserPrincipal GetUserPrincipal(string name)
        {
            return new TestUserPrincipal(name);
        }
    }

    public class TestUserPrincipal : IUserPrincipal
    {
        private readonly string _name;

        public TestUserPrincipal(string name)
        {
            _name = name;
            this.Privileges = new string[] { "edit" };
        }

        public bool IsInRole(string role)
        {
            if (Privileges == null)
            {
                return false;
            }
            return Privileges.Any(x => string.Equals(x, role, StringComparison.OrdinalIgnoreCase));
        }

        public IIdentity Identity
        {
            get
            {
                return new GenericIdentity(this._name);
            }
        }

        public string[] Privileges { get; set; }
    }

 
}
