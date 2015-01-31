using Moonlit.Caching;
using Moonlit.CastleExtensions.Caching;

namespace Moonlit.CastleExtensions.TextFixtures.Caching
{
    public class Person : IPerson
    {
        public string Name { get; set; }

        [CacheQuery("Q")]
        public string GetName()
        {
            return Name;
        }

        public void SetName(string value)
        {
            Name = value;
        }

        [CacheRefresh("Q")]
        public void Refresh()
        {
        }
    }
}