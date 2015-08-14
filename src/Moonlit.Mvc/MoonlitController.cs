using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using Moonlit.Caching;
using Moonlit.Mvc.Templates;
using Newtonsoft.Json;

namespace Moonlit.Mvc
{
    public abstract class MoonlitController : Controller
    {
        public IFlash Flash { get; set; }


        public void SetFlash(object target)
        {
            Flash.Set(target);
        }
        public async Task SetFlashAsync(object target)
        {
            await Flash.SetAsync(target).ConfigureAwait(false);
        }

 

        protected virtual ActionResult Template(Template template)
        {
            return new TemplateResult(template, ViewData)
            {
                TempData = TempData,
                ViewEngineCollection = ViewEngineCollection,
            };
        }
    }
}
