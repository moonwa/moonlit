using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using Moonlit.Mvc.Controls;
using Moonlit.Mvc.Maintenance.Domains;
using Moonlit.Mvc.Maintenance.Properties;
using Moonlit.Mvc.Templates;

namespace Moonlit.Mvc.Maintenance.Models
{
    public class CultureListModel : IPagedRequest
    {
        public CultureListModel()
        {
            PageIndex = 1;
            PageSize = 10;
            OrderBy = "Name";
        }

        public string Keyword { get; set; }
        public string Name { get; set; }

        public string OrderBy { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public bool? IsEnabled { get; set; }
        public Template CreateTemplate(RequestContext requestContext, IMaintDbRepository db)
        {
            var urlHelper = new UrlHelper(requestContext);
            var query = db.Cultures.AsQueryable();

            if (!string.IsNullOrWhiteSpace(Keyword))
            {
                var keyword = Keyword.Trim();
                query = query.Where(x => x.Name.Contains(keyword) || x.DisplayName.Contains(keyword));
            }
            if (!string.IsNullOrWhiteSpace(Name))
            {
                var name = Name.Trim();

                query = query.Where(x => x.Name.StartsWith(name));
            }
            if (IsEnabled != null)
            {
                query = query.Where(x => x.IsEnabled == IsEnabled);
            }

            return new AdministrationSimpleListTemplate(query)
            {
                Title = CultureTextResources.CultureList,
                Description = CultureTextResources.CultureListDescription,
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
                        Label = CultureTextResources.CultureName,
                        FieldName = "Name",
                        Control = new TextBox
                        {
                            MaxLength = 12,
                            Value=Name,
                        }
                    }
                },
                DefaultSort = "Name",
                DefaultPageSize = 10,
                DefaultPageIndex = 1,
                Table = new Table
                {
                    Columns = new[]
                    {
                        new TableColumn
                        {
                            CellTemplate = x => new CheckBox()
                            {
                                Name = "ids",
                                Value = ((Culture) x.Target).CultureId.ToString()
                            }
                        },
                        new TableColumn
                        {
                            Sort = "Name",
                            Header = CultureTextResources.CultureName,
                            CellTemplate = x => new Literal
                            {
                                Text = ((Culture) x.Target).Name
                            }
                        },
                        new TableColumn
                        {
                            Sort = "DisplayName",
                            Header = CultureTextResources.CultureDisplayName,
                            CellTemplate = x => new Literal
                            {
                                Text = ((Culture) x.Target).DisplayName
                            }
                        }, 
                        new TableColumn
                        {
                            Sort = "IsEnabled",
                            Header =  CultureTextResources.CultureIsEnabled,
                            CellTemplate = x => new Literal
                            {
                                Text = ((Culture) x.Target).IsEnabled ? CultureTextResources.Yes : CultureTextResources.No,
                            }
                        },
                        new TableColumn
                        {
                            Header =CultureTextResources.Operation,
                            CellTemplate = x =>
                            {
                                var url = RequestMappings.Current.GetRequestMapping("editculture").MakeUrl(urlHelper, new {id= ((Culture) x.Target).CultureId});
                                return new ControlCollection()
                                    { 
                                        Controls= new List<Control>() {
                                            new Link
                                            {
                                                Style = LinkStyle.Normal,
                                                Text = CultureTextResources.Edit, 
                                                Url = url,
                                            }
                                        }
                                    };
                            }
                        }
                    }
                },
                GlobalButtons = new IClickable[]
                {
                    new Button
                    {
                        Text = CultureTextResources.Search,
                        ActionName = ""
                    },
                    new Button
                    {
                        Text = CultureTextResources.Disable,
                        ActionName = "Disable"
                    },
                    new Button
                    {
                        Text = CultureTextResources.Enable,
                        ActionName = "Enable"
                    },
                },
                RecordButtons = new IClickable[]
                {
                    new Link
                    {
                        Text = CultureTextResources.New,
                        Url = RequestMappings.Current.GetRequestMapping("createculture").MakeUrl(urlHelper, null),
                    },
                }
            };
        }
    }
}