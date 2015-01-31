using System.Configuration;

namespace Moonlit.ActiveDirectory
{
    /// <summary>
    /// 
    /// </summary>
    public class DefaultUserElement : ConfigurationElement
    {
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
        [ConfigurationProperty("encrypted", DefaultValue = false)]
        public bool Encrypted
        {
            get
            {
                return (bool)this["encrypted"];
            }
            set
            {
                this["encrypted"] = value;
            }
        }
    }
}