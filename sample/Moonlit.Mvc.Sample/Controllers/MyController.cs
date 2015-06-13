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

    public class SimpleHelper
    {
        public static ISite CreateSite()
        {
            return new Site
            {
                Title = "Moonlit ≤‚ ‘’æµ„",
                CopyRight = "moon.wa",
            };
        }
    }

    public class Site : ISite
    {
        public string Title { get; set; }
        public string CopyRight { get; set; }
    }
}