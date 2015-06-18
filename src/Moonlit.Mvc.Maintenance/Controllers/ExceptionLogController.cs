using System.Web.Mvc;
using Moonlit.Mvc.Maintenance.Models;
using Moonlit.Mvc.Maintenance.Properties;

namespace Moonlit.Mvc.Maintenance.Controllers
{
    [Authorize(Roles = MaintModule.PrivilegeAdminUser, Order = 1000)]
    public class ExceptionLogController : MaintControllerBase
    {
        [RequestMapping("ExceptionLogs", "ExceptionLog")]
        [SitemapNode(Parent = "Site", ResourceType = typeof(CultureTextResources), Text = "ExceptionLogList")]
        public ActionResult Index(ExceptionLogListModel model)
        {
            return Template(model.CreateTemplate(Request.RequestContext, MaintDbContext));
        }
    }
}