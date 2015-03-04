using System.Web.Mvc;

namespace Moonlit.Mvc
{
    public class RequestMappingsAttribute : ActionFilterAttribute
    {
        private readonly RequestMappings _requestMappings;

        public RequestMappingsAttribute(RequestMappings requestMappings)
        {
            _requestMappings = requestMappings;
        }

        internal const string RequestRoutesKey = "_RequestRoutesAttribute";
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.RequestContext.HttpContext.Items[RequestRoutesKey] = _requestMappings;
            base.OnActionExecuting(filterContext);
        }
    }
}