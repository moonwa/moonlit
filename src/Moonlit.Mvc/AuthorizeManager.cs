using System.Web.Mvc;
using Moonlit.Caching;

namespace Moonlit.Mvc
{
    public class AuthorizeManager
    {
        public static void Setup()
        {
            var cacheManager = DependencyResolver.Current.GetService<ICacheManager>(false);
            var userLoader = DependencyResolver.Current.GetService<IUserLoader>(false);
            //            var moonlitAuthorizeAttribute = new MoonlitAuthorizeAttribute(new Authenticate(cacheManager), userLoader)
            //            {
            //                Order = 100,
            //            };
            GlobalFilters.Filters.Add(new MoonlitAuthorizationAttribute(new Authenticate(cacheManager), userLoader));
        }
    }
}