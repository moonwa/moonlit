using System;
using System.Linq;
using System.Web.Mvc;
using Moonlit.Caching;
using Moonlit.Mvc.Maintenance.Domains;
using Moonlit.Mvc.Maintenance.Models;

namespace Moonlit.Mvc.Maintenance.Controllers
{
    [AllowAnonymous]
    public class SignInController : MaintControllerBase
    {
        private readonly Authenticate _authenticate;
        private readonly IPrivilegeLoader _privilegeLoader;
        public SignInController(Authenticate authenticate, IPrivilegeLoader privilegeLoader)
        {
            _authenticate = authenticate;
            _privilegeLoader = privilegeLoader;
        }

        public ActionResult Index()
        {
            SignInModel model = new SignInModel();
            return Template(model.CreateTemplate());
        }
        [FormAction("SignIn")]
        [HttpPost]
        public ActionResult Index(SignInModel model, string returnUrl)
        {
            var siteModel = new SiteModel(Database.SystemSettings);

            if (!ModelState.IsValid)
            {
                return Template(model.CreateTemplate());
            }

            var db = Database;
            var adminUser = db.Users.FirstOrDefault(x => x.LoginName == model.UserName);
            if (adminUser == null)
            {
                this.ModelState.AddModelError("UserName", "用户名错");
                return Template(model.CreateTemplate());
            }


            var expiredTime = DateTime.Now.AddMinutes(-30);
            // Request.UserHostAddress
            int count = siteModel.MaxSignInFailTimes - adminUser.LoginFailedLogs.OrderByDescending(x => x.CreationTime).Count(x => x.CreationTime > expiredTime && x.IpAddress == Request.UserHostAddress);
            if (count <= 0)
            {
                this.ModelState.AddModelError("UserName", "您已经失败 " + siteModel.MaxSignInFailTimes + " 次，请明天再试。");
                return Template(model.CreateTemplate());
            }

            if (adminUser.HashPassword(model.Password) != adminUser.Password)
            {
                adminUser.LoginFailedLogs.Add(new UserLoginFailedLog()
                {
                    User = adminUser,
                    IpAddress = Request.UserHostAddress,
                    CreationTime = DateTime.Now,
                });
                db.SaveChanges();
                count--;
                if (count > 0)
                {
                    this.ModelState.AddModelError("Password", "密码错, 您还剩" + count + " 次");
                }
                else
                {
                    this.ModelState.AddModelError("Password", "密码错, 已经失败" + siteModel.MaxSignInFailTimes + " 次，请明天再试");
                }
                return Template(model.CreateTemplate());
            }
            foreach (var log in adminUser.LoginFailedLogs.Where(x => x.IpAddress == Request.UserHostAddress).ToList())
            {
                db.UserLoginFailedLogs.Remove(log);
            }
            db.SaveChanges();

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