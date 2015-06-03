using System;

namespace Moonlit.Mvc.Sitemap
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

        public SitemapNode CreateSiteMap()
        {
            return new SitemapNode
            {
                Text = Text,
                Name = Name,
            };
        }
    }
}