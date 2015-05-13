using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;

namespace Moonlit.Mvc
{
    public class MoonlitMvcRegister
    {
        private readonly RouteCollection _routes;
        private IDependencyResolver _dependencyResolver = null;

        public MoonlitMvcRegister(RouteCollection routes)
        {
            _routes = routes;
        }

        public void Register()
        {
            RequestMappings.Current.MapRequestMappings(_routes);
            var attribute = new MoonlitMvcAttribute(RequestMappings.Current, Themes.Current);
            if (_dependencyResolver != null)
            {
                attribute.DependencyResolver = _dependencyResolver;
            }
            GlobalFilters.Filters.Add(attribute);
        }

        public MoonlitMvcRegister SetDependencyResolvor(IDependencyResolver dependencyResolver)
        {
            _dependencyResolver = dependencyResolver;
            return this;
        }
    }
}
