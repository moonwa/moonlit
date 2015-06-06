using System.Collections.Generic;
using System.Web.Mvc;

namespace Moonlit.Mvc.Themes
{
    public class Themes
    {
        private Dictionary<string, Theme> _themes = new Dictionary<string, Theme>();

        static Themes()
        {
            GlobalFilters.Filters.Add(new ThemeAttribute());
        }
        public void Register(Theme theme)
        {
            _themes[theme.Name] = theme;
        }

        public Theme GetTheme(string name)
        {
            Theme theme;
            if (_themes.TryGetValue(name, out theme))
            {
                return theme;
            }
            return null;
        }
    }
}