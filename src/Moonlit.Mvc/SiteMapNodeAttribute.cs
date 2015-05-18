using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Moonlit.Mvc
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Assembly, AllowMultiple = true, Inherited = true)]
    public class SiteMapNodeAttribute : ActionFilterAttribute
    {
        public string Name { get; set; }
        public string Parent { get; set; }
        public string Icon { get; set; }
        public string Text { get; set; }
        public string SiteMap { get; set; }

        public bool IsHidden { get; set; }
        public string Url { get; set; }

        public SiteMapNodeAttribute(string name)
        {
            Name = name;
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var node = MoonlitContext.Current.SiteMaps.FindSiteMapNode(Name, this.SiteMap);
            node.IsCurrent = true;
            MoonlitContext.Current.SiteMaps.CurrentNode = node;

            List<SiteMapNode> nodes = new List<SiteMapNode>();
            do
            {
                nodes.Add(node);
                node.InCurrent = true;
                node = node.ParentNode;
            } while (node.ParentNode != null);  // ignore the root node
            nodes.Reverse();
            MoonlitContext.Current.SiteMaps.Breadcrumb = nodes;
            base.OnActionExecuted(filterContext);
        }
    }
}