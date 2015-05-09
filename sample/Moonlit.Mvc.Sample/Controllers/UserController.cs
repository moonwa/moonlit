using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Moonlit.Mvc.Templates;

namespace Moonlit.Mvc.Sample.Controllers
{
    [RequestMapping(Url = "User")]
    public class UserController : MoonlitController
    {
        [RequestMapping(Name = "list")]
        // GET: User
        public ActionResult Index(UserListQueryModel model)
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
            if (!string.IsNullOrWhiteSpace(model.UserName))
            {
                queryable = queryable.Where(x => x.UserName.Contains(model.UserName));
            }
            var template = new AdministrationSimpleListTemplate(this.ControllerContext, queryable)
            {
                Criteria = new[]
                {
                    new Field
                    {
                        Width = 4,
                        Label = "用户名",
                        FieldName = "UserName",
                        Editor = new TextBox()
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
                    Columns = new IColumn[]
                    {
                        new CheckBoxColumn()
                        {
                            Field = "UserName",
                        },
                        new TextColumn()
                        {
                            Sort = "UserName",
                            Header = "用户名",
                            Field = "UserName"
                        },
                        new TextColumn
                        {
                            Sort = "Gender",
                            Header= "性别",
                            Field = "Gender",
                        },
                        new TextColumn
                        {
                            Sort = "DateOfBirth",
                            Header= "出生日期",
                            Field = "DateOfBirth",
                            Formatter = (x)=>string.Format("{0:yyyy-MM-dd}", x.Value)
                        }
                    }
                },
                GlobalButtons = new IClickable[]
                {
                    new FormActionButton()
                    {
                        Text = "搜索",
                        ActionName = "",
                    }
                }
            };
            return Template(template);
        }
    }



    public class User
    {
        public string UserName { get; set; }
        public Gender Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
    }

    public enum Gender
    {
        Male,
        Female
    }
    public class UserListQueryModel
    {
        public string UserName { get; set; }
    }
}