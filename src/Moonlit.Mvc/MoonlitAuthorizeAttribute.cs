using System.Linq;
using System.Web.Mvc;

namespace Moonlit.Mvc
{
    public class AuthenticationManager
    {
        public static AuthenticationManager Current { get; private set; }

        static AuthenticationManager()
        {
            Current = new AuthenticationManager();
        }

        public void Register(Authenticate authenticate, IAuthenticateProvider authenticateProvider)
        {
            var attribute = new MoonlitAuthorizeAttribute(authenticate, authenticateProvider);
            GlobalFilters.Filters.Add(attribute);
        }
    }
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