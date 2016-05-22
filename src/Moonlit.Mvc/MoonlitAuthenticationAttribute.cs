using System.Linq;
using System.Security.Principal;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using Moonlit.Caching;

namespace Moonlit.Mvc
{
    public class MoonlitAuthenticationAttribute : FilterAttribute, IAuthenticationFilter
    {
        private readonly ICacheManager _cacheManager;

        public MoonlitAuthenticationAttribute()
        {
            
        }
        public MoonlitAuthenticationAttribute(ICacheManager cacheManager)
        {
            _cacheManager = cacheManager;
        }

        public void OnAuthentication(AuthenticationContext filterContext)
        {
            var authenticate = new Authenticate(_cacheManager??MoonlitDependencyResolver.Current.Resolve<ICacheManager>());
            var session = authenticate.GetSession();
            if (session != null)
            {
                var _userLoader = MoonlitDependencyResolver.Current.Resolve<IUserLoader>();
                var userPrincipal = _userLoader.GetUserPrincipal(session.UserName);

                if (userPrincipal != null)
                {
                    userPrincipal.Privileges =
                        (userPrincipal.Privileges ?? new string[0]).Intersect(session.Privileges ?? new string[0])
                            .ToArray();
                    filterContext.HttpContext.User = userPrincipal;
                }
            }
            else
            {
                filterContext.HttpContext.User = new GenericPrincipal(new GenericIdentity(""), new string[0]);
            }
        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
        }
    }
}