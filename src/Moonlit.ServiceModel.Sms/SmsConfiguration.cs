using System.Configuration;

namespace Moonlit.ServiceModel.Sms
{
    public class SmsConfiguration : ConfigurationSection
    {
        [ConfigurationProperty("port", IsRequired = true)]
        public int Port
        {
            get { return (int)base["port"]; }
            set { base["port"] = value; }
        }
        [ConfigurationProperty("host", IsRequired = true)]
        public string Host
        {
            get { return (string)base["host"]; }
            set { base["host"] = value; }
        }
        [ConfigurationProperty("password", IsRequired = true)]
        public string Password
        {
            get { return (string)base["password"]; }
            set { base["password"] = value; }
        }
        [ConfigurationProperty("username", IsRequired = true)]
        public string UserName
        {
            get { return (string)base["username"]; }
            set { base["username"] = value; }
        }
        [ConfigurationProperty("queueCount", IsRequired = false, DefaultValue = "1")]
        public int QueueCount
        {
            get { return (int)base["queueCount"]; }
            set { base["queueCount"] = value; }
        }
    }
}
