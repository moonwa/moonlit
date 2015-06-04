using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Moonlit.Mvc.Templates;
using Moonlit.Mvc.Themes;

namespace Moonlit.Mvc
{

    public class TemplateResult : ViewResult
    {
        private readonly Template _template;

        public TemplateResult(Template template, ViewDataDictionary viewData)
        {
            _template = template;
            viewData.Model = _template;
            this.ViewData = viewData;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            this.ViewName = context.HttpContext.GetObject<Theme>().Name + "/" + _template.ViewName;
            _template.OnReadyRender(context);
            base.ExecuteResult(context);
        }
    }
}
