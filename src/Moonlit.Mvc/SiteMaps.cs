using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Compilation;
using System.Web.Mvc;

namespace Moonlit.Mvc
{
    public class SiteMaps
    {
        private readonly List<SiteMap> _siteMaps = new List<SiteMap>();
        private SiteMap _defaultSiteMap;
        static SiteMaps()
        {
            Current = new SiteMaps();
        }

        public static SiteMaps Current { get; private set; }

        public IEnumerable<SiteMap> Items
        {
            get { return _siteMaps.AsReadOnly(); }
        }

        public void MapSiteMaps()
        {
            var referencedAssemblies = BuildManager.GetReferencedAssemblies();
            MapRequestMappings(referencedAssemblies.Cast<Assembly>());
        }

        public void MapRequestMappings(IEnumerable<Assembly> assemblies)
        {
            foreach (Assembly referencedAssembly in assemblies)
            {
                var mvcAttr = referencedAssembly.GetCustomAttribute<MvcAttribute>();
                if (mvcAttr == null)
                {
                    continue;
                }
                var siteMapAttr = referencedAssembly.GetCustomAttribute<SiteMapAttribute>();
                _defaultSiteMap = _defaultSiteMap ?? siteMapAttr.CreateSiteMap();
            }

            foreach (Assembly referencedAssembly in assemblies)
            {
                var mvcAttr = referencedAssembly.GetCustomAttribute<MvcAttribute>();
                if (mvcAttr == null)
                {
                    continue;
                }

                foreach (var exportedType in referencedAssembly.GetExportedTypes())
                {
                    var typeAttr = exportedType.GetCustomAttribute<SiteMapNodeAttribute>(true);
                    if (typeAttr != null && string.IsNullOrWhiteSpace(typeAttr.Url))
                    {
                        throw new Exception("the requestMappintAttribute on controller have to with url");
                    }

                    var methodInfos = exportedType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                    foreach (var methodInfo in methodInfos)
                    {
                        var requestMappingAttr = methodInfo.GetCustomAttribute<RequestMappingAttribute>(false);
                        if (requestMappingAttr != null)
                        {
                            var url = requestMappingAttr.Url ?? "";
                            if (typeAttr != null)
                            {
                                url = typeAttr.Url + "/" + url;
                            }
                            var route = routes.MapRoute(requestMappingAttr.Name,
                                url,
                                defaults:
                                    new
                                    {
                                        controller = exportedType.Name.Replace("Controller", ""),
                                        action = methodInfo.Name,

                                    },
                                namespaces: new[] { exportedType.Namespace }
                                );
                            if (mvcAttr.Module != null)
                            {
                                route.DataTokens["area"] = mvcAttr.Module;
                            }
                            _siteMaps.Add(new RequestMapping()
                            {
                                Name = requestMappingAttr.Name
                            });
                        }
                    }
                }
            }
        }


    }
}