using System.Linq;
using System.Reflection;
using System.Web.Compilation;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Moonlit.Caching; 
using Module = Autofac.Module;

namespace Moonlit.Mvc.Maintenance
{
    public class MaintModule : Module
    {
        private Themes _themes = Themes.Current;
        protected override void Load(ContainerBuilder builder)
        {
            var assemblies = BuildManager.GetReferencedAssemblies().Cast<Assembly>().Where(x => x.GetCustomAttribute<MvcAttribute>() != null).ToArray();
            builder.RegisterControllers(assemblies).PropertiesAutowired();

            // OPTIONAL: Register model binders that require DI.
            builder.RegisterModelBinders(assemblies);
            builder.RegisterModelBinderProvider();

            // OPTIONAL: Register web abstractions like HttpContextBase.
            builder.RegisterModule<AutofacWebTypesModule>();

            // OPTIONAL: Enable property injection in view pages.
            builder.RegisterSource(new ViewRegistrationSource());

            // OPTIONAL: Enable property injection into action filters.
            builder.RegisterFilterProvider();

            builder.RegisterType<MemoryCacheManager>().As<ICacheManager>().SingleInstance(); 
            builder.RegisterType<SessionCachingFlash>().As<IFlash>();
            builder.RegisterType<ReflectionPrivilegeLoader>().As<IPrivilegeLoader>().SingleInstance();
            builder.RegisterType<CacheKeyManager>().AsSelf().SingleInstance();
            builder.RegisterType<ModuleConfiguration>().AsSelf().InstancePerDependency();
            builder.RegisterType<UserLoader>().As<IUserLoader>().InstancePerDependency(); 
            builder.RegisterType<MaintModuleConfiguration>().As<IModuleConfiguration>();
            builder.RegisterType<Authenticate>().As<Authenticate>();
            builder.RegisterType<ReflectionSitemapsLoader>().As<ISitemapsLoader>();
            builder.RegisterType<ReflectionDashboardIconLoader>().As<IDashboardIconLoader>();
            builder.Register(context => new DefaultThemeLoader(_defaultThemeName)).As<IThemeLoader>(); 

            MvcConfigure.EnableCulture();
            GlobalFilters.Filters.Add(new ExceptionLogAttribute());
            base.Load(builder);
        }

        private string _defaultThemeName;
        public void AddTheme(Theme theme, bool isDefault)
        {
            _themes.Register(theme);
            if (isDefault)
            {
                _defaultThemeName = theme.Name;
            }
        }
    }

}