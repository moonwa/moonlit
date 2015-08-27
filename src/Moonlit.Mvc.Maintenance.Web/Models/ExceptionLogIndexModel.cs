using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using Moonlit.Mvc.Controls;
using Moonlit.Mvc.Maintenance.Domains;
using Moonlit.Mvc.Maintenance.Properties;
using Moonlit.Mvc.Templates;

namespace Moonlit.Mvc.Maintenance.Models
{
    public partial class ExceptionLogIndexModel : IPagedRequest
    {
        public ExceptionLogIndexModel()
        {
            PageIndex = 1;
            PageSize = 10;
            OrderBy = "ExceptionLogId desc";
        }



        public string OrderBy { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; } 
        private IQueryable GetDataSource(ControllerContext controllerContext)
        {
            var irepository = DependencyResolver.Current.GetService<IMaintDbRepository>();
            var query = irepository.ExceptionLogs.AsQueryable();
            if (!string.IsNullOrWhiteSpace(Keyword))
            {
                var keyword = Keyword.Trim();

                query = query.Where(x => x.Exception.Contains(keyword) || x.RouteData.StartsWith(keyword));
            }
            if (StartTime != null)
            {
                query = query.Where(x => StartTime <= x.CreationTime);
            }
            if (EndTime != null)
            {
                var endTime = EndTime.Value.AddDays(1);
                query = query.Where(x => x.CreationTime < endTime);
            }

            return query;
        }

        partial void OnTemplate(AdministrationSimpleListTemplate template, ControllerContext controllerContext)
        {
            var tableBuilder = new TableBuilder<ExceptionLog>();
            template.GlobalButtons = new IClickable[]
            {
                new Button
                {
                    Text = MaintCultureTextResources.Search,
                    ActionName = ""
                },
            };
            template.Table = tableBuilder
              .Add(tableBuilder.Literal(x => x.CreationTime.Format(), controllerContext), MaintCultureTextResources.ExceptionLogCreationTime, "CreationTime")
              .Add(tableBuilder.Literal(x => x.RouteData.Format(), controllerContext), MaintCultureTextResources.ExceptionLogRouteData, "RouteData")
              .Add(tableBuilder.Literal(x => x.Exception.Format(), controllerContext), MaintCultureTextResources.ExceptionLogException, "Exception")
              .Build();
        }
    }
}