using Moonlit.Caching;

namespace Moonlit.Proxy.TextFixtures.Caching
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