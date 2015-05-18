using System;

namespace Moonlit.Mvc
{
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true, Inherited = true)]
    public class SiteMapAttribute : Attribute
    {
        public string Name { get; set; }
        public string Text { get; set; }

        public SiteMapAttribute(string name)
        {
            Name = name;
        }

        public bool IsDefault { get; set; }

        public SiteMapNode CreateSiteMap()
        {
            return new SiteMapNode
            {
                Text = Text,
                Name = Name,
            };
        }
    }
}