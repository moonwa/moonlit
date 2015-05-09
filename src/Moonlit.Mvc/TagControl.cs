using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace Moonlit.Mvc
{
    public abstract class TagControl : Control 
    {
        public string CssClass { get; set; }
        protected TagControl()
        {
            this.DataAttributes = new Dictionary<string, object>();
        }
        public Dictionary<string, object> DataAttributes { get; set; }

        protected abstract TagBuilder CreateTagBuilder(HtmlHelper htmlHelper);
        public override IHtmlString Render(HtmlHelper htmlHelper)
        {
            TagBuilder tagBuilder = CreateTagBuilder(htmlHelper);
            foreach (var dataAttribute in this.DataAttributes)
            {
                if (dataAttribute.Value != null)
                {
                    tagBuilder.Attributes["data-" + dataAttribute.Key] = dataAttribute.Value.ToString();
                }
            }
            if (CssClass != null)
            {
                tagBuilder.AddCssClass(CssClass);
            }
            if (Name != null)
            {
                tagBuilder.GenerateId(Name);
                tagBuilder.Attributes["name"] = Name;
            }
            return MvcHtmlString.Create(tagBuilder.ToString(TagRenderMode.Normal));
        }
    }
}