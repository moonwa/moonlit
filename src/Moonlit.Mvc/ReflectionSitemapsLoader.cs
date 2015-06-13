using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Compilation;

namespace Moonlit.Mvc
{
    public class ReflectionSitemapsLoader : ISitemapsLoader
    {
        static ReflectionSitemapsLoader()
        {
            GlobalSitemaps = new Sitemaps();
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
                foreach (var SitemapNode in sitemapNodes.ToList())
                {
                    var parentNode = GlobalSitemaps.FindSitemapNode(SitemapNode.Parent, SitemapNode.SiteMap);
                    if (parentNode != null)
                    {
                        parentNode.Nodes.Add(SitemapNode);
                        sitemapNodes.Remove(SitemapNode);
                    }
                }
            } while (nodeCount != sitemapNodes.Count);
        }

        private static List<SitemapNode> MakeSitemapNodes(IEnumerable<Assembly> assemblies)
        {
            List<SitemapNode> SitemapNodes = new List<SitemapNode>();
            foreach (Assembly referencedAssembly in assemblies)
            {
                var mvcAttr = referencedAssembly.GetCustomAttribute<MvcAttribute>();
                if (mvcAttr == null)
                {
                    continue;
                }
                foreach (var SitemapNodeAttr in referencedAssembly.GetCustomAttributes<SitemapNodeAttribute>())
                {
                    SitemapNodes.Add(new SitemapNode()
                    {
                        IsHidden = SitemapNodeAttr.IsHidden,
                        Text = SitemapNodeAttr.Text,
                        Icon = SitemapNodeAttr.Icon,
                        Parent = SitemapNodeAttr.Parent,
                        Url = SitemapNodeAttr.Url,
                        SiteMap = SitemapNodeAttr.SiteMap ?? GlobalSitemaps.DefaultSiteMap.Name,
                        Name = SitemapNodeAttr.Name,
                    });
                }
                foreach (var exportedType in referencedAssembly.GetExportedTypes())
                {
                    var methodInfos = exportedType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                    foreach (var methodInfo in methodInfos)
                    {
                        var SitemapNodeAttrs = methodInfo.GetCustomAttributes<SitemapNodeAttribute>(false);
                        foreach (var SitemapNodeAttr in SitemapNodeAttrs)
                        {
                            foreach (var requestMappingAttr in methodInfo.GetCustomAttributes<RequestMappingAttribute>())
                            {
                                var url = requestMappingAttr.Url;

                                var SitemapNode = new SitemapNode
                                {
                                    IsHidden = SitemapNodeAttr.IsHidden,
                                    Text = SitemapNodeAttr.Text,
                                    Icon = SitemapNodeAttr.Icon,
                                    Parent = SitemapNodeAttr.Parent,
                                    Url = url,
                                    SiteMap = SitemapNodeAttr.SiteMap ?? GlobalSitemaps.DefaultSiteMap.Name,
                                    Name = SitemapNodeAttr.Name ?? requestMappingAttr.Name,
                                };
                                SitemapNodes.Add(SitemapNode);
                            }
                        }
                    }
                }
            }
            return SitemapNodes;
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
        private static Sitemaps GlobalSitemaps;

        public Sitemaps Create()
        {
            return GlobalSitemaps;
        }
    }
}