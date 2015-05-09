using System;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;

namespace Moonlit.Mvc
{
    public class TextBox : TagControl
    {
        public int? MaxLength { get; set; }
        public string PlaceHolder { get; set; }
        protected override TagBuilder CreateTagBuilder(HtmlHelper htmlHelper)
        {
            TagBuilder tagBuilder = new TagBuilder("input");
            tagBuilder.AddCssClass("form-control");
            if (MaxLength != null)
            {
                tagBuilder.Attributes["maxlength"] = this.MaxLength.ToString();
            }
            if (!string.IsNullOrWhiteSpace(PlaceHolder))
            {
                tagBuilder.Attributes["placeholder"] = this.PlaceHolder.Trim();
            }
            var s = htmlHelper.GetModelStateValue(Name, typeof(string));
            tagBuilder.Attributes["value"] = s as string;
            return tagBuilder;
        }

    }
}