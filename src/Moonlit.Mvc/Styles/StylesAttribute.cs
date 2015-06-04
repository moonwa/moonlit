using System.Web.Mvc;

namespace Moonlit.Mvc.Styles
{
    public class StylesAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            filterContext.HttpContext.SetObject(new Styles());
            base.OnResultExecuting(filterContext);
        }

        public StylesAttribute()
        {
            Order = 5;
        }
    }
}