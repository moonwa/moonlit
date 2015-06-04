using System.Web.Mvc;

namespace Moonlit.Mvc.Flash
{
    public static class Flash
    { 

        public static void Register()
        { 
            GlobalFilters.Filters.Add(new FlashAttribute());
        }
    }
}