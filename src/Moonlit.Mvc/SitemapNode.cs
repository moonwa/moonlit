using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Moonlit.Mvc
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
        /// �Ƿ����أ���������أ�����ʾ�ڲ˵�����ýڵ㻹�ǻᱻ׷��
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