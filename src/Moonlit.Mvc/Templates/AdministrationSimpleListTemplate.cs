using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace Moonlit.Mvc.Templates
{

    public class AdministrationSimpleListTemplate : ITemplate
    {
        private readonly ControllerContext _controllerContext;
        private readonly IQueryable _queryable;
        public string ViewName { get { return "templates/administration/SimpleList"; } }

        public AdministrationSimpleListTemplate(ControllerContext controllerContext, IQueryable queryable)
        {
            _controllerContext = controllerContext;
            _queryable = queryable;
            Criteria = new Field[0];
            GlobalButtons = new IClickable[0];
            RecordButtons = new IClickable[0];
        }
        public void OnReadyRender(ControllerContext context)
        {
            foreach (var criterion in Criteria)
            {
                criterion.ReadyRender();
            }
        }

        public Field[] Criteria { get; set; }
        public IClickable[] GlobalButtons { get; set; }
        public IClickable[] RecordButtons { get; set; }
        public Table Table { get; set; }
        public int DefaultPageIndex { get; set; }
        public int DefaultPageSize { get; set; }
        public string DefaultSort { get; set; }
        public IEnumerable GetData()
        {
            var items = _queryable;
            var sort = GetValueWithDefault(DefaultSort, "sort");
            if (!string.IsNullOrWhiteSpace(sort))
            {
                items = items.OrderBy(sort);
            }
            var pageSize = Convert.ToInt32(GetValueWithDefault(DefaultPageSize.ToString(), "pageSize"));
            var pageIndex = Convert.ToInt32(GetValueWithDefault(DefaultPageIndex.ToString(), "pageIndex"));
            if (pageSize > 0 && pageIndex > 0)
            {
                items = items.Skip((pageIndex - 1) * pageSize).Take(pageSize);
            }
            return items;
        }
        public PaginationInfo GetPaginationInfo()
        {
            var totalCount = _queryable.Count();

            var pageIndex = Convert.ToInt32(GetValueWithDefault(DefaultPageIndex.ToString(), "pageIndex"));
            var pageSize = Convert.ToInt32(GetValueWithDefault(DefaultPageSize.ToString(), "pageSize"));
            if (pageIndex <= 0 || pageSize <= 0) return null;
            return new PaginationInfo
            {
                ItemCount = totalCount,
                OrderBy = GetValueWithDefault(DefaultSort, "sort"),
                PageCount = pageSize == 0 ? 1 : (int)Math.Ceiling(totalCount / (double)pageSize),
                PageSize = pageSize,
                PageIndex = pageIndex,
            };
        }
        private string GetValueWithDefault(string defaultValue, string key)
        {
            string sort = defaultValue;
            var sortValue = _controllerContext.Controller.ValueProvider.GetValue(key);
            if (sortValue != null)
            {
                sort = sortValue.AttemptedValue ;
            }
            return sort;
        }
    }

    public class Field
    {
        public string Label { get; set; }
        public string FieldName { get; set; }
        public Control Editor { get; set; }
        public int Width { get; set; }

        public void ReadyRender()
        {
            this.Editor.Name = this.FieldName;
        }

        public IHtmlString Render(HtmlHelper htmlHelper)
        {
            TagBuilder labelBuilder = new TagBuilder("label");
            labelBuilder.AddCssClass("label");
            labelBuilder.Attributes["for"] = FieldName;
            labelBuilder.InnerHtml = Label;

            return MvcHtmlString.Create(labelBuilder.ToString(TagRenderMode.Normal) + this.Editor.Render(htmlHelper));
        }
    }
    public class Link : TagControl, IClickable
    {
        public string Url { get; set; }
        public string Text { get; set; }
        protected override TagBuilder CreateTagBuilder(HtmlHelper htmlHelper)
        {
            TagBuilder builder = new TagBuilder("a");
            builder.AddCssClass("btn btn-default");
            if (Url != null)
            {
                builder.Attributes["href"] = Url;
            }
            builder.InnerHtml = Text;
            return builder;
        }
    }

    public interface IClickable
    {
    }

    public class FormActionButton : TagControl, IClickable
    {
        public string ActionName { get; set; }
        public string Text { get; set; }

        protected override TagBuilder CreateTagBuilder(HtmlHelper htmlHelper)
        {
            TagBuilder builder = new TagBuilder("button");
            builder.AddCssClass("btn btn-default");
            builder.Attributes["type"] = "Submit";
            builder.Attributes["form_action"] = ActionName;
            builder.InnerHtml = Text;
            return builder;
        }
    }

    public class PaginationInfo
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int PageCount { get; set; }
        public int ItemCount { get; set; }
        public string OrderBy { get; set; }
    }
}