using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Compilation;
using System.Web.Mvc;
using System.Web.Routing;

namespace Moonlit.Mvc
{
    public class RequestRoutes
    {
        private List<RouteItem> _routes = new List<RouteItem>();
        public void MapRequestMappings(RouteCollection route)
        {
            var referencedAssemblies = BuildManager.GetReferencedAssemblies();
            MapRequestMappings(route, referencedAssemblies.Cast<Assembly>());
        }

        public static void MapRequestMappings(RouteCollection route, IEnumerable<Assembly> assemblies)
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
                        }
                    }
                }
            }
        }
    }

    public class RouteItem
    {
    }
}
