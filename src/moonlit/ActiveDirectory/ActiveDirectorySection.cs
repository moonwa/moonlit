using System.Configuration;

namespace Moonlit.ActiveDirectory
{
    /// <summary>
    /// 
    /// </summary>
    public class ActiveDirectorySection : ConfigurationSection
    { 
        [ConfigurationProperty("directorys", IsRequired = true)]
        public ActiveDirectoryElements Directorys
        {
            get { return this["directorys"] as ActiveDirectoryElements; }
            set { this["directorys"] = value; }
        }
    }
}