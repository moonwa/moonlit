using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Moonlit.Mvc
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Assembly, AllowMultiple = true, Inherited = true)]
    public class SitemapNodeAttribute : ActionFilterAttribute
    {
        public string Name { get; set; }
        public string Parent { get; set; }
        public string Icon { get; set; }
        public string Text { get; set; }
        public string SiteMap { get; set; }

        public bool IsHidden { get; set; }
        public string Url { get; set; }

        public SitemapNodeAttribute(string name)
        {
            Name = name;
            Order = 2;
        }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            var sitemaps = Sitemaps.Current;
            var node = sitemaps.FindSitemapNode(Name, this.SiteMap);
            node.IsCurrent = true;
            sitemaps.CurrentNode = node;

            List<SitemapNode> nodes = new List<SitemapNode>();
            do
            {
                nodes.Add(node);
                node.InCurrent = true;
                node = node.ParentNode;
            } while (node.ParentNode != null);  // ignore the root node
            nodes.Reverse();
            sitemaps.Breadcrumb = nodes;
            base.OnResultExecuting(filterContext);
        }

    }
}