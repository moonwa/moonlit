using System.Collections.Generic;

namespace Moonlit.Mvc
{
    public class Themes
    {
        private Dictionary<string, Theme> _themes = new Dictionary<string, Theme>();
        private Theme _defaultTheme;
        public void Register(Theme theme)
        {
            _themes[theme.Name] = theme;
        }
        public void RegisterDefault(Theme theme)
        {
            _defaultTheme = theme;
        }

        public Theme GetTheme(string name)
        {
            if (name == null)
            {
                return _defaultTheme;
            }
            return _themes[name];
        }

        static Themes()
        {
            Current = new Themes();
        }
        public static Themes Current { get; private set; }
    }
}