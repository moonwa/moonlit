using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Web.Compilation;
using System.Web.Mvc;

namespace Moonlit.Mvc
{
    public class SiteMaps
    {
        private readonly List<SiteMapNode> _siteMaps = new List<SiteMapNode>();
        public SiteMapNode DefaultSiteMap { get; private set; }

        static SiteMaps()
        {
            Current = new SiteMaps();
        }

        public static SiteMaps Current { get; private set; }

        public IEnumerable<SiteMapNode> Items
        {
            get { return _siteMaps.AsReadOnly(); }
        }

        public SiteMapNode CurrentNode { get; set; }
        public List<SiteMapNode > Breadcrumb { get; set; }

        internal void MapSiteMaps()
        {
            var referencedAssemblies = BuildManager.GetReferencedAssemblies();
            Register(referencedAssemblies.Cast<Assembly>(), RequestMappings.Current);
        }

        public void Register(IEnumerable<Assembly> assemblies, RequestMappings requestMappings)
        {
            assemblies = assemblies.ToList();
            RegisterSiteMaps(assemblies);
            var siteMapNodes = MakeSiteMapNodes(assemblies);
            int nodeCount = 0;
            do
            {
                nodeCount = siteMapNodes.Count;
                foreach (var siteMapNode in siteMapNodes.ToList())
                {
                    var parentNode = FindSiteMapNode(siteMapNode.Parent, siteMapNode.SiteMap);
                    if (parentNode != null)
                    {
                        parentNode.Nodes.Add(siteMapNode);
                        siteMapNodes.Remove(siteMapNode);
                    }
                }
            } while (nodeCount != siteMapNodes.Count);
        }

        public SiteMapNode FindSiteMapNode(string nodeName, string siteMapName)
        {
            var rootSiteMapNode = FindRootSiteMap(siteMapName);
            if (rootSiteMapNode == null)
            {
                return null;
            }
            return FindSiteMapNode(nodeName, rootSiteMapNode);
        }

        private SiteMapNode FindRootSiteMap(string siteMapName)
        {
            return string.IsNullOrEmpty(siteMapName) ? DefaultSiteMap : _siteMaps.FirstOrDefault(x => string.Equals(siteMapName, x.Name));
        }

        private SiteMapNode FindSiteMapNode(string nodeName, SiteMapNode node)
        {
            if (string.IsNullOrEmpty(nodeName) || string.Equals(nodeName, node.Name, StringComparison.OrdinalIgnoreCase))
            {
                return node;
            }

            foreach (var child in node.Nodes)
            {
                var foundInChild = FindSiteMapNode(nodeName, child);
                if (foundInChild != null)
                {
                    return foundInChild;
                }
            }
            return null;
        }

        private List<SiteMapNode> MakeSiteMapNodes(IEnumerable<Assembly> assemblies)
        {
            List<SiteMapNode> siteMapNodes = new List<SiteMapNode>();
            foreach (Assembly referencedAssembly in assemblies)
            {
                var mvcAttr = referencedAssembly.GetCustomAttribute<MvcAttribute>();
                if (mvcAttr == null)
                {
                    continue;
                }
                foreach (var siteMapNodeAttr in referencedAssembly.GetCustomAttributes<SiteMapNodeAttribute>())
                {
                    siteMapNodes.Add(new SiteMapNode()
                    {
                        IsHidden = siteMapNodeAttr.IsHidden,
                        Text = siteMapNodeAttr.Text,
                        Icon = siteMapNodeAttr.Icon,
                        Parent = siteMapNodeAttr.Parent,
                        Url = siteMapNodeAttr.Url,
                        SiteMap = siteMapNodeAttr.SiteMap ?? DefaultSiteMap.Name,
                        Name = siteMapNodeAttr.Name,
                    });
                }
                foreach (var exportedType in referencedAssembly.GetExportedTypes())
                {
                    var methodInfos = exportedType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                    foreach (var methodInfo in methodInfos)
                    {
                        var siteMapNodeAttrs = methodInfo.GetCustomAttributes<SiteMapNodeAttribute>(false);
                        foreach (var siteMapNodeAttr in siteMapNodeAttrs)
                        {
                            foreach (var requestMappingAttr in methodInfo.GetCustomAttributes<RequestMappingAttribute>())
                            {
                                var url = requestMappingAttr.Url;

                                var siteMapNode = new SiteMapNode
                                {
                                    IsHidden = siteMapNodeAttr.IsHidden,
                                    Text = siteMapNodeAttr.Text,
                                    Icon = siteMapNodeAttr.Icon,
                                    Parent = siteMapNodeAttr.Parent,
                                    Url = url,
                                    SiteMap = siteMapNodeAttr.SiteMap ?? DefaultSiteMap.Name,
                                    Name = siteMapNodeAttr.Name ?? requestMappingAttr.Name,
                                };
                                siteMapNodes.Add(siteMapNode);
                            }
                        }
                    }
                }
            }
            return siteMapNodes;
        }

        private void RegisterSiteMaps(IEnumerable<Assembly> assemblies)
        {
            foreach (Assembly referencedAssembly in assemblies)
            {
                var mvcAttr = referencedAssembly.GetCustomAttribute<MvcAttribute>();
                if (mvcAttr == null)
                {
                    continue;
                }
                var siteMapAttrs = referencedAssembly.GetCustomAttributes<SiteMapAttribute>();
                foreach (var siteMapAttr in siteMapAttrs)
                {
                    var siteMap = siteMapAttr.CreateSiteMap();
                    DefaultSiteMap = DefaultSiteMap ?? siteMap;
                    if (siteMapAttr.IsDefault)
                    {
                        DefaultSiteMap = siteMap;
                    }
                    _siteMaps.Add(siteMap);
                }
            }
        }

        public SiteMaps Clone(IPrincipal user)
        {
            SiteMaps other = new SiteMaps();
            foreach (var siteMapNode in _siteMaps)
            {
                var clonedSiteMapNode = Clone(user, siteMapNode);
                other._siteMaps.Add(clonedSiteMapNode);
                if (siteMapNode == DefaultSiteMap)
                {
                    other.DefaultSiteMap = clonedSiteMapNode;
                }
            }
            return other;
        }

        private SiteMapNode Clone(IPrincipal user, SiteMapNode siteMapNode)
        {
            SiteMapNode clonedNode = new SiteMapNode
            {
                Icon = siteMapNode.Icon,
                IsHidden = siteMapNode.IsHidden,
                Name = siteMapNode.Name,
                Text = siteMapNode.Text,
                Url = siteMapNode.Url,
                Parent = siteMapNode.Parent, 
                SiteMap = siteMapNode.SiteMap,
            };
            foreach (var child in siteMapNode.Nodes)
            {
                var clonedChildren = Clone(user, child);
                clonedChildren.ParentNode = clonedNode;
                clonedNode.Nodes.Add(clonedChildren);
            }
            return clonedNode;
        }
    }
}