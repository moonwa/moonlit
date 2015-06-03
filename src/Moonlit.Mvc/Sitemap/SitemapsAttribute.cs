using System.Web.Mvc;

namespace Moonlit.Mvc.Sitemap
{
    public class SitemapsAttribute : MoonlitActionFilterAttribute
    {
        private readonly Sitemaps _sitemaps;

        public SitemapsAttribute(Sitemaps sitemaps)
        {
            _sitemaps = sitemaps;
        }

        public SitemapsAttribute()
            : this(Sitemaps.Current)
        {
        }
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var model = ResolveModel(filterContext);
            IMoonlitModel sitemapModel = model as IMoonlitModel;
            if (sitemapModel != null)
            {
                sitemapModel.SetObject(_sitemaps.Clone(filterContext.HttpContext.User));
            }
            base.OnActionExecuted(filterContext);
        }
    }
}