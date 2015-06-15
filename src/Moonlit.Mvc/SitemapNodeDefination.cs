using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Moonlit.Mvc
{
    [DebuggerDisplay("{Name}: {Text}")]
    public class SitemapNodeDefination
    {
        public string Icon { get; set; }
        public Func<string> Text { get; set; }
        public IUrl Url { get; set; }
        public string SiteMap { get; set; }
        public string Name { get; set; }
        public List<SitemapNodeDefination> Nodes { get; set; }
        /// <summary>
        /// </summary>
        public bool IsHidden { get; set; }

        public SitemapNodeDefination ParentNode { get; set; }

        public SitemapNodeDefination()
        {
            Nodes = new List<SitemapNodeDefination>();
        }
    }
}