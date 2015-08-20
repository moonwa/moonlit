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
        private const string DefaultUrl = "SignIn";
        public SignInController(Authenticate authenticate, IPrivilegeLoader privilegeLoader, ICacheManager cacheManager)
        {
            _authenticate = authenticate;
            _privilegeLoader = privilegeLoader;
            _cacheManager = cacheManager.GetPrefixCacheManager("sign_failed::");
        }
         

        public ActionResult Index()
        {
            SignInModel model = new SignInModel();
            return Template(model.CreateTemplate());
        }
        [HttpPost]
        public ActionResult Index(SignInModel model, string returnUrl)
        {
            var siteModel = new SiteModel(MaintDbContext.SystemSettings);
            var cacheKey = "signin_fail_times:" + model.UserName + ":" + Request.UserHostAddress;

            int count = _cacheManager.Get<int?>(cacheKey) ?? siteModel.MaxSignInFailTimes;
            if (count == 0)
            {
                this.ModelState.AddModelError("UserName", "您已经失败 " + siteModel.MaxSignInFailTimes + " 次，请明天再试。");
                return Template(model.CreateTemplate());
            }

            if (!ModelState.IsValid)
            {
                return Template(model.CreateTemplate());
            }
            var db = MaintDbContext;
            var adminUser = db.Users.FirstOrDefault(x => x.LoginName == model.UserName);
            if (adminUser == null)
            {
                this.ModelState.AddModelError("UserName", "用户名错");
                return Template(model.CreateTemplate());
            }

            if (adminUser.HashPassword(model.Password) != adminUser.Password)
            {
                _cacheManager.Set(cacheKey, count - 1, TimeSpan.FromDays(1));
                this.ModelState.AddModelError("Password", "密码错");
                return Template(model.CreateTemplate());
            }
            var privileges = adminUser.IsSuper ? _privilegeLoader.Load().Items.Select(x => x.Name).ToArray() : adminUser.Roles.ToList().SelectMany(x => x.PrivilegeArray).ToArray();
            _authenticate.SetSession(adminUser.LoginName, new Session
            {
                UserName = adminUser.LoginName,
                Privileges = privileges,
                AppId = "Website",
                ExpiredTime = DateTime.Now.AddDays(1),
            });
            if (string.IsNullOrEmpty(returnUrl))
            {
                return Redirect("/");
            }
            return Redirect(returnUrl);
        }

    }
}