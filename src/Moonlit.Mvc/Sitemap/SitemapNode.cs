using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Moonlit.Mvc.Sitemap
{
    [DebuggerDisplay("{Name}: {Text}")]
    public class SitemapNode
    {
        internal string Parent { get; set; }
        public string Icon { get; set; }
        public string Text { get; set; }
        public string Url { get; set; }
        public string SiteMap { get; set; }
        public string Name { get; set; }
        public List<SitemapNode> Nodes { get; set; }
        public IEnumerable<SitemapNode> VisibleNodes { get { return this.Nodes.Where(x => !x.IsHidden); } }
        /// <summary>
        /// 是否隐藏，如果是隐藏，则不显示在菜单项，但该节点还是会被追踪
        /// </summary>
        public bool IsHidden { get; set; }

       

        public SitemapNode ParentNode { get; set; }
        public bool IsCurrent { get; set; }
        public bool InCurrent { get; set; }

        public SitemapNode()
        {
            Nodes = new List<SitemapNode>();
        }
    }
}