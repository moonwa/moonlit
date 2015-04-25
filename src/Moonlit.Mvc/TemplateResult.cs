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
        private readonly ITemplate _template;

        public TemplateResult(ITemplate template)
        {
            _template = template;
            this.ViewName = template.ViewName;
            this.ViewData.Model = template;
        }

        public override void ExecuteResult(ControllerContext context)
        {

            _template.OnReadyRender(context);
            base.ExecuteResult(context);
        }
    }
}
