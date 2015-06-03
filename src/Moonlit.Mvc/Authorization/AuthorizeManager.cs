using System.Web.Mvc;

namespace Moonlit.Mvc.Authorization
{
    public class AuthorizeManager
    {
        public static AuthorizeManager Current { get; private set; }

        static AuthorizeManager()
        {
            Current = new AuthorizeManager();
        }

        public void Register(Authenticate authenticate, IAuthenticateProvider authenticateProvider)
        {
            GlobalFilters.Filters.Add(new MoonlitAuthorizeAttribute(authenticate, authenticateProvider));
            GlobalFilters.Filters.Add(new UserAttribute());
        }
    }
}