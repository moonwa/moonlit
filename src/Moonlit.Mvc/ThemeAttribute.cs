using System.Web.Mvc;

namespace Moonlit.Mvc
{
    public class ThemeAttribute : MoonlitActionFilterAttribute
    {
        private readonly Themes _themes;

        public ThemeAttribute(  Themes themes)
        {
            _themes = themes;
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var model = ResolveModel(filterContext) as IMoonlitModel;
            if (model != null)
            {
                var theme = _themes.GetTheme(null);
//                theme.PreRequest(moonlitContext, filterContext.RequestContext);
//                model.SetObject(theme);
            }
            base.OnActionExecuted(filterContext);
        }
    }
}