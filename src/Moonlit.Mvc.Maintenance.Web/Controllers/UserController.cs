using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Security;
using Moonlit.Collections;
using Moonlit.Mvc.Controls;
using Moonlit.Mvc.Maintenance.Domains;
using Moonlit.Mvc.Maintenance.Models;
using Moonlit.Mvc.Maintenance.Properties;
using Moonlit.Mvc.Templates;
using Moonlit.Mvc.Url;
using UrlHelper = System.Web.Mvc.UrlHelper;

namespace Moonlit.Mvc.Maintenance.Controllers
{
    [MoonlitAuthorize(Roles = MaintPrivileges.PrivilegeAdminUser)]
    public class UserController : MaintControllerBase
    {
        [SitemapNode(Parent = "BasicData", Name = "users", ResourceType = typeof(MaintCultureTextResources), Text = "AdminUserIndex")]
        [Display(Name = "用户管理", Description = "用户管理描述，这是一段很长的描述")]
        public ActionResult Index(AdminUserIndexModel model)
        {
            return Template(model.CreateTemplate(ControllerContext, Database));
        }


        [FormAction("disable")]
        [ActionName("Index")]
        [HttpPost]
        public ActionResult Disable(AdminUserIndexModel model, int[] ids)
        {
            if (ids != null && ids.Length > 0)
            {
                foreach (var adminUser in Database.Users.Where(x => x.IsEnabled && !x.IsBuildIn && ids.Contains(x.UserId)).ToList())
                {
                    adminUser.IsEnabled = false;
                }
                Database.SaveChanges();
            }
            return Template(model.CreateTemplate(ControllerContext,Database));
        }
        [FormAction("enable")]
        [ActionName("Index")]
        [HttpPost]
        public ActionResult Enable(AdminUserIndexModel model, int[] ids)
        {
            if (ids != null && ids.Length > 0)
            {
                foreach (var adminUser in Database.Users.Where(x => !x.IsEnabled && !x.IsBuildIn && ids.Contains(x.UserId)).ToList())
                {
                    adminUser.IsEnabled = true;
                }
                Database.SaveChanges();
            }
            return Template(model.CreateTemplate(ControllerContext,Database));
        }


        [SitemapNode(Text = "创建用户", Parent = "users")]
        public ActionResult Create()
        {
            var model = new AdminUserCreateModel();
            return Template(model.CreateTemplate(ControllerContext));
        }

        [HttpPost]
        public async Task<ActionResult> Create(AdminUserCreateModel model)
        {
            var user = new User();

            if (!TryUpdateModel2(user, model))
            {
                return Template(model.CreateTemplate(ControllerContext));
            }

            var db = Database;
            var loginName = model.LoginName.Trim();
            if (await db.Users.AnyAsync(x => x.LoginName == loginName))
            {
                var errorMessage = string.Format(MaintCultureTextResources.ValidationDumplicate,
                    MaintCultureTextResources.AdminUserLoginName, loginName);
                ModelState.AddModelError("LoginName", string.Format(errorMessage, loginName));
                return Template(model.CreateTemplate(ControllerContext));
            }
            db.Users.Add(user);
            await db.SaveChangesAsync();
            await SetFlashAsync(new FlashMessage
            {
                Text = MaintCultureTextResources.SuccessToSave,
                MessageType = FlashMessageType.Success,
            });
            return Create();
        }

        [SitemapNode(Text = "编辑用户", Parent = "users")]
        public async Task<ActionResult> Edit(int id)
        {
            var db = Database;
            var adminUser = await db.Users.FirstOrDefaultAsync(x => x.UserId == id);
            if (adminUser == null)
            {
                return HttpNotFound();
            }

            AdminUserEditModel model = new AdminUserEditModel();
            model.FromEntity(adminUser, base.CreateFromContext(false));

            return Template(model.CreateTemplate(ControllerContext));
        }


        [HttpPost]
        public async Task<ActionResult> Edit(AdminUserEditModel model, int id)
        {
            var user = Database.Users.Include(x => x.Roles).FirstOrDefault(x => x.UserId == id);
            if (user == null)
            {
                return HttpNotFound();
            }

            model.FromEntity(user, CreateFromContext(true));

            if (!TryUpdateModel2(user, model))
            {
                return Template(model.CreateTemplate(ControllerContext));
            }


            await Database.SaveChangesAsync();
            await SetFlashAsync(new FlashMessage
            {
                Text = MaintCultureTextResources.SuccessToSave,
                MessageType = FlashMessageType.Success,
            });

            return Template(model.CreateTemplate(ControllerContext));
        }

        [MoonlitAuthorize(Roles = MaintPrivileges.PrivilegeAdminUser)]
        [SitemapNode(Text = "UserLoginFailedLogIndex", Name = "UserLoginFailedLogs", Parent = "users", ResourceType = typeof(MaintCultureTextResources))]
        public ActionResult UserLoginFailedLogIndex(UserLoginFailedLogIndexModel model)
        {
            return Template(model.CreateTemplate(ControllerContext));
        }

        [ActionName("UserLoginFailedLogIndex")]
        [HttpPost]
        [FormAction("Delete")]
        public async Task<ActionResult> DeleteUserLoginFailedLogIndex(UserLoginFailedLogIndexModel model, long[] ids)
        {
            foreach (var item in Database.UserLoginFailedLogs.Where(x => ids.Contains(x.UserLoginFailedLogId)).ToList())
            {
                Database.UserLoginFailedLogs.Remove(item);
            }
            await Database.SaveChangesAsync();
            await SetFlashAsync(new FlashMessage
            {
                Text = MaintCultureTextResources.SuccessToSave,
                MessageType = FlashMessageType.Success,
            });

            return UserLoginFailedLogIndex(model);
        }
    }
}