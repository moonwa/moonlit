using System.Web.Mvc;

namespace Moonlit.Mvc.Scripts
{
    public class ScriptAttribute : ActionFilterAttribute
    {
        public ScriptAttribute()
        {
            Order = 5;
        }
        public override void OnResultExecuting(ResultExecutingContext filterContext) 
        {
            filterContext.HttpContext.SetObject<Scripts>(new Scripts());
            base.OnResultExecuting(filterContext);
        }
    }
}