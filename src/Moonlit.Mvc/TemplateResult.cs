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
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var model = this.Model as IMoonlitModel;

            this.ViewName = model.GetObject<Theme>().Name + "/" + _template.ViewName;
            this.ViewData.Model = _template;
            _template.OnReadyRender(context);
            base.ExecuteResult(context);
        }
    }
}
