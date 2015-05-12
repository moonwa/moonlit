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

        public MoonlitMvcRegister(RouteCollection routes)
        {
            _routes = routes;
        }
         
        public void Register( )
        { 
            RequestMappings.Current.MapRequestMappings(_routes);
            GlobalFilters.Filters.Add(new MoonlitMvcAttribute(RequestMappings.Current, Themes.Current));
        }
        
    }
}
