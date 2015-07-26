using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Moonlit.Mvc.Maintenance.Models;
using Moonlit.Mvc.Maintenance.Properties;

namespace Moonlit.Mvc.Maintenance.Controllers
{
    [MoonlitAuthorize()]
    public class ProfileController : MaintControllerBase
    {
        [RequestMapping("settings", "Profile/Settings")]
        [SitemapNode(ResourceType = typeof(MaintCultureTextResources), Text = "ProfileSettings", Group = "ProfileGroup", Order = 100, SiteMap = "Profile")]
        public ActionResult Settings()
        {
            ProfileSettingsModel model = new ProfileSettingsModel();
            var db = MaintDbContext;
            var user = db.Users.FirstOrDefault(x => x.LoginName == User.Identity.Name);
            model.SetInnerObject(user);
            return Template(model.CreateTemplate(Request.RequestContext, MaintDbContext));
        }
        [RequestMapping("settings_postback", "Profile/Settings")]
        [HttpPost]
        public async Task<ActionResult> Settings(ProfileSettingsModel model)
        {
            if (!ModelState.IsValid)
            {
                return Template(model.CreateTemplate(Request.RequestContext, MaintDbContext));
            }
            var db = MaintDbContext;
            var user = db.Users.FirstOrDefault(x => x.LoginName == User.Identity.Name && x.IsEnabled);
            if (user == null)
            {
                return HttpNotFound();
            }
            user.CultureId = model.Culture ?? 0;
            user.DateOfBirth = model.DateOfBirth;
            user.Gender = model.Gender;
            user.UserName = model.UserName;
            await db.SaveChangesAsync();
            await SetFlashAsync(new FlashMessage
            {
                Text = MaintCultureTextResources.SuccessToSave,
                MessageType = FlashMessageType.Success,
            });
            return Template(model.CreateTemplate(Request.RequestContext, MaintDbContext));
        }
        [RequestMapping("changepassword", "Profile/ChangePassword")]
        [SitemapNode(ResourceType = typeof(MaintCultureTextResources), Text = "ProfileChangePassword", Group = "ProfileGroup", Order = 100, SiteMap = "Profile")]
        public ActionResult ChangePassword()
        {
            var model = new ProfileChangePasswordModel();
            var db = MaintDbContext;
            var user = db.Users.FirstOrDefault(x => x.LoginName == User.Identity.Name);
            model.SetInnerObject(user);
            return Template(model.CreateTemplate(Request.RequestContext, MaintDbContext));
        }
        [RequestMapping("changepassword_postback", "Profile/ChangePassword")]
        [HttpPost]
        public async Task<ActionResult> ChangePassword(ProfileChangePasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return Template(model.CreateTemplate(Request.RequestContext, MaintDbContext));
            }
            var db = MaintDbContext;
            var user = db.Users.FirstOrDefault(x => x.LoginName == User.Identity.Name && x.IsEnabled);
            if (user == null)
            {
                return HttpNotFound();
            }
            if (!string.Equals(user.HashPassword(model.OldPassword), user.Password, StringComparison.OrdinalIgnoreCase))
            {
                ModelState.AddModelError("OldPassword", string.Format(MaintCultureTextResources.ValidationError, MaintCultureTextResources.ProfileChangePasswordOldPassword));
                return Template(model.CreateTemplate(Request.RequestContext, MaintDbContext));
            }
            user.Password = user.HashPassword(model.NewPassword);
            await db.SaveChangesAsync();
            await SetFlashAsync(new FlashMessage
            {
                Text = MaintCultureTextResources.SuccessToSave,
                MessageType = FlashMessageType.Success,
            });
            return Template(model.CreateTemplate(Request.RequestContext, MaintDbContext));
        }
    }
}