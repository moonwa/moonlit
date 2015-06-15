using System;

namespace Moonlit.Mvc
{
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true, Inherited = true)]
    public class SitemapAttribute : Attribute
    {
        public string Name { get; set; }
        public string Text { get; set; }

        public SitemapAttribute(string name)
        {
            Name = name;
        }

        public bool IsDefault { get; set; }

        public SitemapNodeDefination CreateSiteMap()
        {
            return new SitemapNodeDefination
            {
                Text = Text,
                Name = Name,
                Url = new ConstUrl("#"),
            };
        }
    }
}