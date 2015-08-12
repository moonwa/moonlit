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
        public static string GetRequestMappingUrl(this UrlHelper urlHelper, string mappingName, object routeValues = null)
        {
            return RequestMappings.Current.GetRequestMapping(mappingName).MakeUrl(urlHelper, routeValues);
        }
        public static string Asset
            (this UrlHelper urlHelper, string url)
        {
            return "http://121.42.41.232:8033/" + url;
        }
    }
}
