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

        public void Register(Authenticate authenticate, IUserLoader userLoader)
        {
            var attribute = new MoonlitAuthorizeAttribute(authenticate, userLoader);
            GlobalFilters.Filters.Add(attribute);
        }
    }
}