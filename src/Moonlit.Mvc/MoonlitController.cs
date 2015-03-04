using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Moonlit.Mvc
{ 
    public abstract class MoonlitController : Controller
    {
        protected ActionResult RedirectToRequestMapping(string mappingName, object routeValues)
        {
            var mapping = this.ControllerContext.RequestContext.GetRequestMappings().GetRequestMapping(mappingName);
            if (mapping== null)
            {
                throw new Exception("Not found request mapping " + mappingName);
            }
            return RedirectToRequestMapping(mapping, routeValues);
        }
        protected ActionResult RedirectToRequestMapping(RequestMapping requestMapping, object routeValues)
        {
            return Redirect(requestMapping.MakeUrl(routeValues));
        }
    }
}
