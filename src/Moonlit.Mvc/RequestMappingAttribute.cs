using System;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Moonlit.Mvc
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class RequestMappingAttribute : ActionFilterAttribute, INamed, IUrl
    {
        public string Name { get; set; }
        public string Url { get; set; }

        public RequestMappingAttribute(string name, string url)
        {
            Name = name;
            Url = url;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var attrs = filterContext.ActionDescriptor.GetCustomAttributes(typeof(RequestMappingsAttribute), true);
            if (attrs.Length > 0)
            {
                var requestMapping = RequestMappings.Current.GetRequestMapping(this.Name);
                RequestMapping.Current = requestMapping;
                foreach (var parameterDescriptor in filterContext.ActionDescriptor.GetParameters())
                {
                    if (parameterDescriptor.ParameterType == typeof(RequestMapping))
                    {
                        filterContext.ActionParameters[parameterDescriptor.ParameterName] = requestMapping;
                    }
                }
            }

            base.OnActionExecuting(filterContext);
        }

        public string MakeUrl(RequestContext requestContext)
        {
            return RequestMappings.Current.GetRequestMapping(this.Name).MakeUrl(new UrlHelper(requestContext), null);
        }
    }
}
