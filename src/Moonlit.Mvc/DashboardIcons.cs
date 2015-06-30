using System.Collections.Generic;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Moonlit.Mvc
{
    public class DashboardIcons
    {
        public IEnumerable<DashboardIcon> Items
        {
            get { return _dashboardIcons.AsReadOnly(); }
        }
        private readonly List<DashboardIcon> _dashboardIcons = new List<DashboardIcon>();

        public static DashboardIcons Current
        {
            get
            {
                var dashboardIcons = HttpContext.Current.GetObject<DashboardIcons>();
                if (dashboardIcons == null)
                {
                    var loader = DependencyResolver.Current.GetService<IDashboardIconLoader>(false);
                    if (loader == null)
                    {
                        return null;
                    }

                    var httpContext = new HttpContextWrapper(HttpContext.Current);
                    var routeData = RouteTable.Routes.GetRouteData(httpContext);
                    var requestContext = new RequestContext(httpContext, routeData);
                    dashboardIcons = loader.Create(requestContext);
                    dashboardIcons.Filter(HttpContext.Current.User, requestContext);

                    HttpContext.Current.SetObject(dashboardIcons);
                }
                return dashboardIcons;
            }
        }


        public void Filter(IPrincipal user, RequestContext requestContext)
        {

        }



        public void Add(DashboardIcon icon)
        {
            _dashboardIcons.Add(icon);
        }
    }
}