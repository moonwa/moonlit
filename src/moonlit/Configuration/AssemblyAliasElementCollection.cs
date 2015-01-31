using System.Configuration;

namespace Moonlit.Configuration
{
    public class AssemblyAliasElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new AssemblyAliasElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            AssemblyAliasElement ele = element as AssemblyAliasElement;
            if (ele != null) return ele.Name;
            return "";
        }
    }
}