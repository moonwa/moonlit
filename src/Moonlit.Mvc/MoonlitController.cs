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

        public ILocalizer Localizer { get; set; }

        public void SetFlash(object target)
        {
            Flash.Set(target);
        }
        public async Task SetFlashAsync(object target)
        {
            await Flash.SetAsync(target).ConfigureAwait(false);
        }
        public string Localize(string text, string defaultValue)
        {
            return Localizer.GetString(text, defaultValue, Thread.CurrentThread.CurrentUICulture.Name);
        }
        public string Localize(string text)
        {
            return Localize(text, text);
        }
        protected ActionResult RedirectToRequestMapping(RequestMappings requestMappings, string mappingName, object routeValues)
        {
            var mapping = requestMappings.GetRequestMapping(mappingName);
            if (mapping == null)
            {
                throw new Exception("Not found request mapping " + mappingName);
            }
            return RedirectToRequestMapping(mapping, routeValues);
        }
        protected ActionResult RedirectToRequestMapping(RequestMapping requestMapping, object routeValues)
        {

            return Redirect(requestMapping.MakeUrl(this.Url, routeValues));
        }

        protected virtual ActionResult Template(Template template)
        {
            return new TemplateResult(template)
            {
                ViewData = ViewData,
                TempData = TempData,
                ViewEngineCollection = ViewEngineCollection
            };
        }
    }
}
