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
        public void MapRequestMappings(RouteCollection route)
        {
            var referencedAssemblies = BuildManager.GetReferencedAssemblies();
            MapRequestMappings(route, referencedAssemblies.Cast<Assembly>());
        }
        public void MapRequestMappings(RouteCollection route, IEnumerable<Assembly> assemblies)
        {
            foreach (Assembly referencedAssembly in assemblies)
            {
                var areaAttr = referencedAssembly.GetCustomAttribute<AreaAttribute>();
                if (areaAttr == null)
                {
                    continue;
                }

                foreach (var exportedType in referencedAssembly.GetExportedTypes())
                {
                    var methodInfos = exportedType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                    foreach (var methodInfo in methodInfos)
                    {
                        var requestMappingAttr = methodInfo.GetCustomAttribute<RequestMappingAttribute>(false);
                        if (requestMappingAttr != null)
                        {
                            var route1 = route.MapRoute(requestMappingAttr.Name,
                                requestMappingAttr.Url ?? "",
                                defaults:
                                    new
                                    {
                                        controller = exportedType.Name.Replace("Controller", ""),
                                        action = methodInfo.Name,

                                    },
                                namespaces: new[] { exportedType.Namespace }
                                );
                            route1.DataTokens["area"] = areaAttr.Area;
                            _requestMappings.Add(new RequestMapping()
                            {
                                Url = requestMappingAttr.Url,
                                Name = requestMappingAttr.Name
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
            return this._requestMappings.FirstOrDefault(x => string.Equals(mappingName, x.Name));
        }
    }
}