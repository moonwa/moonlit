using System.Web.Mvc;
using Autofac;

namespace Moonlit.Mvc.Maintenance
{
    internal class MaintModuleConfiguration : IModuleConfiguration
    {
        public void Configure(IContainer container)
        {
            AuthorizeManager.Setup();
            ModelBinders.Binders.DefaultBinder = new MaintInjectBinder(ModelBinders.Binders.DefaultBinder, container);
             
            Moonlit.Properties.MoonlitCultureTextResources.LanguageLoader = DependencyResolver.Current.GetService<ILanguageLoader>(false) ??
                                                  new NullLanguageLoader();
        }
    }
}