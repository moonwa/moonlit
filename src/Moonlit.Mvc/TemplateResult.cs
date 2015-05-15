using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Moonlit.Mvc.Templates;

namespace Moonlit.Mvc
{

    public class TemplateResult : ViewResult
    {
        private readonly Template _template;

        public TemplateResult(Template template)
        {
            _template = template;
            this.ViewName = MoonlitContext.Current.Theme + "/" + template.ViewName;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            this.ViewData.Model = _template;
            _template.OnReadyRender(context);
            base.ExecuteResult(context);
        }
    }
}
