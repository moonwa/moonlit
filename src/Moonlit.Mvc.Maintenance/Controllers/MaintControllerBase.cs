using System;
using System.Web.Mvc;
using Moonlit.Mvc.Maintenance.Domains;
using Moonlit.Mvc.Maintenance.Models;
using Moonlit.Mvc.Maintenance.Services;
using Moonlit.Mvc.Templates;

namespace Moonlit.Mvc.Maintenance.Controllers
{
    public class MaintControllerBase : MoonlitController
    {
        public IMaintDbRepository MaintDbContext { get; set; }
        public IMaintDomainService MaintDomainService { get; set; }

        protected override ActionResult Template(Template template)
        {
            template.Site = new SiteModel(MaintDomainService.GetSystemSettings());
            return base.Template(template);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                using (MaintDbContext as IDisposable)
                {

                }
            }
            base.Dispose(disposing);
        }
    }
}