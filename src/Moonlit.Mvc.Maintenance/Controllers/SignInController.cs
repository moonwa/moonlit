using System;
using System.Linq;
using System.Web.Mvc;
using Moonlit.Caching;
using Moonlit.Mvc.Maintenance.Models;

namespace Moonlit.Mvc.Maintenance.Controllers
{
    [AllowAnonymous]
    public class SignInController : MaintControllerBase
    {
        private readonly Authenticate _authenticate;
        private readonly IPrivilegeLoader _privilegeLoader;
        private readonly ICacheManager _cacheManager;
        private const string Url = "SignIn";
        public SignInController(Authenticate authenticate, IPrivilegeLoader privilegeLoader, ICacheManager cacheManager)
        {
            _authenticate = authenticate;
            _privilegeLoader = privilegeLoader;
            _cacheManager = cacheManager.GetPrefixCacheManager("sign_failed::");
        }

        [RequestMapping("signin", Url)]
        public ActionResult SignIn()
        {
            SignInModel model = new SignInModel();
            return Template(model.CreateTemplate());
        }
        [RequestMapping("signin_postback", Url)]
        [HttpPost]
        public ActionResult SignIn(SignInModel model, string returnUrl)
        {

            if (!ModelState.IsValid)
            {
                return Template(model.CreateTemplate());
            }
            var db = MaintDbContext;
            var adminUser = db.Users.FirstOrDefault(x => x.LoginName == model.UserName);
            if (adminUser == null)
            {
                this.ModelState.AddModelError("UserName", "用户不存在");
                return Template(model.CreateTemplate());
            }
            int count = _cacheManager.Get<int?>(HttpContext.Request.UserHostAddress + ":" + adminUser.UserName) ?? 5;
            if (count == 0)
            {
                this.ModelState.AddModelError("UserName", "您已经失败 5 次，请明天再试。");
                return Template(model.CreateTemplate());
            }
            if (adminUser.HashPassword(model.Password) != adminUser.Password)
            {
                _cacheManager.Set(HttpContext.Request.UserHostAddress + ":" + adminUser.UserName, count - 1, TimeSpan.FromDays(1));
                this.ModelState.AddModelError("Password", "密码错误");
                return Template(model.CreateTemplate());
            }
            var privileges = adminUser.IsSuper ? _privilegeLoader.Load().Items.Select(x => x.Name).ToArray() : adminUser.Roles.ToList().SelectMany(x => x.GetPrivileges()).ToArray();
            _authenticate.SetSession(adminUser.LoginName, new Session
            {
                UserName = adminUser.LoginName,
                Privileges = privileges,
                AppId = "Website",
                ExpiredTime = DateTime.Now.AddDays(1),
            });
            if (string.IsNullOrEmpty(returnUrl))
            {
                return RedirectToRequestMapping("Home", null);
            }
            return Redirect(returnUrl);
        }

    }
}