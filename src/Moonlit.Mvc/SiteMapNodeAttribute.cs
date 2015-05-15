using System;

namespace Moonlit.Mvc
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Assembly, AllowMultiple = true, Inherited = true)]
    public class SiteMapNodeAttribute : Attribute
    {
        public string Name { get; set; }
        public string Parent { get; set; }
        public string Icon { get; set; }

        public SiteMapNodeAttribute(string name)
        {
            Name = name;
        }
    }
}