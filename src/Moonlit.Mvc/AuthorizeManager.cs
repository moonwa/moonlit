using System.Web.Mvc;
using Moonlit.Caching;

namespace Moonlit.Mvc
{
    public class AuthorizeManager
    {
        public static void Setup()
        {
  
            GlobalFilters.Filters.Add(new MoonlitAuthenticationAttribute( ));
        }
    }
}