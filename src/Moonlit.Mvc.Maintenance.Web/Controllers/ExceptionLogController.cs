using System.Web.Mvc;
using Moonlit.Mvc.Maintenance.Models;
using Moonlit.Mvc.Maintenance.Properties;

namespace Moonlit.Mvc.Maintenance.Controllers
{
    [MoonlitAuthorize(Roles = MaintPrivileges.PrivilegeAdminUser)]
    public class ExceptionLogController : MaintControllerBase
    {

        [SitemapNode(Parent = "Site", ResourceType = typeof(MaintCultureTextResources), Text = "ExceptionLogIndex")]
        public ActionResult Index(ExceptionLogIndexModel model)
        {
            return Template(model.CreateTemplate(ControllerContext, Database));
        }
    }
}