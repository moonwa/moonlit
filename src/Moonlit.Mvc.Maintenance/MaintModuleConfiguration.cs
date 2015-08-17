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

            Formatter.Register(x => x == typeof(bool) || x == typeof(bool?), new BooleanFormatter(() => MaintCultureTextResources.Yes, () => MaintCultureTextResources.No));
            Formatter.Register(x => x == typeof(DateTime) || x == typeof(DateTime?), new DateFormatter());
            Formatter.Register(x => x.ToWithoutNullableType().IsEnum, new EnumFormatter());
            MoonlitCultureTextResources.LanguageLoader = DependencyResolver.Current.GetService<ILanguageLoader>(false) ??
                                                  new NullLanguageLoader();
        }
    }


}