using System.Configuration;

namespace Moonlit.ActiveDirectory
{
    public class ActiveDirectoryElement : ConfigurationElement
    {
        [ConfigurationProperty("baseUrl", IsKey = true, IsRequired = true)]
        public string BaseUrl
        {
            get
            {
                return this["baseUrl"] as string;
            }
            set
            {
                this["baseUrl"] = value;
            }
        }

        [ConfigurationProperty("user")]
        public string User
        {
            get
            {
                return this["user"] as string;
            }
            set
            {
                this["user"] = value;
            }
        }
        [ConfigurationProperty("password")]
        public string Password
        {
            get
            {
                return this["password"] as string;
            }
            set
            {
                this["password"] = value;
            }
        }

        [ConfigurationProperty("isEnabled",
            DefaultValue = true,
            IsRequired = false,
            IsKey = false)]
        public bool IsEnabled
        {
            get { return (bool)this["isEnabled"]; }
            set { this["isEnabled"] = value; }
        }
    }
}