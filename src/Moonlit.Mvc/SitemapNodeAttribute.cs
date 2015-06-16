using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Moonlit.Mvc
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Assembly, AllowMultiple = true, Inherited = true)]
    public class SitemapNodeAttribute : ActionFilterAttribute
    {
        private string _text;
        public string Name { get; set; }
        public string Parent { get; set; }
        public string Icon { get; set; }

        public string Text
        {
            get { throw new Exception("Please get Text via GetText"); }
            set { _text = value; }
        }

        private string _group;
        public string Group
        {
            get { throw new Exception("Please get Group via GetText"); }
            set { _group = value; }
        }

        public string GetText()
        {
            if (string.IsNullOrEmpty(_text))
            {
                return null;
            }
            if (ResourceType != null)
            {
                return EntityAccessor.GetAccessor(ResourceType).GetProperty(null, _text) as string;
            }
            return _text;
        }
        public string GetGroup()
        {
            if (string.IsNullOrEmpty(_group))
            {
                return null;
            }
            if (ResourceType != null)
            {
                return EntityAccessor.GetAccessor(ResourceType).GetProperty(null, _group) as string;
            }
            return _group;
        }
        public string SiteMap { get; set; }
        public bool IsHidden { get; set; }
        public Type ResourceType { get; set; }
        public SitemapNodeAttribute()
        {
            Order = 2;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (string.IsNullOrEmpty(Name))
            {
                var name = filterContext.ActionDescriptor.GetCustomAttributes(false).OfType<INamed>().FirstOrDefault();
                if (name != null)
                {
                    this.Name = name.Name;
                }
            }
            base.OnActionExecuting(filterContext);
        }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {

            var sitemaps = Sitemaps.Current;
            if (sitemaps == null || string.IsNullOrEmpty(Name))
            {
                return;
            }
            var node = sitemaps.FindSitemapNode(Name, this.SiteMap);
            node.IsCurrent = true;
            sitemaps.CurrentNode = node;

            List<SitemapNode> nodes = new List<SitemapNode>();
            do
            {
                nodes.Add(node);
                node.InCurrent = true;
                node = node.Parent;
            } while (node != null && node.Parent != null);  // ignore the root node
            nodes.Reverse();
            sitemaps.Breadcrumb = nodes;
            base.OnResultExecuting(filterContext);
        }

    }
}