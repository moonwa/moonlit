using System;
using System.Web.Mvc;
using Moonlit.Mvc.Maintenance.Domains;
using Moonlit.Mvc.Maintenance.Models;
using Moonlit.Mvc.Templates;

namespace Moonlit.Mvc.Maintenance.Controllers
{
    public class MaintControllerBase : MoonlitController
    {
        public MaintControllerBase()
        {
            Database = new MaintDbContext();
        }

        public MaintDbContext Database { get; set; }

        protected override ActionResult Template(Template template)
        {
            template.Site = new SiteModel(Database.SystemSettings);
            return base.Template(template);
        }
 

        protected FromEntityContext CreateFromContext()
        {
            var isPost = "post".EqualsIgnoreCase(HttpContext.Request.HttpMethod);
            return CreateFromContext(isPost);
        }
        protected FromEntityContext CreateFromContext(bool isPostback)
        {
            return new FromEntityContext
            {
                ControllerContext = ControllerContext,
                IsPostback = isPostback,
            };
        }
    }
}