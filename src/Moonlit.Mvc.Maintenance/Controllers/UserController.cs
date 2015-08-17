﻿using System;
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
    [MoonlitAuthorize(Roles = MaintModule.PrivilegeAdminUser)]
    public class UserController : MaintControllerBase
    {
        [SitemapNode(Parent = "BasicData", Name = "users", ResourceType = typeof(MaintCultureTextResources), Text = "AdminUserList")]
        [Display(Name = "用户管理", Description = "用户管理描述，这是一段很长的描述")]
        public ActionResult Index(AdminUserListModel model)
        {
            return Template(model.CreateTemplate(ControllerContext));
        }


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
            return Template(model.CreateTemplate(ControllerContext));
        }
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
            return Template(model.CreateTemplate(ControllerContext));
        }


        [SitemapNode(Text = "创建用户", Parent = "users")]
        public ActionResult Create()
        {
            var model =  new AdminUserCreateModel() ;
            return Template(model.CreateTemplate(ControllerContext));
        }

        [HttpPost]
        public async Task<ActionResult> Create(AdminUserCreateModel model)
        {
            ModelState.Clear();

            var user = new User();
            model.ToEntity(user);

            if (!ValidateAs<AdminUserCreateModel, User>(user))
            {
                return Template(model.CreateTemplate(ControllerContext));
            }
           
            var db = MaintDbContext;
            var loginName = model.LoginName.Trim();
            if (await db.Users.AnyAsync(x => x.LoginName == loginName))
            {
                var errorMessage = string.Format(MaintCultureTextResources.ValidationDumplicate,
                    MaintCultureTextResources.AdminUserLoginName, loginName);
                ModelState.AddModelError("LoginName", string.Format(errorMessage, loginName));
                return Template(model.CreateTemplate(ControllerContext));
            }
            db.Add(user);
            await db.SaveChangesAsync();
            await SetFlashAsync(new FlashMessage
            {
                Text = MaintCultureTextResources.SuccessToSave,
                MessageType = FlashMessageType.Success,
            });
            return RedirectToAction("Create");
        }

        [SitemapNode(Text = "编辑用户", Parent = "users")]
        public async Task<ActionResult> Edit(int id)
        {
            var db = MaintDbContext;
            var adminUser = await db.Users.FirstOrDefaultAsync(x => x.UserId == id);
            if (adminUser == null)
            {
                return HttpNotFound();
            }

            AdminUserEditModel model = new AdminUserEditModel();
            model.FromEntity(adminUser, false);

            return Template(model.CreateTemplate(ControllerContext));
        }


        [HttpPost]
        public async Task<ActionResult> Edit(AdminUserEditModel model, int id)
        {
            var user = MaintDbContext.Users.Include(x => x.Roles).FirstOrDefault(x => x.UserId == id);
            if (user == null)
            {
                return HttpNotFound();
            }

            model.FromEntity(user, true);
            model.ToEntity(user);
            if (!ValidateAs<AdminUserCreateModel, User>(user))
            {
                return Template(model.CreateTemplate(ControllerContext));
            }

          
            await MaintDbContext.SaveChangesAsync();
            await SetFlashAsync(new FlashMessage
            {
                Text = MaintCultureTextResources.SuccessToSave,
                MessageType = FlashMessageType.Success,
            });

            return Template(model.CreateTemplate(ControllerContext));
        }
    }
}