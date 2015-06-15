using System.Diagnostics;
using System.Linq;
using System.Web;
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
            var moonlitAuthorizeAttribute = new MoonlitAuthorizeAttribute(new Authenticate(cacheManager), userLoader)
            {
                Order = 100,
            };
            GlobalFilters.Filters.Add(moonlitAuthorizeAttribute);
        }
    }
    public class MoonlitAuthorizeAttribute : AuthorizeAttribute
    {
        private readonly Authenticate _authenticate;
        private readonly IUserLoader _userLoader;

        public MoonlitAuthorizeAttribute(Authenticate authenticate, IUserLoader userLoader)
        {
            _authenticate = authenticate;
            _userLoader = userLoader;
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.User != null)
            {
                var session = _authenticate.GetSession();
                if (session != null)
                {
                    var user = _userLoader.GetUserPrincipal(session.UserName);

                    if (user != null)
                    {
                        user.Privileges = (user.Privileges ?? new string[0]).Intersect(session.Privileges ?? new string[0]).ToArray();
                        filterContext.HttpContext.User = user;
                    }
                }
            }
        }
         
    }
}