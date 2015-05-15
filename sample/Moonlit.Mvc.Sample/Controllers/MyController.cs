using System.Web.Mvc;
using Moonlit.Mvc.Templates;

namespace Moonlit.Mvc.Sample.Controllers
{
    public class MyController : MoonlitController
    {
        protected override ActionResult Template(Template template)
        {
            template.Site = SimpleHelper.CreateSite();
            return base.Template(template);
        }
          
    }
}