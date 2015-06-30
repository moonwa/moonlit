using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web.Compilation;
using System.Web.Routing;

namespace Moonlit.Mvc
{
    public class ReflectionDashboardIconLoader : IDashboardIconLoader
    {
        static ReflectionDashboardIconLoader()
        {
            DashboardIcons = new List<DashbardIconWithDisplay>();
            Register(BuildManager.GetReferencedAssemblies().Cast<Assembly>());
        }
        public static void Register(IEnumerable<Assembly> assemblies)
        {
            assemblies = assemblies.ToList();
            DashboardIcons = MakeDashboardIcons(assemblies);
        }


        private static List<DashbardIconWithDisplay> MakeDashboardIcons(IEnumerable<Assembly> assemblies)
        {
            List<DashbardIconWithDisplay> icons = new List<DashbardIconWithDisplay>();
            foreach (Assembly referencedAssembly in assemblies)
            {
                var mvcAttr = referencedAssembly.GetCustomAttribute<MvcAttribute>();
                if (mvcAttr == null)
                {
                    continue;
                }

                foreach (var exportedType in referencedAssembly.GetExportedTypes())
                {
                    var methodInfos = exportedType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                    foreach (var methodInfo in methodInfos)
                    {
                        var dashboardIconAttributes = methodInfo.GetCustomAttributes<DashboardIconAttribute>(false);
                        var info = methodInfo as ICustomAttributeProvider;

                        foreach (var sitemapNodeAttr in dashboardIconAttributes)
                        {
                            var sitemapNode = MakeNode(sitemapNodeAttr, info);

                            icons.Add(sitemapNode);
                        }
                    }
                }
            }
            return icons;
        }

        class DashbardIconWithDisplay
        {
            public DashboardIconAttribute DashboardIcon { get; set; }
            public DisplayAttribute Display { get; set; }

            public IUrl Url { get; set; }
        }
        private static DashbardIconWithDisplay MakeNode(DashboardIconAttribute sitemapNodeAttr, ICustomAttributeProvider info)
        {
            var sitemapNode = new DashbardIconWithDisplay
            {
                DashboardIcon = sitemapNodeAttr,
                Display = info.GetCustomAttributes(false).OfType<DisplayAttribute>().FirstOrDefault(),
                Url = info.GetCustomAttributes(false).OfType<IUrl>().FirstOrDefault() ?? new ConstUrl("#")
            };
            return sitemapNode;
        }
        private static List<DashbardIconWithDisplay> DashboardIcons;

        public DashboardIcons Create(RequestContext requestContext)
        {
            DashboardIcons sitemaps = new DashboardIcons();
            foreach (var dashbardIconWithDisplay in DashboardIcons)
            {
                var dashboardIcon = Create(dashbardIconWithDisplay, requestContext);
                sitemaps.Add(dashboardIcon);
            }
            return sitemaps;
        }

        private DashboardIcon Create(DashbardIconWithDisplay dashbardIconWithDisplay, RequestContext requestContext)
        {
            DashboardIcon icon = new DashboardIcon
            {
                Icon = dashbardIconWithDisplay.DashboardIcon.Icon,
                Url = dashbardIconWithDisplay.Url.MakeUrl(requestContext),
                Text = dashbardIconWithDisplay.Display == null ? "" : dashbardIconWithDisplay.Display.GetName(),
                Description = dashbardIconWithDisplay.Display == null ? "" : dashbardIconWithDisplay.Display.GetDescription(),
                Order = dashbardIconWithDisplay.DashboardIcon.Order,
            };
            return icon;
        }
    }
}