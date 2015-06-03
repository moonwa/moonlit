using System.Web.Mvc;

namespace Moonlit.Mvc
{
    public class MoonlitActionFilterAttribute : ActionFilterAttribute
    {
        public object ResolveModel(ActionExecutedContext filterContext)
        {
            var vr = filterContext.Result as ViewResult;
            if (vr != null)
            {
                return vr.Model;
            }

            return null;
        }
    }
}