using System;
using System.Linq;
using System.Web.Mvc;
using Moonlit.Mvc.Controls;
using Moonlit.Mvc.Sample.Models;
using Moonlit.Mvc.Templates;
using SelectListItem = Moonlit.Mvc.Controls.SelectListItem;

namespace Moonlit.Mvc.Sample.Controllers
{
    //    [Authorize(Roles = "edit")]

    public class UserCreateController : MyController
    {
        private const string RequestUrl = "User/Create";
        public UserCreateController()
        {
        }
        [SitemapNode(Parent = "Users", IsHidden = true)]
        [RequestMapping("CreateUser", RequestUrl)]
        [HttpGet]
        // GET: User
        public ActionResult Index()
        {
            var model = new Models.User();
            return Render(model);
        }

        private ActionResult Render(Models.User model)
        {
            var template = new AdministrationSimpleEditTemplate(model)
            {
                Fields = new[]
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
                        Label = "性别",
                        FieldName = "Gender",
                        Control = new DropdownList()
                        {
                            Items = new[]
                            {
                                new SelectListItem
                                {
                                    Text = "男",
                                    Value = Gender.Male.ToString(),
                                    Selected = true,
                                },
                                new SelectListItem
                                {
                                    Text = "女",
                                    Value = Gender.Female.ToString(),
                                    Selected = false,
                                }
                            }
                        }
                    }
                },
                Buttons = new IClickable[]
                {
                    new Button()
                    {
                        Text = "保存",
                        ActionName = "",
                    }
                }
            };
            return Template(template);
        }

        [RequestMapping("CreateUser_Save", RequestUrl)]
        [HttpPost]
        [ActionName("Index")]
        // GET: User
        public ActionResult Save(Models.User model)
        {
            if (ModelState.IsValid)
            {
                SetFlash(new FlashMessage
                {
                    Text = "保存成功",
                    MessageType = FlashMessageType.Success
                });
            }
            return Render(model);
        }
    }
}