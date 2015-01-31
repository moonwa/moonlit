using System.Configuration;

namespace Moonlit.Configuration
{
    public class AssemblyAliasElement : ConfigurationElement
    {
        [ConfigurationProperty("name",
            IsRequired = true,
            IsKey = true)]
        public string Name
        {
            get { return (string) this["name"]; }
            set { this["name"] = value; }
        }

        [ConfigurationProperty("assembly",
            IsRequired = true,
            IsKey = false)]
        public string Assembly
        {
            get { return (string)this["assembly"]; }
            set { this["assembly"] = value; }
        }
    }
}