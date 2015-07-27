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
    public class RoleListModel : IPagedRequest
    {
        public RoleListModel()
        {
            PageIndex = 1;
            PageSize = 10;
            OrderBy = "UserName";
        }

        public string Keyword { get; set; }

        public string OrderBy { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public bool? IsEnabled { get; set; }
        public Template CreateTemplate(RequestContext requestContext, IMaintDbRepository db)
        {
            var urlHelper = new UrlHelper(requestContext); 
            var query = db.Roles.AsQueryable();
            if (!string.IsNullOrWhiteSpace(Keyword))
            {
                var keyword = Keyword.Trim();

                query = query.Where(x => x.Name.StartsWith(keyword) || x.Name.StartsWith(keyword));
            }
            if (IsEnabled != null)
            {
                query = query.Where(x => x.IsEnabled == IsEnabled);
            }

            return new AdministrationSimpleListTemplate(query)
            {
                Title = MaintCultureTextResources.RoleList,
                Description = MaintCultureTextResources.RoleListDescription,
                QueryPanelTitle = MaintCultureTextResources.PanelQuery,
                Criteria = new[]
                {
                    new Field
                    {
                        Width = 6,
                        Label = MaintCultureTextResources.Keyword,
                        FieldName = "Keyword",
                        Control = new TextBox
                        {
                            MaxLength = 12,
                            Value = Keyword
                        }
                    }, 
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
                                Value = ((Role) x.Target).RoleId.ToString()
                            }
                        },
                        new TableColumn
                        {
                            Sort = "Name",
                            Header = MaintCultureTextResources.RoleName,
                            CellTemplate = x => new Literal
                            {
                                Text = ((Role) x.Target).Name
                            }
                        },  
                        new TableColumn
                        {
                            Sort = "IsEnabled",
                            Header =  MaintCultureTextResources.RoleIsEnabled,
                            CellTemplate = x => new Literal
                            {
                                Text = ((Role) x.Target).IsEnabled ? MaintCultureTextResources.Yes : MaintCultureTextResources.No,
                            }
                        },
                        new TableColumn
                        {
                            Header =MaintCultureTextResources.Operation,
                            CellTemplate = x =>
                            {
                                var url = RequestMappings.Current.GetRequestMapping("editrole").MakeUrl(urlHelper, new {id= ((Role) x.Target).RoleId});
                                return new ControlCollection()
                                    { 
                                        Controls= new List<Control>() {
                                            new Link
                                            {
                                                Style = LinkStyle.Normal,
                                                Text = MaintCultureTextResources.Edit, 
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
                        Text = MaintCultureTextResources.Search,
                        ActionName = ""
                    },
                    new Link
                    {
                        Text = MaintCultureTextResources.New,
                        Url = RequestMappings.Current.GetRequestMapping("createrole").MakeUrl(urlHelper, null),
                    },
                    new Button
                    {
                        Text = MaintCultureTextResources.Disable,
                        ActionName = "Disable"
                    },
                    new Button
                    {
                        Text = MaintCultureTextResources.Enable,
                        ActionName = "Enable"
                    },
                },
                RecordButtons = new IClickable[]
                {
                }
            };
        }
    }
}