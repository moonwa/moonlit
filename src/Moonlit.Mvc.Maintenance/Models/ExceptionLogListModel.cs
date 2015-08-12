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
    public class ExceptionLogListModel : IPagedRequest
    {
        public ExceptionLogListModel()
        {
            PageIndex = 1;
            PageSize = 10;
            OrderBy = "ExceptionLogId desc";
        }

        [TextBox]
        [Field(FieldWidth.W6)]
        [Display(ResourceType = typeof(MaintCultureTextResources), Name = "Keyword")]
        public string Keyword { get; set; }
        [DatePicker]
        [Field(FieldWidth.W6)]
        [Display(ResourceType = typeof(MaintCultureTextResources), Name = "StartTime")]
        public DateTime? StartTime { get; set; }
        [DatePicker]
        [Field(FieldWidth.W6)]
        [Display(ResourceType = typeof(MaintCultureTextResources), Name = "EndTime")]
        public DateTime? EndTime { get; set; }

        public string OrderBy { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }

        public Template CreateTemplate(ControllerContext controllerContext, IMaintDbRepository db)
        {
            var query = db.ExceptionLogs.AsQueryable();
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
            var template = new AdministrationSimpleListTemplate(query)
            {
                Title = MaintCultureTextResources.ExceptionLogList,
                Description = MaintCultureTextResources.ExceptionLogListDescription,
                QueryPanelTitle = MaintCultureTextResources.PanelQuery,
                DefaultSort = "ExceptionLogId desc",
                DefaultPageSize = 10,
                Criteria = TemplateHelper.MakeFields(this, controllerContext),
                GlobalButtons = new IClickable[]
                {
                    new Button
                    {
                        Text = MaintCultureTextResources.Search,
                        ActionName = ""
                    },
                },
                Table = new Table
                {
                    Columns = new List<TableColumn>
                    {
                        new TableColumn
                        {
                            Sort = "CreationTime",
                            Header = MaintCultureTextResources.ExceptionLogCreationTime,
                            CellTemplate = x => new Literal
                            {
                                Text = ((ExceptionLog) x.Target).CreationTime.ToString()
                            }
                        },
                        new TableColumn
                        {
                            Sort = "RouteData",
                            Header = MaintCultureTextResources.ExceptionLogRouteData,
                            CellTemplate = x => new Literal
                            {
                                Text = ((ExceptionLog) x.Target).RouteData
                            }
                        },
                        new TableColumn
                        {
                            Sort = "Exception",
                            Header = MaintCultureTextResources.ExceptionLogException,
                            CellTemplate = x => new Literal
                            {
                                Text = ((ExceptionLog) x.Target).Exception
                            }
                        },
                    }
                }
            }; 
            return template;
        }
    }
}