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
            foreach (var criterion in this.Criteria)
            {
                criterion.OnReadyRender(context);
            }
            Table.DataSource = GetData();
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
        public Pager GetPager()
        {
            var totalCount = _queryable.Count();

            var pageIndex = Convert.ToInt32(GetValueWithDefault(DefaultPageIndex.ToString(), "pageIndex"));
            var pageSize = Convert.ToInt32(GetValueWithDefault(DefaultPageSize.ToString(), "pageSize"));
            if (pageIndex <= 0 || pageSize <= 0) return null;
            return new Pager
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
                sort = sortValue.AttemptedValue;
            }
            return sort;
        }
    }
}