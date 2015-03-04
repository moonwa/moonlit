using System;
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

        internal const string RequestMappingKey = "__RequestMappingKey";
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.RequestContext.HttpContext.Items[RequestMappingKey] = filterContext.RequestContext.GetRequestMappings().GetRequestMapping(this.Name);
            base.OnActionExecuting(filterContext);
        }
    }
}
