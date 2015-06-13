using System.Collections.Generic;
using System.Web.Mvc;

namespace Moonlit.Mvc
{
    public class Themes
    {
        private Dictionary<string, Theme> _themes = new Dictionary<string, Theme>();
        public static Themes Current { get; private set; }
        static Themes()
        {
            Current = new Themes();
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