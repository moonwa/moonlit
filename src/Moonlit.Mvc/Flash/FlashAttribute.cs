using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Moonlit.Mvc.Sitemap;

namespace Moonlit.Mvc.Flash
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Assembly, AllowMultiple = true, Inherited = true)]
    public class FlashAttribute : ActionFilterAttribute
    { 

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            filterContext.HttpContext.SetObject(DependencyResolver.Current.GetService<IFlash>());
            base.OnResultExecuting(filterContext);
        }
    }
}