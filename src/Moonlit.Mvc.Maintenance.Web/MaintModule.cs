using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Web.Compilation;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Moonlit.Caching;
using Moonlit.Mvc.Maintenance.Domains;
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
            builder.RegisterType<ModuleConfiguration>().AsSelf().InstancePerDependency();
            builder.RegisterType<UserLoader>().As<IUserLoader>().InstancePerDependency();
            builder.RegisterType<MaintainLanguageLoader>().As<ILanguageLoader>().InstancePerDependency();
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

    public class MaintainLanguageLoader : ILanguageLoader
    {
        private readonly ICacheManager _cacheManager;

        public MaintainLanguageLoader(ICacheManager cacheManager)
        {
            _cacheManager = cacheManager;
        }

        public string Get(string key)
        {
            var cultureName = Thread.CurrentThread.CurrentUICulture.Name;
            var cacheKey = $"languages-{cultureName.ToLower()}";
            var languages = _cacheManager.Get<LanguageItem[]>(cacheKey);
            if (languages == null)
            {
                var db = new MaintDbContext();
                languages = db.CultureTexts.Where(x => x.Culture.Name == cultureName)
                    .Select(x => new LanguageItem()
                    {
                        CultureName = cultureName,
                        Key = x.Name,
                        Text = x.Text,
                    }).ToArray();
                _cacheManager.Set(cacheKey, languages, TimeSpan.MaxValue);
            }
            return languages.FirstOrDefault(x => key.EqualsIgnoreCase(x.Key) && cultureName.EqualsIgnoreCase(x.CultureName))?.Text ?? key;
        }

        public class LanguageItem
        {
            public string Key { get; set; }
            public string Text { get; set; }
            public string CultureName { get; set; }
        }
    }
}