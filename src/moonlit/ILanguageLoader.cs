namespace Moonlit
{
    public interface ILanguageLoader
    {
        string Get(string key, string culture);
    }
}