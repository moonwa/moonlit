using System.Web.Mvc;

namespace Moonlit.Mvc.Themes
{
    public class ThemeAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            var theme = DependencyResolver.Current.GetService<IThemeLoader>().Theme;
            theme.PreRequest(filterContext.RequestContext);
            base.OnResultExecuting(filterContext);
        }
    }
}