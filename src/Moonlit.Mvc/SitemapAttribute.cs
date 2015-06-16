using System;

namespace Moonlit.Mvc
{
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true, Inherited = true)]
    public class SitemapAttribute : Attribute
    {
        private string _text;
        public string Name { get; set; }

        public string Text
        {
            get { throw new Exception("Please get text via GetText"); }
            set { _text = value; }
        }

        public SitemapAttribute(string name)
        {
            Name = name;
        }

        public Type ResourceType { get; set; }
        public bool IsDefault { get; set; }

        public string GetText()
        {
            if (ResourceType != null)
            {
                return EntityAccessor.GetAccessor(ResourceType).GetProperty(null, _text) as string;
            }
            return _text;
        }
        public SitemapNodeDefination CreateSiteMap()
        {
            return new SitemapNodeDefination
            {
                Text = () => GetText(),
                Group = () => "",
                Name = Name,
                Url = new ConstUrl("#"),
            };
        }
    }
}