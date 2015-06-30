using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using Moonlit.Mvc.Controls;

namespace Moonlit.Mvc.Templates
{

    public class AdministrationSimpleListTemplate : Template
    {
        public IQueryable Queryable { get; set; } 
        public string QueryPanelTitle { get; set; }
        public AdministrationSimpleListTemplate()
            : this(null)
        {

        }
        public AdministrationSimpleListTemplate(IQueryable queryable)
        {
            Queryable = queryable;

            Criteria = new Field[0];
            GlobalButtons = new IClickable[0];
            RecordButtons = new IClickable[0];
        }
        public override void OnReadyRender(ControllerContext context)
        {
            foreach (var criterion in this.Criteria)
            {
                criterion.OnReadyRender(context);
            }
            Table.DataSource = GetData(context);
        }

        public Field[] Criteria { get; set; }
        public IClickable[] GlobalButtons { get; set; }
        public IClickable[] RecordButtons { get; set; }
        public Table Table { get; set; }
        public int DefaultPageIndex { get; set; }
        public int DefaultPageSize { get; set; }
        public string DefaultSort { get; set; }
        public IEnumerable GetData(ControllerContext controllerContext)
        {
            var items = Queryable;
            var sort = GetValueWithDefault(DefaultSort, "sort", controllerContext);
            if (!string.IsNullOrWhiteSpace(sort))
            {
                items = items.OrderBy(sort);
            }
            var pageSize = Convert.ToInt32(GetValueWithDefault(DefaultPageSize.ToString(), "pageSize", controllerContext));
            var pageIndex = Convert.ToInt32(GetValueWithDefault(DefaultPageIndex.ToString(), "pageIndex", controllerContext));
            if (pageSize > 0 && pageIndex > 0)
            {
                items = items.Skip((pageIndex - 1) * pageSize).Take(pageSize);
            }
            return items;
        }
        public Pager GetPager(ControllerContext controllerContext)
        {
            var totalCount = Queryable.Count();

            var pageIndex = Convert.ToInt32(GetValueWithDefault(DefaultPageIndex.ToString(), "pageIndex", controllerContext));
            var pageSize = Convert.ToInt32(GetValueWithDefault(DefaultPageSize.ToString(), "pageSize", controllerContext));
            if (pageIndex <= 0 || pageSize <= 0) return null;
            return new Pager
            {
                ItemCount = totalCount,
                OrderBy = GetValueWithDefault(DefaultSort, "sort", controllerContext),
                PageCount = pageSize == 0 ? 1 : (int)Math.Ceiling(totalCount / (double)pageSize),
                PageSize = pageSize,
                PageIndex = pageIndex,
            };
        }
        private string GetValueWithDefault(string defaultValue, string key, ControllerContext controllerContext)
        {
            string sort = defaultValue;
            var sortValue = controllerContext.Controller.ValueProvider.GetValue(key);
            if (sortValue != null)
            {
                sort = sortValue.AttemptedValue;
            }
            return sort;
        }
    }
}