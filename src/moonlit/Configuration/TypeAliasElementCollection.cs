using System.Configuration;

namespace Moonlit.Configuration
{
    public class TypeAliasElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new TypeAliasElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            TypeAliasElement ele = element as TypeAliasElement;
            if (ele != null) return ele.Name;
            return "";
        }
    }
}