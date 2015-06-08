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
            var theme = DependencyResolver.Current.GetService<IThemeLoader>().Theme;
            this.ViewName = theme.Name + "/" + _template.ViewName;
            _template.OnReadyRender(context);
            base.ExecuteResult(context);
        }
    }
}
