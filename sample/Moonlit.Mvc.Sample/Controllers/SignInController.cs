using System;
using System.Linq;
using System.Web.Mvc;
using Moonlit.Mvc.Controls;
using Moonlit.Mvc.Sample.Models;
using Moonlit.Mvc.Templates;

namespace Moonlit.Mvc.Sample.Controllers
{
    public class SignInController : MyController
    {
        private const string RequestUrl = "SignIn";
        private readonly Authenticate _authenticate;

        public SignInController(Authenticate authenticate)
        {
            _authenticate = authenticate;
        }

        [RequestMapping("SignIn", RequestUrl)]
        public ActionResult Index(RequestMappings requestMappings)
        {
            SignInModel model = new SignInModel();
            return RenderTemplate(model);
        }
        [ActionName("Index")]
        [RequestMapping("SignIn_Save", RequestUrl)]
        [HttpPost]
        public ActionResult Save(string userName, SignInModel model)
        {
            if (ModelState.IsValid)
            {
                _authenticate.SetSession(model.UserName, new Session
                {
                    Privileges = new[]
                    {
                        "view"
                    }
                });
                return RedirectToRequestMapping("Users", null);
            }
            return RenderTemplate(model);
        }

        private ActionResult RenderTemplate(SignInModel model)
        {
            var template = new SimpleBoxTemplate
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
                }
            };
            return Template(template);
        }
    }
}