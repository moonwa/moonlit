using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Compilation;
using System.Web.Mvc;
using System.Web.Routing;

namespace Moonlit.Mvc
{
    public class ReflectionSitemapsLoader : ISitemapsLoader
    {
        static ReflectionSitemapsLoader()
        {
            GlobalSitemaps = new SitemapsDefination();
            Register(BuildManager.GetReferencedAssemblies().Cast<Assembly>());
        }
        public static void Register(IEnumerable<Assembly> assemblies)
        {
            assemblies = assemblies.ToList();
            RegisterSiteMaps(assemblies);
            var sitemapNodes = MakeSitemapNodes(assemblies);
            int nodeCount = 0;
            do
            {
                nodeCount = sitemapNodes.Count;
                foreach (var sitemapNode in sitemapNodes.ToList())
                {
                    var parentNode = GlobalSitemaps.FindSitemapNode(sitemapNode.Parent, sitemapNode.Node.SiteMap);
                    if (parentNode != null)
                    {
                        parentNode.Nodes.Add(sitemapNode.Node);
                        sitemapNodes.Remove(sitemapNode);
                    }
                }
            } while (nodeCount != sitemapNodes.Count);
        }

        class SitemapNodeDefinationWithParent
        {
            public string Parent { get; set; }
            public SitemapNodeDefination Node { get; set; }
        }
        private static List<SitemapNodeDefinationWithParent> MakeSitemapNodes(IEnumerable<Assembly> assemblies)
        {
            List<SitemapNodeDefinationWithParent> sitemapNodes = new List<SitemapNodeDefinationWithParent>();
            foreach (Assembly referencedAssembly in assemblies)
            {
                var mvcAttr = referencedAssembly.GetCustomAttribute<MvcAttribute>();
                if (mvcAttr == null)
                {
                    continue;
                }
                foreach (var sitemapNodeAttr in referencedAssembly.GetCustomAttributes<SitemapNodeAttribute>())
                {
                    
                    var node = MakeNode(sitemapNodeAttr, new ConstUrl("#"));

                    sitemapNodes.Add(new SitemapNodeDefinationWithParent() { Node = node, Parent = sitemapNodeAttr.Parent });
                }
                foreach (var exportedType in referencedAssembly.GetExportedTypes())
                {
                    var methodInfos = exportedType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                    foreach (var methodInfo in methodInfos)
                    {
                        var sitemapNodeAttrs = methodInfo.GetCustomAttributes<SitemapNodeAttribute>(false);

                        foreach (var sitemapNodeAttr in sitemapNodeAttrs)
                        {
                            var sitemapNode = MakeNode(sitemapNodeAttr, new RouteUrl(methodInfo.Name, exportedType.Name.Replace("Controller", "")));

                            sitemapNodes.Add(new SitemapNodeDefinationWithParent()
                            {
                                Node = sitemapNode,
                                Parent = sitemapNodeAttr.Parent
                            });
                        }
                    }
                }
            }
            return sitemapNodes;
        }

        private static SitemapNodeDefination MakeNode(SitemapNodeAttribute sitemapNodeAttr, IUrl url )
        {
            var sitemapNode = new SitemapNodeDefination
            {
                IsHidden = sitemapNodeAttr.IsHidden,
                Text = () => sitemapNodeAttr.GetText(),
                Group = () => sitemapNodeAttr.GetGroup(),
                Icon = sitemapNodeAttr.Icon,
                Order = sitemapNodeAttr.Order,
                SiteMap = sitemapNodeAttr.SiteMap ?? GlobalSitemaps.DefaultSiteMap.Name,
            };

            if (string.IsNullOrEmpty(sitemapNodeAttr.Name))
            {
                sitemapNodeAttr.Name = Guid.NewGuid().ToString("N");
            }
            sitemapNode.Name = sitemapNodeAttr.Name;
            sitemapNode.Url = url;
            return sitemapNode;
        }

        private static void RegisterSiteMaps(IEnumerable<Assembly> assemblies)
        {
            foreach (Assembly referencedAssembly in assemblies)
            {
                var mvcAttr = referencedAssembly.GetCustomAttribute<MvcAttribute>();
                if (mvcAttr == null)
                {
                    continue;
                }
                var siteMapAttrs = referencedAssembly.GetCustomAttributes<SitemapAttribute>();
                foreach (var siteMapAttr in siteMapAttrs)
                {
                    var siteMap = siteMapAttr.CreateSiteMap();
                    GlobalSitemaps.DefaultSiteMap = GlobalSitemaps.DefaultSiteMap ?? siteMap;
                    if (siteMapAttr.IsDefault)
                    {
                        GlobalSitemaps.DefaultSiteMap = siteMap;
                    }
                    GlobalSitemaps.Add(siteMap);
                }
            }
        }
        private static SitemapsDefination GlobalSitemaps;

        public Sitemaps Create(RequestContext requestContext)
        {
            Sitemaps sitemaps = new Sitemaps();
            foreach (var siteMapDefination in GlobalSitemaps.SiteMapNodeDefinations)
            {
                var sitemapNode = Create(siteMapDefination, null, requestContext);
                sitemaps.Add(sitemapNode);
                if (siteMapDefination == GlobalSitemaps.DefaultSiteMap)
                {
                    sitemaps.DefaultSiteMap = sitemapNode;
                }
            }
            return sitemaps;
        }

        private SitemapNode Create(SitemapNodeDefination sitemapNodeDefination, SitemapNode parent, RequestContext requestContext)
        {
            SitemapNode node = new SitemapNode
            {
                Icon = sitemapNodeDefination.Icon,
                Name = sitemapNodeDefination.Name,
                Parent = parent,
                Url = sitemapNodeDefination.Url.MakeUrl(requestContext),
                IsCurrent = sitemapNodeDefination.Url.IsCurrent(requestContext),
                Text = sitemapNodeDefination.Text(),
                Group = sitemapNodeDefination.Group(),
                Order = sitemapNodeDefination.Order,
                IsHidden = sitemapNodeDefination.IsHidden,
            };
            foreach (var childNodeDefination in sitemapNodeDefination.Nodes)
            {
                var childNode = Create(childNodeDefination, node, requestContext);
                if (childNode.IsCurrent)
                {
                    childNode.InCurrent = true;
                }
                node.Nodes.Add(childNode);
            }
            node.Nodes = node.Nodes.OrderBy(x => x.Order).ToList();
            return node;
        }
    }

    internal class RouteUrl : IUrl
    {
        private readonly string _action;
        private readonly string _controller;

        public RouteUrl(string action, string controller)
        {
            _action = action;
            _controller = controller;
        }

        #region Implementation of IUrl

        public string MakeUrl(RequestContext requestContext)
        {
            var urlHelper = new UrlHelper(requestContext);
            return urlHelper.Action(_action, _controller);
        }

        public bool IsCurrent(RequestContext requestContext)
        {
            return _controller.EqualsIgnoreCase(requestContext.RouteData.GetRequiredString("controller"))
                   && _action.EqualsIgnoreCase(requestContext.RouteData.GetRequiredString("action"));
        }

        #endregion
    }
}