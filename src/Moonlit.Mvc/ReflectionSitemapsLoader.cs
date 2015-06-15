using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Compilation;
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
                    var node = MakeNode(sitemapNodeAttr, null);

                    sitemapNodes.Add(new SitemapNodeDefinationWithParent() { Node = node, Parent = sitemapNodeAttr.Parent });
                }
                foreach (var exportedType in referencedAssembly.GetExportedTypes())
                {
                    var methodInfos = exportedType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                    foreach (var methodInfo in methodInfos)
                    {
                        var sitemapNodeAttrs = methodInfo.GetCustomAttributes<SitemapNodeAttribute>(false);
                        var info = methodInfo as ICustomAttributeProvider;

                        foreach (var sitemapNodeAttr in sitemapNodeAttrs)
                        {
                            var sitemapNode = MakeNode(sitemapNodeAttr, info);

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

        private static SitemapNodeDefination MakeNode(SitemapNodeAttribute sitemapNodeAttr, ICustomAttributeProvider info)
        {
            var sitemapNode = new SitemapNodeDefination
            {
                IsHidden = sitemapNodeAttr.IsHidden,
                Text = () => sitemapNodeAttr.GetText(),
                Icon = sitemapNodeAttr.Icon,
                SiteMap = sitemapNodeAttr.SiteMap ?? GlobalSitemaps.DefaultSiteMap.Name,
            };

            if (info != null)
            {
                var namedAttr = info.GetCustomAttributes(false).OfType<INamed>().FirstOrDefault();
                if (namedAttr != null)
                {
                    sitemapNodeAttr.Name = namedAttr.Name;
                }
            }
            if (string.IsNullOrEmpty(sitemapNodeAttr.Name))
            {
                sitemapNodeAttr.Name = Guid.NewGuid().ToString("N");
            }
            sitemapNode.Name = sitemapNodeAttr.Name;

            if (info != null)
            {
                var urlAttr = info.GetCustomAttributes(false).OfType<IUrl>().FirstOrDefault();
                if (urlAttr != null)
                {
                    sitemapNode.Url = urlAttr;
                }
            }
            sitemapNode.Url = sitemapNode.Url ?? new ConstUrl("#");
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
                Text = sitemapNodeDefination.Text(),
                IsHidden = sitemapNodeDefination.IsHidden,
            };
            foreach (var childNodeDefination in sitemapNodeDefination.Nodes)
            {
                node.Nodes.Add(Create(childNodeDefination, node, requestContext));
            }
            return node;
        }
    }
}