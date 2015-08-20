using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Moonlit.Mvc.Maintenance.Models;

namespace Moonlit.Mvc.Maintenance.Controllers
{
    [MoonlitAuthorize()]
    public class HomeController : MaintControllerBase
    {
        // GET: Dashboard
        [DashboardIcon]
        [SitemapNode(IsHidden = true)]
        public ActionResult Index(DashboardModel model)
        {
            return Template(model.CreateTemplate(Request.RequestContext));
        }
    }
}