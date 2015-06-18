using System.Web.Mvc;

namespace Moonlit.Mvc.Maintenance.Controllers
{  
    [Authorize]
    public class HomeController : MoonlitController
    {

        [RequestMapping("Home", "")]
        public ActionResult Index()
        {
            return RedirectToRequestMapping("users", null);
        }

    }
}