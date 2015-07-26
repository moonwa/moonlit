using System.Web.Mvc;
using Moonlit.Mvc.Maintenance.Properties;

namespace Moonlit.Mvc.Maintenance.Controllers
{ 
    public class SignOutController : MaintControllerBase
    {
        private readonly Authenticate _authenticate; 
        public SignOutController(Authenticate authenticate)
        {
            _authenticate = authenticate; 
        }

        [RequestMapping("SignOut", "SignOut")]
        [SitemapNode(ResourceType = typeof(MaintCultureTextResources), Text = "Exit", Order = 1000000, SiteMap = "Profile")]
        public ActionResult SignIn()
        {
            _authenticate.SignOut();
            return Redirect ("/");
        }
    }
}