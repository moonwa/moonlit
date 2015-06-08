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


        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            foreach (var parameterDescriptor in filterContext.ActionDescriptor.GetParameters())
            {
                if (parameterDescriptor.ParameterType == typeof(RequestMappings))
                {
                    filterContext.ActionParameters[parameterDescriptor.ParameterName] = _requestMappings;
                }
            }

            base.OnActionExecuting(filterContext);
        }
    }
}