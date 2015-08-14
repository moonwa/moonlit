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
            var template = OnCreate(new AdminUserCreateModel());
            return Template(template);
        }

        private Template OnCreate(AdminUserCreateModel user)
        {
            return user.CreateTemplate(ControllerContext);
        }
        //        private AdministrationSimpleEditTemplate OnCreate(User user)
        //        {
        //            var template = new AdministrationSimpleEditTemplate
        //            {
        //                Title = MaintCultureTextResources.AdminUserCreate,
        //                Description = MaintCultureTextResources.AdminUserCreateDescription,
        //                FormTitle = MaintCultureTextResources.AdminUserInfo,
        //                Fields = new FieldsBuilder().ForEntity(user, ControllerContext).Build(),
        //                Buttons = new IClickable[]
        //                {
        //                    new Button
        //                    {
        //                        Text = MaintCultureTextResources.Save,
        //                        ActionName = ""
        //                    }
        //                }
        //            };
        //            return template;
        //        }

        //        [HttpPost]
        //        public async Task<ActionResult> Create([Bind(Include = "UserName,LoginName,Password,Gender,DateOfBirth,IsEnabled,CultureId")] User user, int[] roles)
        //        {
        //            if (!ModelState.IsValid)
        //            {
        //                return Template(OnCreate(user));
        //            }
        //            var db = MaintDbContext;
        //            var loginName = user.LoginName.Trim();
        //            if (await db.Users.AnyAsync(x => x.LoginName == loginName))
        //            {
        //                var errorMessage = string.Format(MaintCultureTextResources.ValidationDumplicate,
        //                    MaintCultureTextResources.AdminUserLoginName, loginName);
        //                ModelState.AddModelError("LoginName", string.Format(errorMessage, loginName));
        //                return Template(OnCreate(user));
        //            }
        //
        //            user.Password = user.HashPassword(user.Password ?? "");
        //            if (roles != null)
        //            {
        //                user.Roles = db.Roles.Where(x => roles.Contains(x.RoleId)).ToList();
        //            }
        //            db.Add(user);
        //            await db.SaveChangesAsync();
        //            await SetFlashAsync(new FlashMessage
        //            {
        //                Text = MaintCultureTextResources.SuccessToSave,
        //                MessageType = FlashMessageType.Success,
        //            });
        //            return RedirectToAction("Create");
        //        }
        [HttpPost]
        public async Task<ActionResult> Create(AdminUserCreateModel model)
        {
            ModelState.Clear();  

            var user = new User();
            model.ToEntity(user );

            var metadata = ModelMetadataProviders.Current.GetMetadataForType((Func<object>)(() => user), typeof(User));
            var modelValidator = ModelValidator.GetModelValidator(metadata, this.ControllerContext);
            foreach (ModelValidationResult validationResult in modelValidator.Validate((object)null))
            {
                this.ModelState.AddModelError(validationResult.MemberName, validationResult.Message);
            }

            if (!ModelState.IsValid)
            {
                return Template(OnCreate(model));
            }

            var db = MaintDbContext;
            var loginName = model.LoginName.Trim();
            if (await db.Users.AnyAsync(x => x.LoginName == loginName))
            {
                var errorMessage = string.Format(MaintCultureTextResources.ValidationDumplicate,
                    MaintCultureTextResources.AdminUserLoginName, loginName);
                ModelState.AddModelError("LoginName", string.Format(errorMessage, loginName));
                return Template(OnCreate(model));
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
            var template = OnEdit(adminUser);

            return Template(template);
        }

        private AdministrationSimpleEditTemplate OnEdit(User adminUser)
        {
            var template = new AdministrationSimpleEditTemplate
            {
                Title = MaintCultureTextResources.AdminUserEdit,
                Description = MaintCultureTextResources.AdminUserEditDescription,
                FormTitle = MaintCultureTextResources.AdminUserInfo,
                Fields = new FieldsBuilder().ForEntity(adminUser, ControllerContext).ReadOnly("LoginName").Build(),
                Buttons = new IClickable[]
                {
                    new Button
                    {
                        Text = MaintCultureTextResources.Save,
                        ActionName = ""
                    }
                }
            };
            return template;
        }

        [HttpPost]
        public async Task<ActionResult> Edit(int id, int[] roles)
        {
            var user = MaintDbContext.Users.Include(x => x.Roles).FirstOrDefault(x => x.UserId == id);
            if (user == null)
            {
                return HttpNotFound();
            }

            if (!TryUpdateModel(user, null, "UserName,Password,Gender,DateOfBirth,IsEnabled,CultureId".Split(',')))
            {
                return Template(OnEdit(user));

            }
            user.Roles = roles.IsNullOrEmpty() ? new List<Role>() : MaintDbContext.Roles.Where(x => roles.Contains(x.RoleId)).ToList();
            if (!string.IsNullOrEmpty(user.Password))
            {
                user.Password = user.HashPassword(user.Password);
            }
            await MaintDbContext.SaveChangesAsync();
            await SetFlashAsync(new FlashMessage
            {
                Text = MaintCultureTextResources.SuccessToSave,
                MessageType = FlashMessageType.Success,
            });

            return Template(OnEdit(user));
        }
    }
}