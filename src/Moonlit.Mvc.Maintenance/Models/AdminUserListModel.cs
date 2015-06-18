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
    public class AdminUserListModel : IPagedRequest
    {
        public AdminUserListModel()
        {
            PageIndex = 1;
            PageSize = 10;
            OrderBy = "UserName";
        }

        public string Keyword { get; set; }
        public string UserName { get; set; }

        public string OrderBy { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public bool? IsEnabled { get; set; }
        public Template CreateTemplate(RequestContext requestContext, IMaintDbRepository db)
        {
            var urlHelper = new UrlHelper(requestContext);
            var query = db.Users.AsQueryable();
            if (!string.IsNullOrWhiteSpace(Keyword))
            {
                var keyword = Keyword.Trim();

                query = query.Where(x => x.LoginName.StartsWith(keyword) || x.UserName.StartsWith(keyword));
            }
            if (!string.IsNullOrWhiteSpace(UserName))
            {
                var userName = UserName.Trim();

                query = query.Where(x => x.UserName.StartsWith(userName));
            }
            if (IsEnabled != null)
            {
                query = query.Where(x => x.IsEnabled == IsEnabled);
            }

            return new AdministrationSimpleListTemplate(query)
            {
                Title = CultureTextResources.AdminUserList,
                Description = CultureTextResources.AdminUserListDescription,
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
                        Label = CultureTextResources.AdminUserUserName,
                        FieldName = "UserName",
                        Control = new TextBox
                        {
                            MaxLength = 12,
                            Value=UserName,
                        }
                    }
                },
                DefaultSort = "UserName",
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
                                Value = ((User) x.Target).UserId.ToString()
                            }
                        },
                        new TableColumn
                        {
                            Sort = "LoginName",
                            Header = CultureTextResources.AdminUserLoginName,
                            CellTemplate = x => new Literal
                            {
                                Text = ((User) x.Target).LoginName
                            }
                        },
                        new TableColumn
                        {
                            Sort = "UserName",
                            Header = CultureTextResources.AdminUserUserName,
                            CellTemplate = x => new Literal
                            {
                                Text = ((User) x.Target).UserName
                            }
                        },
                        new TableColumn
                        {
                            Sort = "Gender",
                            Header = CultureTextResources.AdminUserGender,
                            CellTemplate = x => new Literal
                            {
                                Text = ((User) x.Target).Gender.ToDisplayString()
                            }
                        },
                        new TableColumn
                        {
                            Sort = "DateOfBirth",
                            Header =  CultureTextResources.AdminUserDateOfBirth,
                            CellTemplate = x => new Literal
                            {
                                Text = string.Format("{0:yyyy-MM-dd}", ((User) x.Target).DateOfBirth)
                            }
                        },
                        new TableColumn
                        {
                            Sort = "IsEnabled",
                            Header =  CultureTextResources.AdminUserIsEnabled,
                            CellTemplate = x => new Literal
                            {
                                Text = ((User) x.Target).IsEnabled ? CultureTextResources.Yes : CultureTextResources.No,
                            }
                        },
                        new TableColumn
                        {
                            Header =CultureTextResources.Operation,
                            CellTemplate = x =>
                            {
                                var url = RequestMappings.Current.GetRequestMapping("edituser").MakeUrl(urlHelper, new {id= ((User) x.Target).UserId});
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
                        Url = RequestMappings.Current.GetRequestMapping("createuser").MakeUrl(urlHelper, null),
                    },
                }
            };
        }
    }
}