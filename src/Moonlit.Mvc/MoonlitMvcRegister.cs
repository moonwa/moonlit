using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;
using Moonlit.Caching;

namespace Moonlit.Mvc
{
    public class MoonlitMvcRegister
    {
        public MoonlitMvcRegister EnableRequestMapping(RouteCollection routes)
        {
            RequestMappings.Current.MapRequestMappings(routes);
            var attribute = new RequestMappingsAttribute(RequestMappings.Current);
            GlobalFilters.Filters.Add(attribute);
            return this;
        }
        public MoonlitMvcRegister EnableSiteMap()
        {
            SiteMaps.Current.MapSiteMaps();

            GlobalFilters.Filters.Add(new SiteMapsAttribute(SiteMaps.Current));
            return this;
        }

        public MoonlitMvcRegister EnableDependencyResolver(IDependencyResolver dependencyResolver)
        {
            var attribute = new DependencyAttribute(dependencyResolver);
            GlobalFilters.Filters.Add(attribute);
            return this;
        }
        public MoonlitMvcRegister EnableTheme(Themes themes)
        {
            var attribute = new ThemeAttribute(themes);
            GlobalFilters.Filters.Add(attribute);
            return this;
        }
        public MoonlitMvcRegister EnableAuthenticate(Authenticate authenticate, IAuthenticateProvider authenticateProvider)
        {
            var attribute = new MoonlitAuthorizeAttribute(authenticate, authenticateProvider);
            GlobalFilters.Filters.Add(attribute);
            return this;
        }

        public void AutoRegister(IDependencyResolver dependencyResolver)
        {
            var authenticate = new Authenticate(dependencyResolver.Resolve<ICacheManager>());
            var authenticateProvider = dependencyResolver.Resolve<IAuthenticateProvider>();
            this.EnableDependencyResolver(dependencyResolver)
                .EnableAuthenticate(authenticate, authenticateProvider)
                .EnableRequestMapping(RouteTable.Routes)
                .EnableSiteMap()
                .EnableTheme(Themes.Current);
        }
    }

    public class SiteMapsAttribute : ActionFilterAttribute
    {
        private readonly SiteMaps _siteMaps;

        public SiteMapsAttribute(SiteMaps siteMaps)
        {
            _siteMaps = siteMaps;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            MoonlitContext.Current.SiteMaps = _siteMaps.Clone(filterContext.HttpContext.User);
            base.OnActionExecuting(filterContext);
        }
    }
}
