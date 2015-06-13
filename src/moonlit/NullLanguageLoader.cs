namespace Moonlit
{
    public class NullLanguageLoader : ILanguageLoader
    {
        public string Get(string key, string culture)
        {
            return key;
        }
    }
}