using System.Web.Mvc;

namespace Moonlit.Mvc.Sitemap
{
    public class SitemapsAttribute : ActionFilterAttribute
    {
        private readonly Sitemaps _sitemaps;

        public SitemapsAttribute(Sitemaps sitemaps)
        {
            _sitemaps = sitemaps;
            Order = 1;
        }

        public SitemapsAttribute()
            : this(Sitemaps.Current)
        {
        }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            filterContext.HttpContext.SetObject(_sitemaps.Clone(filterContext.HttpContext.User));
            
            base.OnResultExecuting(filterContext);
        } 
    }
}