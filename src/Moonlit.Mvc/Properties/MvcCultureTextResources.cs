namespace Moonlit.Mvc.Properties
{
    public static class MvcCultureTextResources
    {
        public static string Get(string key)
        {
            return LanguageLoader.Get(key);
        }
        public static ILanguageLoader LanguageLoader
        {
            get { return Moonlit.Properties.MoonlitCultureTextResources.LanguageLoader; }
        }
    }
}
