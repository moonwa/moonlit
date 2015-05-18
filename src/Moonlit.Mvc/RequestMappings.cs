using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Compilation;
using System.Web.Mvc;
using System.Web.Routing;

namespace Moonlit.Mvc
{
    public class RequestMappings
    {
        private readonly List<RequestMapping> _requestMappings = new List<RequestMapping>();

        static RequestMappings()
        {
            Current = new RequestMappings();
        }

        public static RequestMappings Current { get; private set; }
        public void MapRequestMappings(RouteCollection routes)
        {
            var referencedAssemblies = BuildManager.GetReferencedAssemblies();
            MapRequestMappings(routes, referencedAssemblies.Cast<Assembly>());
        }

        public void MapRequestMappings(RouteCollection routes, IEnumerable<Assembly> assemblies)
        {
            foreach (Assembly referencedAssembly in assemblies)
            {
                var mvcAttr = referencedAssembly.GetCustomAttribute<MvcAttribute>();
                if (mvcAttr == null)
                {
                    continue;
                }

                foreach (var exportedType in referencedAssembly.GetExportedTypes())
                {
                    var typeAttr = exportedType.GetCustomAttribute<RequestMappingAttribute>(true);
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
                            _requestMappings.Add(new RequestMapping()
                            {
                                Name = requestMappingAttr.Name,
                            });
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 获取指定的 <see cref="RequestMapping"/>
        /// </summary>
        /// <param name="mappingName"></param>
        /// <returns></returns>
        public RequestMapping GetRequestMapping(string mappingName)
        {
            if (string.IsNullOrEmpty(mappingName))
            {
                return null;
            }
            return this._requestMappings.FirstOrDefault(x => string.Equals(mappingName, x.Name, StringComparison.OrdinalIgnoreCase));
        }
    }
}