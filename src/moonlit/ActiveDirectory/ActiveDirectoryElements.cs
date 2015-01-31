using System.Configuration;

namespace Moonlit.ActiveDirectory
{
    /// <summary>
    /// 
    /// </summary>
    public class ActiveDirectoryElements : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ActiveDirectoryElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ActiveDirectoryElement)element).BaseUrl;
        }

        public new ActiveDirectoryElement this[string ad]
        {
            get
            {
                return BaseGet(ad) as ActiveDirectoryElement;
            }
            set
            {
                base.BaseAdd(value);
            }
        }
    }
}