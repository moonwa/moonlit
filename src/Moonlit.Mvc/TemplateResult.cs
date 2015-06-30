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

        public TemplateResult(Template template, ViewDataDictionary viewData)
        {
            _template = template;
            viewData.Model = _template;
            this.ViewData = viewData;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var theme = Theme.Current;
            if (theme == null)
            {
                throw new Exception("请启用主题");
            }
            this.ViewName = theme.ResolveControl(_template.GetType());
            _template.OnReadyRender(context);
            base.ExecuteResult(context);
        }
    }
}
