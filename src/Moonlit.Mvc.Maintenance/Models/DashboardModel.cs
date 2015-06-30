using System.Web.Routing;
using Moonlit.Mvc.Templates;

namespace Moonlit.Mvc.Maintenance.Models
{
    public class DashboardModel
    {
        public Template CreateTemplate(RequestContext requestContext)
        {

            return new AdministrationDashboardTemplate
            {
                Title = "Ê×Ò³",
                Description = "Ê×Ò³",
                DashboardIcons = DashboardIcons.Current,
            };
        }
    }
}