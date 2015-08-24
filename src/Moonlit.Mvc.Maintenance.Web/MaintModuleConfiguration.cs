using System;
using System.Web.Mvc;
using Autofac;
using Moonlit.Mvc.Maintenance.Properties;
using Moonlit.Properties;

namespace Moonlit.Mvc.Maintenance
{
    internal class MaintModuleConfiguration : IModuleConfiguration
    {
        public void Configure(IContainer container)
        {
            AuthorizeManager.Setup();
            ModelBinders.Binders.DefaultBinder = new MaintInjectBinder(ModelBinders.Binders.DefaultBinder, container);

            Formatter.Register((x, v) => x == typeof(bool) || x == typeof(bool?), new BooleanFormatter(() => MaintCultureTextResources.Yes, () => MaintCultureTextResources.No));
            Formatter.Register((x, v) => (x == typeof(DateTime) || x == typeof(DateTime?)) && v != null && ((DateTime)v).Date == ((DateTime)v), new DateFormatter());
            Formatter.Register((x, v) => (x == typeof(DateTime) || x == typeof(DateTime?)), new DateTimeFormatter());
            Formatter.Register((x, v) => x.ToWithoutNullableType().IsEnum, new EnumFormatter());
            MoonlitCultureTextResources.LanguageLoader = DependencyResolver.Current.GetService<ILanguageLoader>(false) ??
                                                  new NullLanguageLoader();
        }
    }


}