using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Moonlit.Mvc.Url
{
    public static class UrlHelperHelper
    {
        public static string RouteUrl(this UrlHelper urlHelper, RequestMapping requestMapping, object routeValues)
        {
            return requestMapping.MakeUrl(urlHelper, routeValues);
        }
    }
}
