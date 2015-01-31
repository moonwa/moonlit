namespace Moonlit.CastleExtensions.TextFixtures.Caching
{
    public interface IPerson
    {
        string GetName();
        void SetName(string value);
        void Refresh();
    }
}
