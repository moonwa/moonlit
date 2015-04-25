using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Moonlit.Mvc.Templates;

namespace Moonlit.Mvc.Sample.Controllers
{

    [RequestMapping(Url = "user")]
    public class UserController : Controller
    {
        [RequestMapping("user", "list")]
        // GET: User
        public ActionResult Index()
        {
            return new TemplateResult(new AdministrationSimpleListTemplate
            {
                Criteria = new[]
                {
                    new AdministrationSimpleListTemplate.Criterion
                    {
                        Width = 3,
                        Label = "用户名",
                        Field = "UserName",
                        Editor = new TextBox()
                        {
                           MaxLength = 12,
                        }
                    }
                },
                Table = new Table
                {
                    Columns = new Column[]
                    {
                        new Column()
                        {
                            Header = new TextColumnHeader
                            {
                                Sort = "UserName",
                                Text = "用户名",
                            },
                            Cell = new BoundedFieldCell()
                            {
                                Field = "UserName"
                            }
                        }
                    }
                },
                GlobalButtons = new IClickable[]
                {
                    new FormActionButton()
                    {
                        Text = "搜索",
                        ActionName = "Query",
                        Handler = (UserListQueryModel model) =>
                        {
                            return new User[]
                            {
                                new User
                                {
                                    UserName = "user1", Gender = Gender.Male, DateOfBirth = DateTime.Parse("1993-01-03")
                                }
                            }.
                            AsQueryable() ;
                        }
                    }
                }
            });
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
    public class BoundedFieldCell : ColumnCell
    {
        public string Field { get; set; }
    }
}