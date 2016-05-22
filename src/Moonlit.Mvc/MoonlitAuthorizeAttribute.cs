using System;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace Moonlit.Mvc
{
    public class MoonlitAuthorizeAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new MyHttpUnauthorizedResult();
        }


        public class MyHttpUnauthorizedResult : ActionResult
        {
            public override void ExecuteResult(ControllerContext context)
            {
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                context.HttpContext.Response.Redirect(RedirectUrl);
            }
        }

        private static string _redirectUrl = "/signin";

        public static string RedirectUrl
        {
            get { return _redirectUrl; }
            set { _redirectUrl = value; }
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