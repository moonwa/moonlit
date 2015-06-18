using System;
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

        public string Keyword { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }

        public string OrderBy { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public Template CreateTemplate(RequestContext requestContext, IMaintDbRepository db)
        {
            var urlHelper = new UrlHelper(requestContext);
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
            
            return new AdministrationSimpleListTemplate(query)
            {
                Title = CultureTextResources.ExceptionLogList,
                Description = CultureTextResources.ExceptionLogListDescription,
                QueryPanelTitle = CultureTextResources.PanelQuery,
                Criteria = new[]
                {
                    new Field
                    {
                        Width = 6,
                        Label = CultureTextResources.Keyword,
                        FieldName = "Keyword",
                        Control = new TextBox
                        {
                            MaxLength = 12,
                            Value = Keyword
                        }
                    },
                    new Field
                    {
                        Width = 6,
                        Label = CultureTextResources.StartTime,
                        FieldName = "StartTime",
                        Control = new DatePicker()
                        {
                            Value=StartTime,
                        }
                    },
                    new Field
                    {
                        Width = 6,
                        Label = CultureTextResources.EndTime,
                        FieldName = "EndTime",
                        Control = new DatePicker()
                        {
                            Value=StartTime,
                        }
                    }
                },
                DefaultSort = "ExceptionLogId desc",
                DefaultPageSize = 10,
                DefaultPageIndex = 1,
                Table = new Table
                {
                    Columns = new[]
                    { 
                        new TableColumn
                        {
                            Sort = "CreationTime",
                            Header = CultureTextResources.ExceptionLogCreationTime,
                            CellTemplate = x => new Literal
                            {
                                Text = ((ExceptionLog) x.Target).CreationTime.ToString()
                            }
                        }, 
                        new TableColumn
                        {
                            Sort = "RouteData",
                            Header = CultureTextResources.ExceptionLogRouteData,
                            CellTemplate = x => new Literal
                            {
                                Text = ((ExceptionLog) x.Target).RouteData
                            }
                        },
                        new TableColumn
                        {
                            Sort = "Exception",
                            Header = CultureTextResources.ExceptionLogException,
                            CellTemplate = x => new Literal
                            {
                                Text = ((ExceptionLog) x.Target).Exception
                            }
                        },
                    }
                },
                GlobalButtons = new IClickable[]
                {
                    new Button
                    {
                        Text = CultureTextResources.Search,
                        ActionName = ""
                    },
                    
                },
                RecordButtons = new IClickable[]
                { 
                }
            };
        }
    }
}