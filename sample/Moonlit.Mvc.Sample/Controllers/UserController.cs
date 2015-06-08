using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Moonlit.Mvc.Controls;
using Moonlit.Mvc.Sample.Models;
using Moonlit.Mvc.Templates;

namespace Moonlit.Mvc.Sample.Controllers
{ 
    //    [Authorize]
    public class UserController : MyController
    {
        private const string RequestUrl = "User";
        [RequestMapping("Users",  RequestUrl)]
        [SitemapNode("Users", Text = "用户管理", Parent = "BasicData")]
        public ActionResult Index(UserListQueryModel model)
        {
            var datasources = GetDataSources();
            if (!string.IsNullOrWhiteSpace(model.UserName))
            {
                datasources = datasources.Where(x => x.UserName.Contains(model.UserName));
            }
            var template = new AdministrationSimpleListTemplate(this.ControllerContext, datasources)
            {
                Title = "用户列表",
                Description = "管理系统中所有用户",
                Criteria = new[]
                {
                    new Field
                    {
                        Width = 6,
                        Label = "用户名",
                        FieldName = "UserName",
                        Control = new TextBox()
                        {
                            MaxLength = 12,
                            Value = model.UserName
                        }
                    },
                    new Field
                    {
                        Width = 6,
                        Label = "用户名",
                        FieldName = "UserName2",
                        Control = new TextBox()
                        {
                            MaxLength = 12,
                        }
                    }
                },
                DefaultSort = "UserName",
                DefaultPageSize = 3,
                DefaultPageIndex = 1,
                Table = new Table
                {
                    Columns = new TableColumn[]
                    {
                        new TableColumn()
                        {
                        },
                        new TableColumn()
                        {
                            Sort = "UserName",
                            Header = "用户名",
                            CellTemplate = (x)=> new Literal()
                            {
                                Text =  ((User)x.Target).UserName
                            }
                        },
                        new TableColumn
                        {
                            Sort = "Gender",
                            Header= "性别", 
                            CellTemplate = (x)=> new Literal()
                            {
                                Text =  ((User)x.Target).Gender.ToDisplayString()
                            }
                        },
                        new TableColumn
                        {
                            Sort = "DateOfBirth",
                            Header= "出生日期", 
                            CellTemplate = (x)=> new Literal()
                            {
                                Text = string.Format("{0:yyyy-MM-dd}", ((User)x.Target).DateOfBirth)
                            }
                        }
                    }
                },
                GlobalButtons = new IClickable[]
                {
                    new Button()
                    {
                        Text = "搜索",
                        ActionName = "",
                    }
                }
            };
            return Template(template);
        }

        private static IQueryable<User> GetDataSources()
        {
            var queryable = new[]
            {
                new User()
                {
                    DateOfBirth = DateTime.Parse("1980-1-1"),
                    Gender = Gender.Male,
                    UserName = "80boys"
                },
                new User()
                {
                    DateOfBirth = DateTime.Parse("1990-1-1"),
                    Gender = Gender.Male,
                    UserName = "90boys"
                },
                new User()
                {
                    DateOfBirth = DateTime.Parse("1970-1-1"),
                    Gender = Gender.Male,
                    UserName = "70boys"
                },
                new User()
                {
                    DateOfBirth = DateTime.Parse("1960-1-1"),
                    Gender = Gender.Male,
                    UserName = "60boys"
                },
                new User()
                {
                    DateOfBirth = DateTime.Parse("1950-1-1"),
                    Gender = Gender.Male,
                    UserName = "50boys"
                },
                new User()
                {
                    DateOfBirth = DateTime.Parse("1940-1-1"),
                    Gender = Gender.Male,
                    UserName = "40boys"
                },
            }.AsQueryable();
            return queryable;
        }
    }
}