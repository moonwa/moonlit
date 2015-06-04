using System.Web.Mvc;

namespace Moonlit.Mvc.Themes
{
    public class ThemeAttribute : ActionFilterAttribute
    {
        private readonly Themes _themes;

        public ThemeAttribute(Themes themes)
        {
            _themes = themes;
            Order = 10;
        }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            var theme = _themes.GetTheme(null);
            theme.PreRequest(filterContext.RequestContext);
            filterContext.HttpContext.SetObject(theme);
            base.OnResultExecuting(filterContext);
        }
    }
}