using System;
using System.Collections.Generic;
using System.Linq;

namespace Moonlit.Mvc
{
    class SitemapsDefination
    {
        private readonly List<SitemapNodeDefination> _siteMaps = new List<SitemapNodeDefination>();

        public IEnumerable<SitemapNodeDefination> SiteMapNodeDefinations
        {
            get
            {
                return _siteMaps;
            }
        }
        public SitemapNodeDefination GetSiteMap(string name)
        {
            return _siteMaps.FirstOrDefault(x => string.Equals(x.Name, name, StringComparison.OrdinalIgnoreCase));
        }
        public SitemapNodeDefination DefaultSiteMap { get; set; }

        public SitemapNodeDefination FindSitemapNode(string nodeName, string siteMapName)
        {
            var rootSitemapNode = FindRootSiteMap(siteMapName);
            if (rootSitemapNode == null)
            {
                return null;
            }
            return FindSitemapNode(nodeName, rootSitemapNode);
        }

        private SitemapNodeDefination FindRootSiteMap(string siteMapName)
        {
            return string.IsNullOrEmpty(siteMapName) ? DefaultSiteMap : _siteMaps.FirstOrDefault(x => string.Equals(siteMapName, x.Name));
        }

        private SitemapNodeDefination FindSitemapNode(string nodeName, SitemapNodeDefination node)
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
        public void Add(SitemapNodeDefination siteMap)
        {
            _siteMaps.Add(siteMap);
        }
    }
}