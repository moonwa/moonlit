using System;
using System.Web.Routing;

namespace Moonlit.Mvc
{
    public static class RequestContextHelper
    {
        public static RequestMappings GetRequestMappings(this RequestContext requestContext)
        {
            var o = requestContext.HttpContext.Items[RequestMappingsAttribute.RequestRoutesKey];
            if (o == null)
            {
                throw new Exception("请注册全局 Filter RequestMappingsAttribute");
            }
            return (RequestMappings)o;
        }

        public static RequestMapping GetCurrentRequestMapping(this RequestContext requestContext)
        {
            return (RequestMapping)requestContext.HttpContext.Items[RequestMappingAttribute.RequestMappingKey];
        }
    }
}