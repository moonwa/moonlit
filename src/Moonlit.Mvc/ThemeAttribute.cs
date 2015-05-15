using System.Web.Mvc;

namespace Moonlit.Mvc
{
    public class ThemeAttribute : ActionFilterAttribute
    {
        private readonly Themes _themes;

        public ThemeAttribute(  Themes themes)
        {
            _themes = themes;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var moonlitContext = MoonlitContext.Current;

            moonlitContext.Theme = _themes.GetTheme(null);
            moonlitContext.Theme.PreRequest(moonlitContext, filterContext.RequestContext);

            base.OnActionExecuting(filterContext);
        }
    }
}