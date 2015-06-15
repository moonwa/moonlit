using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

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

                    var httpContext = new HttpContextWrapper(HttpContext.Current);
                    var routeData = RouteTable.Routes.GetRouteData(httpContext);
                    var requestContext = new RequestContext(httpContext, routeData);
                    sitempas = loader.Create(requestContext);
                    sitempas.Filter(HttpContext.Current.User, requestContext);

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


        public void Filter(IPrincipal user, RequestContext requestContext)
        {
            foreach (var sitemapNode in _siteMaps)
            {
                Filter(user, sitemapNode, requestContext);
            }
        }

        private void Filter(IPrincipal user, SitemapNode sitemapNode, RequestContext requestContext)
        {
            foreach (var child in sitemapNode.Nodes)
            {
                Filter(user, child, requestContext);
            }
        }

        public void Add(SitemapNode siteMap)
        {
            _siteMaps.Add(siteMap);
        }
    }
}