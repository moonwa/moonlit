using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Compilation;
using System.Web.Mvc;
using System.Web.Routing;

namespace Moonlit.Mvc
{
    public class Themes
    {
        private Dictionary<string, Theme> _themes = new Dictionary<string, Theme>();
        private Theme _defaultTheme;
        public void Register(Theme theme)
        {
            _themes[theme.Name] = theme;
        }
        public void RegisterDefault(Theme theme)
        {
            _defaultTheme = theme;
        }

        public Theme GetTheme(string name)
        {
            if (name == null)
            {
                return _defaultTheme;
            }
            return _themes[name];
        }

        static Themes()
        {
            Current = new Themes();
        }
        public static Themes Current { get; private set; }
    }

    public abstract class Theme
    { 
        public override string ToString()
        {
            return Name;
        }
        private Dictionary<Type, string> _control2Templates = new Dictionary<Type, string>();

        public void RegisterControl(Type controlType, string template)
        {
            _control2Templates[controlType] = template;
        }

        public string ResolveControl(Type controlType)
        {
            string template;
            if (_control2Templates.TryGetValue(controlType, out template))
            {
                return template;
            }
            return null;
        }
        protected internal abstract void PreRequest(MoonlitContext context, RequestContext requestContext);
        public abstract string Name { get; }
    }
    public class RequestMappings
    {
        private readonly List<RequestMapping> _requestMappings = new List<RequestMapping>();
        public void MapRequestMappings(RouteCollection route)
        {
            var referencedAssemblies = BuildManager.GetReferencedAssemblies();
            MapRequestMappings(route, referencedAssemblies.Cast<Assembly>());
        }

        static RequestMappings()
        {
            Current = new RequestMappings();
        }

        public static RequestMappings Current { get; private set; }

        public void MapRequestMappings(RouteCollection route, IEnumerable<Assembly> assemblies)
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
                            var route1 = route.MapRoute(requestMappingAttr.Name,
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
                                route1.DataTokens["area"] = mvcAttr.Module;
                            }
                            _requestMappings.Add(new RequestMapping()
                            {
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
            return this._requestMappings.FirstOrDefault(x => string.Equals(mappingName, x.Name, StringComparison.OrdinalIgnoreCase));
        }
    }
}