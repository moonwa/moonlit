using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;

namespace Moonlit.Mvc
{
    public class MoonlitAuthorizationAttribute : FilterAttribute, IAuthenticationFilter
    {
        private readonly Authenticate _authenticate;
        public MoonlitAuthorizationAttribute(Authenticate authenticate)
        {
            _authenticate = authenticate;
        }
        public void OnAuthentication(AuthenticationContext filterContext)
        {
            if (filterContext.HttpContext.User != null)
            {
                var session = _authenticate.GetSession();
                if (session != null)
                {
                    var _userLoader = DependencyResolver.Current.GetService<IUserLoader>();
                    var userPrincipal = _userLoader.GetUserPrincipal(session.UserName);

                    if (userPrincipal != null)
                    {
                        userPrincipal.Privileges = (userPrincipal.Privileges ?? new string[0]).Intersect(session.Privileges ?? new string[0]).ToArray();
                        filterContext.HttpContext.User = userPrincipal;
                    }
                }
            }
        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
        }
    }
    //    public class MoonlitAuthorizeAttribute : AuthorizeAttribute
    //    {
    //        private readonly Authenticate _authenticate;
    //        private readonly IUserLoader _userLoader;
    //
    //        public MoonlitAuthorizeAttribute(Authenticate authenticate, IUserLoader userLoader)
    //        {
    //            _authenticate = authenticate;
    //            _userLoader = userLoader;
    //        }
    //
    //        public override void OnAuthorization(AuthorizationContext filterContext)
    //        {
    //            if (filterContext.HttpContext.User != null)
    //            {
    //                var session = _authenticate.GetSession();
    //                if (session != null)
    //                {
    //                    var user = _userLoader.GetUserPrincipal(session.UserName);
    //
    //                    if (user != null)
    //                    {
    //                        user.Privileges = (user.Privileges ?? new string[0]).Intersect(session.Privileges ?? new string[0]).ToArray();
    //                        filterContext.HttpContext.User = user;
    //                    }
    //                }
    //            }
    //        }
    //         
    //    }
}