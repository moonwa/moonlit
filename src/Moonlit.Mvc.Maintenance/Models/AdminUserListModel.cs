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
    public class AdminUserListModel : IPagedRequest
    {
        public AdminUserListModel()
        {
            PageIndex = 1;
            PageSize = 10;
            OrderBy = "UserName";
        }

        [TextBox]
        [Field(FieldWidth.W6)]
        [Display(ResourceType = typeof (MaintCultureTextResources), Name = "Keyword")]
        public string Keyword { get; set; }

        [TextBox]
        [Field(FieldWidth.W6)]
        [Display(ResourceType = typeof (MaintCultureTextResources), Name = "AdminUserUserName")]
        public string UserName { get; set; }

        public string OrderBy { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }

        [CheckBox]
        [Field(FieldWidth.W6)]
        [Display(ResourceType = typeof (MaintCultureTextResources), Name = "AdminUserIsEnabled")]
        public bool? IsEnabled { get; set; }

        public Template CreateTemplate(ControllerContext controllerContext)
        {
            var irepository = DependencyResolver.Current.GetService<IMaintDbRepository>();
            var query = irepository.Users.AsQueryable();
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
            var urlHelper = new UrlHelper(controllerContext.RequestContext);
            var template = new AdministrationSimpleListTemplate(query)
            {
                Title = MaintCultureTextResources.AdminUserList,
                Description = MaintCultureTextResources.AdminUserListDescription,
                QueryPanelTitle = MaintCultureTextResources.PanelQuery,
                DefaultSort = "UserName",
                DefaultPageSize = 10,
                Criteria = TemplateHelper.MakeFields(this, controllerContext),
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
                        Style = LinkStyle.Button,
                        Url = RequestMappings.Current.GetRequestMapping("CreateUser").MakeUrl(urlHelper, null),
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
                            Header = MaintCultureTextResources.AdminUserLoginName,
                            CellTemplate = x => new Literal
                            {
                                Text = ((User) x.Target).LoginName
                            }
                        },
                        new TableColumn
                        {
                            Sort = "UserName",
                            Header = MaintCultureTextResources.AdminUserUserName,
                            CellTemplate = x => new Literal
                            {
                                Text = ((User) x.Target).UserName
                            }
                        },
                        new TableColumn
                        {
                            Sort = "Gender",
                            Header = MaintCultureTextResources.AdminUserGender,
                            CellTemplate = x => new Literal
                            {
                                Text = ((User) x.Target).Gender.ToDisplayString()
                            }
                        },
                        new TableColumn
                        {
                            Sort = "DateOfBirth",
                            Header = MaintCultureTextResources.AdminUserDateOfBirth,
                            CellTemplate = x => new Literal
                            {
                                Text = string.Format("{0:yyyy-MM-dd}", ((User) x.Target).DateOfBirth)
                            }
                        },
                        new TableColumn
                        {
                            Sort = "IsEnabled",
                            Header = MaintCultureTextResources.AdminUserIsEnabled,
                            CellTemplate = x => new Literal
                            {
                                Text = ((User) x.Target).IsEnabled ? MaintCultureTextResources.Yes : MaintCultureTextResources.No,
                            }
                        },
                        new TableColumn
                        {
                            Header = MaintCultureTextResources.Operation,
                            CellTemplate = x =>
                            {
                                var url = RequestMappings.Current.GetRequestMapping("edituser").MakeUrl(urlHelper, new {id = ((User) x.Target).UserId});
                                return new ControlCollection()
                                {
                                    Controls = new List<Control>()
                                    {
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
            }; 
            return template;
        }
    }
}