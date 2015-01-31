using System;
using System.Configuration;

namespace Moonlit.Configuration
{
    public class TypeResolverSection : ConfigurationSection
    {
        [ConfigurationProperty("types",
            IsRequired = false,
            IsKey = false)]
        public TypeAliasElementCollection TypeAlias
        {
            get { return (TypeAliasElementCollection)this["types"]; }
            set { this["types"] = value; }
        }
        [ConfigurationProperty("assemblies",
            IsRequired = false,
            IsKey = false)]
        public AssemblyAliasElementCollection AssemblyAlias
        {
            get { return (AssemblyAliasElementCollection)this["assemblies"]; }
            set { this["assemblies"] = value; }
        }
    }
}