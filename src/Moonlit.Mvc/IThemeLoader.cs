namespace Moonlit.Mvc
{
    public class DefaultThemeLoader : IThemeLoader
    {
        private readonly string _themeName;
        private readonly Themes _themes;
        private Theme _theme;

        public DefaultThemeLoader(string themeName, Themes themes)
        {
            _themeName = themeName;
            _themes = themes;
        }

        public Theme Theme
        {
            get
            {
                if (_theme == null)
                {
                    _theme = _themes.GetTheme(_themeName);
                }
                return _theme;
            }
        }
    }
    public interface IThemeLoader
    {
        Theme Theme { get; }
    }
}