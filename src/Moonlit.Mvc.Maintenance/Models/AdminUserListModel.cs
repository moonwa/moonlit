using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using System.Xml.Linq;
using Moonlit.Mvc.Controls;
using Moonlit.Mvc.Maintenance.Domains;
using Moonlit.Mvc.Maintenance.Properties;
using Moonlit.Mvc.Templates;
using Moonlit.Mvc.Url;

namespace Moonlit.Mvc.Maintenance.Models
{
    public class ModelBase
    {
        public ControllerContext ControllerContext { get; set; } 
    }
    public partial class AdminUserListModel : ModelBase, IPagedRequest
    {
        public AdminUserListModel()
        {
            PageIndex = 1;
            PageSize = 10;
            OrderBy = "UserName"; 
        }

        partial void OnTemplate(AdministrationSimpleListTemplate template, ControllerContext controllerContext)
        {
            var urlHelper = new UrlHelper(controllerContext.RequestContext);
            template.GlobalButtons = new IClickable[]
            {
                new Button(MaintCultureTextResources.Search, ""),
                new Link(MaintCultureTextResources.New, urlHelper.GetRequestMappingUrl("CreateUser"), LinkStyle.Button),
                new Button(MaintCultureTextResources.Disable, "Disable"),
                new Button(MaintCultureTextResources.Enable, "Enable"),
            };
            template.Table = new Table
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
            };
        }

        public string OrderBy { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }


        private IQueryable<User> GetDataSource(ControllerContext controllerContext)
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
            return query;
        }
    }
}