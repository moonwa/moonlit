using System.Web.Mvc;

namespace Moonlit.Mvc
{
    public class DependencyAttribute : ActionFilterAttribute
    {
        private readonly IDependencyResolver _dependencyResolver;

        public DependencyAttribute(IDependencyResolver dependencyResolver)
        {
            _dependencyResolver = dependencyResolver;
        }


        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var moonlitContext = MoonlitContext.Current;


            moonlitContext.DependencyResolver = _dependencyResolver;
            base.OnActionExecuting(filterContext);
        }
    }
}