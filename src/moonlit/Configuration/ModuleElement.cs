using System.Configuration;

namespace Moonlit.Configuration
{
    public class ModuleElement : ConfigurationElement
    {
        [ConfigurationProperty("assembly",
            IsRequired = true,
            IsKey = true)]
        public string Assembly
        {
            get { return (string) this["assembly"]; }
            set { this["assembly"] = value; }
        } 
    }
}