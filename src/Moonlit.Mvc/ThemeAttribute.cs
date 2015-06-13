using System.Web.Mvc;

namespace Moonlit.Mvc
{
    public class ThemeAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            var theme = Theme.Current;
            if (theme != null)
            {
                theme.PreRequest(filterContext.RequestContext);
            }
            base.OnResultExecuting(filterContext);
        }
    }
}