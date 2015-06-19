using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Moonlit.Mvc.Maintenance.Models;

namespace Moonlit.Mvc.Maintenance.Controllers
{
    [MoonlitAuthorize( )]
    public class DashboardController : MaintControllerBase
    {
        // GET: Dashboard
        [RequestMapping("Home", "")]
        [DashboardIcon]
        public ActionResult Index(DashboardModel model)
        {
            return Template(model.CreateTemplate(Request.RequestContext));
        } 
    }
}