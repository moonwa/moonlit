using System.Configuration;

namespace Moonlit.Configuration
{
    public class TypeAliasElement : ConfigurationElement
    {
        [ConfigurationProperty("name",
            IsRequired = true,
            IsKey = true)]
        public string Name
        {
            get { return (string) this["name"]; }
            set { this["name"] = value; }
        }

        [ConfigurationProperty("type",
            IsRequired = true,
            IsKey = false)]
        public string Type
        {
            get { return (string) this["type"]; }
            set { this["type"] = value; }
        }
    }
}