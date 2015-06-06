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
}