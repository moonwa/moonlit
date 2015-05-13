using System;
using System.Linq;
using System.Web.Mvc;
using Moonlit.Mvc.Controls;
using Moonlit.Mvc.Sample.Models;
using Moonlit.Mvc.Templates;
using SelectListItem = Moonlit.Mvc.Controls.SelectListItem;

namespace Moonlit.Mvc.Sample.Controllers
{
    [RequestMapping(Url = "User/Create")]
    public class UserCreateController : MoonlitController
    {
        public UserCreateController()
        {
            
        }
        [RequestMapping(Name = "CreateUser")]
        [HttpGet]
        // GET: User
        public ActionResult Index(User model)
        {
            var template = new AdministrationSimpleEditTemplate(this.ControllerContext, model)
            {
                Fields = new[]
                {
                    new Field
                    {
                        Width = 6,
                        Label = "�û���",
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
                        Label = "�Ա�",
                        FieldName = "Gender",
                        Control = new DropdownList()
                        {
                            Items = new[]
                            {
                                new SelectListItem
                                {
                                    Text = "��",
                                    Value = Gender.Male,
                                    IsSelected = true,
                                },
                                new SelectListItem
                                {
                                    Text = "Ů",
                                    Value = Gender.Female,
                                    IsSelected = false,
                                }
                            },
                            Value = model.Gender
                        }
                    }
                },
                Buttons = new IClickable[]
                {
                    new Button()
                    {
                        Text = "����",
                        ActionName = "",
                    }
                }
            };
            return Template(template);
        }
        [RequestMapping(Name = "CreateUser_Save")]
        [HttpPost]
        [ActionName("Index")]
        // GET: User
        public ActionResult Save(User model)
        {
            if (ModelState.IsValid)
            {
                SetFlash(new FlashMessage
                {
                    Text = "����ɹ�",
                    MessageType = FlashMessageType.Success
                });
            }
            return Index(model);
        }
    }
}