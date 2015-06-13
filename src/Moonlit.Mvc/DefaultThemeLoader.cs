namespace Moonlit.Mvc
{
    public class DefaultThemeLoader : IThemeLoader
    {
        private readonly string _themeName;
        private readonly Themes _themes;

        public DefaultThemeLoader(string themeName)
            : this(themeName, Themes.Current)
        {

        }
        public DefaultThemeLoader(string themeName, Themes themes)
        {
            _themeName = themeName;
            _themes = themes;
        }

        public Theme Load()
        {
            return _themes.GetTheme(_themeName);
        }
    }
}