using System.Linq;
using System.Web.Mvc;

namespace Moonlit.Mvc
{
    public class MoonlitAuthorizeAttribute : AuthorizeAttribute
    {
        private readonly Authenticate _authenticate;
        private readonly IAuthenticateProvider _authenticateProvider;

        public MoonlitAuthorizeAttribute(Authenticate authenticate, IAuthenticateProvider authenticateProvider)
        {
            _authenticate = authenticate;
            _authenticateProvider = authenticateProvider;
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.User != null)
            {
                var session = _authenticate.GetSession();
                if (session != null)
                {
                    var user = _authenticateProvider.GetUserPrincipal(session.UserName);

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