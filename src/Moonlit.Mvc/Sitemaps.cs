using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Web;
using System.Web.Compilation;
using System.Web.Mvc;

namespace Moonlit.Mvc
{
    public class ReflectionSitemapsLoader : SitemapsLoader
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

        protected override Sitemaps Create()
        {
            return GlobalSitemaps;
        }
    }
    public abstract class SitemapsLoader
    {
        private Sitemaps _current;

        public Sitemaps Current
        {
            get
            {
                if (_current == null)
                {
                    _current = Create().Clone(HttpContext.Current.User);
                }
                return _current;
            }
        }
        protected abstract Sitemaps Create();
    }
    public class Sitemaps
    {
        private readonly List<SitemapNode> _siteMaps = new List<SitemapNode>();
        public SitemapNode DefaultSiteMap { get; set; }
        public static Sitemaps Current
        {
            get
            {
                var loader = DependencyResolver.Current.GetService<SitemapsLoader>();
                if (loader == null)
                {
                    return null;
                }
                return loader.Current;
            }
        }


        public IEnumerable<SitemapNode> Items
        {
            get { return _siteMaps.AsReadOnly(); }
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