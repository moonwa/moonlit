﻿using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Moonlit.Mvc.Maintenance.Domains;
using Moonlit.Mvc.Maintenance.Models;
using Moonlit.Mvc.Maintenance.Properties;

namespace Moonlit.Mvc.Maintenance.Controllers
{
    [Authorize(Roles = MaintModule.PrivilegeAdminUser, Order = 1000)]
    public class UserController : MaintControllerBase
    {
        [RequestMapping("users", "user")]
        [SitemapNode(Parent = "BasicData", ResourceType = typeof(CultureTextResources), Text = "AdminUserList")]
        public ActionResult Index(AdminUserListModel model)
        {
            return Template(model.CreateTemplate(Request.RequestContext, MaintDbContext));
        }
        [RequestMapping("users_disable", "user")]
        [FormAction("disable")]
        [ActionName("Index")]
        [HttpPost]
        public ActionResult Disable(AdminUserListModel model, int[] ids)
        {
            if (ids != null && ids.Length > 0)
            {
                foreach (var adminUser in MaintDbContext.Users.Where(x => x.IsEnabled && !x.IsBuildIn && ids.Contains(x.UserId)).ToList())
                {
                    adminUser.IsEnabled = false;
                }
                MaintDbContext.SaveChanges();
            }
            return Template(model.CreateTemplate(Request.RequestContext, MaintDbContext));
        }
        [RequestMapping("users_enable", "user")]
        [FormAction("enable")]
        [ActionName("Index")]
        [HttpPost]
        public ActionResult Enable(AdminUserListModel model, int[] ids)
        {
            if (ids != null && ids.Length > 0)
            {
                foreach (var adminUser in MaintDbContext.Users.Where(x => !x.IsEnabled && !x.IsBuildIn && ids.Contains(x.UserId)).ToList())
                {
                    adminUser.IsEnabled = true;
                }
                MaintDbContext.SaveChanges();
            }
            return Template(model.CreateTemplate(Request.RequestContext, MaintDbContext));
        }

        [RequestMapping("createuser", "user/create")]
        [SitemapNode(Text = "创建用户", Parent = "users")]
        public ActionResult Create()
        {
            var model = new AdminUserCreateModel();
            return Template(model.CreateTemplate(Request.RequestContext));
        }

        [RequestMapping("createuser_postback", "user/create")]
        [HttpPost]
        public async Task<ActionResult> Create(AdminUserCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                return Template(model.CreateTemplate(Request.RequestContext));
            }
            var db = MaintDbContext;
            var loginName = model.LoginName.Trim();
            var user = await db.Users.FirstOrDefaultAsync(x => x.LoginName == loginName);
            if (user != null)
            {
                var errorMessage = string.Format(CultureTextResources.ValidationDumplicate,
                    CultureTextResources.AdminUserLoginName, loginName);

                ModelState.AddModelError("LoginName", string.Format(errorMessage, loginName));
                return Template(model.CreateTemplate(Request.RequestContext));
            }

            user = new User
            {
                LoginName = loginName,
                UserName = model.UserName.Trim(),
                IsSuper = model.IsSuper,
                Gender = model.Gender,
                DateOfBirth = model.DateOfBirth,
                IsEnabled = model.IsEnabled,
            };

            //                    var roleIds = model.RoleIds ?? new int[0];
            //                    adminUser.Roles = await db.Roles.Where(x => roleIds.Contains(x.RoleId)).ToListAsync();
            db.Add(user);
            user.Password = user.HashPassword(model.Password);
            await db.SaveChangesAsync();
            await SetFlashAsync(new FlashMessage
            {
                Text = CultureTextResources.SuccessToSave,
                MessageType = FlashMessageType.Success,
            });
            return RedirectToAction("Create");
        }

        [RequestMapping("editUser", "user/edit/{id}")]
        [SitemapNode(Text = "编辑用户", Parent = "users")]
        public async Task<ActionResult> Edit(int id)
        {
            var db = MaintDbContext;
            var adminUser = await db.Users.FirstOrDefaultAsync(x=>x.UserId == id);
            if (adminUser == null)
            {
                return HttpNotFound();
            }
            var model = new AdminUserEditModel();
            model.SetInnerObject(adminUser);

            return Template(model.CreateTemplate(Request.RequestContext));
        }
        [RequestMapping("edituser_postback", "user/edit/{id}")]
        [HttpPost]
        public async Task<ActionResult> Edit(AdminUserEditModel model, int id)
        {
            if (!ModelState.IsValid)
            {
                return Template(model.CreateTemplate(Request.RequestContext));
            }
            var db = MaintDbContext;
            var adminUser = await db.Users.FirstOrDefaultAsync(x => x.UserId == id);
            if (adminUser == null)
            {
                return HttpNotFound();
            }
            adminUser.UserName = model.UserName.TrimSafty();
            adminUser.Gender = model.Gender;
            adminUser.DateOfBirth = model.DateOfBirth;
            adminUser.IsEnabled = model.IsEnabled;
            if (!string.IsNullOrEmpty(model.Password))
            {
                adminUser.Password = adminUser.HashPassword(model.Password);
            }
            await db.SaveChangesAsync();
            await SetFlashAsync(new FlashMessage
            {
                Text = CultureTextResources.SuccessToSave,
                MessageType = FlashMessageType.Success,
            });

            return Template(model.CreateTemplate(Request.RequestContext));
        }
    }
}