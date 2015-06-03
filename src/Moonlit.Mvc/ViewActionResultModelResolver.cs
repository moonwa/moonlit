using System.Web.Mvc;

namespace Moonlit.Mvc
{
    public class ViewActionResultModelResolver : IActionResultModelResolver
    {
        public object ResolveModel(ActionExecutedContext filterContext)
        {
            var viewResult = filterContext.Result as ViewResult;
            if (viewResult != null)
            {
                return viewResult.Model;
            }
            return null;
        }

    }
}