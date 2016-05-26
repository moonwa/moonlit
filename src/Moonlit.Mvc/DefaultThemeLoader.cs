namespace Moonlit.Mvc
{
    public class DefaultThemeLoader : IThemeLoader
    {
        private readonly string _themeName;
        private readonly ThemeCollection _themes;

        public DefaultThemeLoader(string themeName)
            : this(themeName, ThemeCollection.Current)
        {

        }
        public DefaultThemeLoader(string themeName, ThemeCollection themes)
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