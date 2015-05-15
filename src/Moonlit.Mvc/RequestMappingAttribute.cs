using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Moonlit.Mvc
{
    public class RequestMappingAttribute : ActionFilterAttribute
    {
        public string Name { get; set; }
        public string Url { get; set; }

        public RequestMappingAttribute()
        {

        }
        public RequestMappingAttribute(string name)
        {
            Name = name;
        }

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
                RequestMappingsAttribute attr = (RequestMappingsAttribute)attrs[0];
                var requestMapping = RequestMappings.Current.GetRequestMapping(this.Name);
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
    }
}
