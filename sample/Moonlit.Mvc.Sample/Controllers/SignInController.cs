using System;
using System.Linq;
using System.Web.Mvc;
using Moonlit.Mvc.Controls;
using Moonlit.Mvc.Sample.Models;
using Moonlit.Mvc.Templates;

namespace Moonlit.Mvc.Sample.Controllers
{
    [RequestMapping(Url = "SignIn")]
    public class SignInController : MyController
    {
        private readonly Authenticate _authenticate;

        public SignInController(Authenticate authenticate)
        {
            _authenticate = authenticate;
        }

        [RequestMapping(Name = "SignIn")]
        public ActionResult Index(RequestMappings requestMappings)
        {
            SignInModel model = new SignInModel();
            return RenderTemplate(model, requestMappings);
        }
        [ActionName("Index")]
        [RequestMapping(Name = "SignIn_Save")]
        [HttpPost]
        public ActionResult Save(SignInModel model, RequestMappings requestMappings)
        {
            if (ModelState.IsValid)
            {
                _authenticate.SetSession(new SignInSession()
                {
                    UserName = model.UserName,
                    Privileges = new[]
                    {
                        "view"
                    }
                });
                return RedirectToRequestMapping(requestMappings, "Users", null);
            }
            return RenderTemplate(model, requestMappings);
        }

        private ActionResult RenderTemplate(SignInModel model, RequestMappings requestMappings)
        {
            var template = new SimpleBoxTemplate(this.ControllerContext)
            {
                Site = SimpleHelper.CreateSite(),
                Title = "用户登录",
                Description = "用户登录",
                Fields = new[]
                {
                    new Field
                    {
                        FieldName = "UserName",
                        Control = new TextBox()
                        {
                            Value = model.UserName,
                            PlaceHolder = "用户名",
                            Icon = "user"
                        }
                    },
                    new Field()
                    {
                        FieldName = "Password",
                        Control = new PasswordBox()
                        {
                            Icon = "lock",
                            PlaceHolder = "登录密码"
                        }
                    }
                },
                Buttons = new IClickable[]
                {
                    new Button
                    {
                        ActionName = "",
                        Text = "登录"
                    }
                },
                Additional = new List
                {
                    Style = ListStyle.Unstyled,
                    //                    Items = new Control[]
                    //                    {
                    //                        new Control[]
                    //                        {
                    //                            new Literal(){
                    //                            Text = "您是否忘记了登录密码?"}, 
                    //                            new Link
                    //                            {
                    //                                Url = "#",
                    //                                Style=  LInkStyle.Normal,
                    //                                Text = "忘记密码"
                    //                            }
                    //                        }.Combine()
                    //                    }
                }
            };
            return Template(template);
        }
    }
}