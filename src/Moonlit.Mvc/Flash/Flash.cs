using System.Web.Mvc;

namespace Moonlit.Mvc.Flash
{
    public static class Flash
    {
        public static IFlash Current { get; set; }

        public static void Register()
        {
            if (Current == null)
            {
                Current = DependencyResolver.Current.GetService<IFlash>();
            }
            GlobalFilters.Filters.Add(new FlashAttribute());
        }
    }
}