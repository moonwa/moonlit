using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace Moonlit.Mvc.Templates
{
    [RequestMapping("AdministrationSimpleListTemplate", "AdministrationSimpleListTemplate")]
    public class AdministrationSimpleListTemplate : ITemplate
    {
        public static string DefaultLinkCss = "btn btn-default";
        public string ViewName { get { return "templates/administration/SimpleList"; } }
        public AdministrationSimpleListTemplate()
        {
            Criteria = new Criterion[0];
            GlobalButtons = new IClickable[0];
            RecordButtons = new IClickable[0];
        }

        [RequestMapping("AdministrationSimpleListTemplate_Handler", "")]
        public class Criterion
        {
            public static string DefaultLabelCss = "col-sm-3";
            public static string DefaultEditorCss = "form-control";

            public string Label { get; set; }
            public string Field { get; set; }
            public Control Editor { get; set; }
            public int Width { get; set; }


            public IHtmlString RenderLabel()
            {
                TagBuilder tagBuilder = new TagBuilder("label");
                tagBuilder.AddCssClass("label");
                tagBuilder.Attributes["for"] = Field;
                if (!string.IsNullOrWhiteSpace(DefaultLabelCss))
                {
                    tagBuilder.AddCssClass(DefaultLabelCss);
                }
                tagBuilder.InnerHtml = Label;
                return MvcHtmlString.Create(tagBuilder.ToString(TagRenderMode.Normal));
            }

            public void ReadyRender()
            {
                ICssClass cssClass = this.Editor as ICssClass;
                cssClass.AddCssClass(DefaultEditorCss);
                this.Editor.Name = this.Field;
            }
        }

        public void OnReadyRender(ControllerContext context)
        {
            foreach (var criterion in Criteria)
            {
                criterion.ReadyRender();
            }
            foreach (var button in GlobalButtons)
            {
                ICssClass cssClass = button as ICssClass;
                if (cssClass != null) cssClass.CssClass += DefaultLinkCss;
            }
            foreach (var button in RecordButtons)
            {
                ICssClass cssClass = button as ICssClass;
                if (cssClass != null) cssClass.CssClass += DefaultLinkCss;
            }
        }
        public Criterion[] Criteria { get; set; }
        public IClickable[] GlobalButtons { get; set; }
        public IClickable[] RecordButtons { get; set; }
        public Table Table { get; set; }
    }

    public class AdministrationSimpleListTemplateController : Controller
    {
        //        public ActionResult Index()
        //        {
        //        }
    }
    public class Link : TagControl, IClickable
    {
        public string Url { get; set; }
        public string Text { get; set; }
        protected override TagBuilder CreateTagBuilder()
        {
            TagBuilder builder = new TagBuilder("a");
            if (Url != null)
            {
                builder.Attributes["href"] = Url;
            }
            builder.InnerHtml = Text;
            return builder;
        }
    }
    public class FormActionButton : TagControl, IClickable
    {
        public string ActionName { get; set; }
        public string Text { get; set; }
        public Delegate Handler { get; set; }

        protected override TagBuilder CreateTagBuilder()
        {
            TagBuilder builder = new TagBuilder("button");
            builder.Attributes["type"] = "Submit";
            builder.Attributes["form_action"] = ActionName;
            builder.InnerHtml = Text;
            return builder;
        }
    }

    public interface IClickable
    {
    }
}