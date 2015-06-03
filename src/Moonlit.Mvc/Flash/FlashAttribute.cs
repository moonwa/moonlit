using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Moonlit.Mvc.Sitemap;

namespace Moonlit.Mvc.Flash
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Assembly, AllowMultiple = true, Inherited = true)]
    public class FlashAttribute : MoonlitActionFilterAttribute
    {
        private readonly IFlash _flash;

        public FlashAttribute()
            : this(Flash.Current)
        {

        }
        public FlashAttribute(IFlash flash)
        {
            _flash = flash;
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var model = ResolveModel(filterContext) as IFlashModel;
            if (model != null)
            {
                model.Flash = _flash;
                base.OnActionExecuted(filterContext);
            }
        }
    }
}