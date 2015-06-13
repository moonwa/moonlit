using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;

namespace Moonlit.Mvc
{
    public class Sitemaps
    {
        private readonly List<SitemapNode> _siteMaps = new List<SitemapNode>();

        public SitemapNode GetSiteMap(string name)
        {
            return _siteMaps.FirstOrDefault(x => string.Equals(x.Name, name, StringComparison.OrdinalIgnoreCase));
        }
        public SitemapNode DefaultSiteMap { get; set; }
        public static Sitemaps Current
        {
            get
            {
                var sitempas = HttpContext.Current.GetObject<Sitemaps>();
                if (sitempas == null)
                {
                    var loader = DependencyResolver.Current.GetService<ISitemapsLoader>(false);
                    if (loader == null)
                    {
                        return null;
                    }

                    sitempas = loader.Create();
                    sitempas = sitempas.Clone(HttpContext.Current.User);

                    HttpContext.Current.SetObject(sitempas);
                }
                return sitempas;
            }
        }

        public SitemapNode CurrentNode { get; set; }
        public List<SitemapNode> Breadcrumb { get; set; }




        public SitemapNode FindSitemapNode(string nodeName, string siteMapName)
        {
            var rootSitemapNode = FindRootSiteMap(siteMapName);
            if (rootSitemapNode == null)
            {
                return null;
            }
            return FindSitemapNode(nodeName, rootSitemapNode);
        }

        private SitemapNode FindRootSiteMap(string siteMapName)
        {
            return string.IsNullOrEmpty(siteMapName) ? DefaultSiteMap : _siteMaps.FirstOrDefault(x => string.Equals(siteMapName, x.Name));
        }

        private SitemapNode FindSitemapNode(string nodeName, SitemapNode node)
        {
            if (string.IsNullOrEmpty(nodeName) || string.Equals(nodeName, node.Name, StringComparison.OrdinalIgnoreCase))
            {
                return node;
            }

            foreach (var child in node.Nodes)
            {
                var foundInChild = FindSitemapNode(nodeName, child);
                if (foundInChild != null)
                {
                    return foundInChild;
                }
            }
            return null;
        }


        public Sitemaps Clone(IPrincipal user)
        {
            Sitemaps other = new Sitemaps();
            foreach (var SitemapNode in _siteMaps)
            {
                var clonedSitemapNode = Clone(user, SitemapNode);
                other._siteMaps.Add(clonedSitemapNode);
                if (SitemapNode == DefaultSiteMap)
                {
                    other.DefaultSiteMap = clonedSitemapNode;
                }
            }
            return other;
        }

        private SitemapNode Clone(IPrincipal user, SitemapNode SitemapNode)
        {
            SitemapNode clonedNode = new SitemapNode
            {
                Icon = SitemapNode.Icon,
                IsHidden = SitemapNode.IsHidden,
                Name = SitemapNode.Name,
                Text = SitemapNode.Text,
                Url = SitemapNode.Url,
                Parent = SitemapNode.Parent,
                SiteMap = SitemapNode.SiteMap,
            };
            foreach (var child in SitemapNode.Nodes)
            {
                var clonedChildren = Clone(user, child);
                clonedChildren.ParentNode = clonedNode;
                clonedNode.Nodes.Add(clonedChildren);
            }
            return clonedNode;
        }

        public void Add(SitemapNode siteMap)
        {
            _siteMaps.Add(siteMap);
        }
    }
}