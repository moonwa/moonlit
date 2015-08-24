using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Moonlit.Mvc.Controls;
using Moonlit.Mvc.Maintenance.Domains;
using Moonlit.Mvc.Maintenance.Models;
using Moonlit.Mvc.Maintenance.Properties;
using Moonlit.Mvc.Templates;
using System.Collections.Generic;
using UrlHelper = System.Web.Mvc.UrlHelper;

namespace Moonlit.Mvc.Maintenance.Controllers
{
    [MoonlitAuthorize(Roles = MaintPrivileges.PrivilegeRole)]
    public class RoleController : MaintControllerBase
    {
        [SitemapNode(Parent = "BasicData", Name = "Roles", ResourceType = typeof(MaintCultureTextResources), Text = "RoleIndex")]
        [Display(Name = "角色管理", Description = "角色管理描述，这是一段很长的描述")]
        public ActionResult Index(RoleIndexModel model)
        {
            return Template(model.CreateTemplate(ControllerContext));
        }



        public const string FormActionNameDisable = "Disable";

        [FormAction(FormActionNameDisable)]
        [ActionName("index")]
        [HttpPost]
        public ActionResult Disable(RoleIndexModel model, int[] ids)
        {
            if (ids != null && ids.Length > 0)
            {
                foreach (var role in MaintDbContext.Roles.Where(x => x.IsEnabled && !x.IsBuildIn && ids.Contains(x.RoleId)).ToList())
                {
                    role.IsEnabled = false;
                }
                MaintDbContext.SaveChanges();
            }
            return Template(model.CreateTemplate(ControllerContext));
        }

        public const string FormActionNameEnable = "enable";

        [FormAction(FormActionNameEnable)]
        [ActionName("index")]
        [HttpPost]
        public ActionResult Enable(RoleIndexModel model, int[] ids)
        {
            if (ids != null && ids.Length > 0)
            {
                foreach (var role in MaintDbContext.Roles.Where(x => !x.IsEnabled && !x.IsBuildIn && ids.Contains(x.RoleId)).ToList())
                {
                    role.IsEnabled = true;
                }
                MaintDbContext.SaveChanges();
            }
            return Template(model.CreateTemplate(ControllerContext));
        }

        [SitemapNode(Text = "创建用户", Parent = "roles")]
        public ActionResult Create()
        {
            RoleCreateModel model = new RoleCreateModel();
            return Template(model.CreateTemplate(ControllerContext));
        }


        [HttpPost]
        public async Task<ActionResult> Create(RoleCreateModel model)
        {
            var role = new Role();


            if (!TryUpdateModel(role, model))
            {
                return Template(model.CreateTemplate(ControllerContext));
            }
            var db = MaintDbContext;

            db.Roles.Add(role);
            await db.SaveChangesAsync();
            await SetFlashAsync(new FlashMessage
            {
                Text = MaintCultureTextResources.SuccessToSave,
                MessageType = FlashMessageType.Success,
            });
            return RedirectToAction("Create");
        }

        [SitemapNode(Text = "RoleEdit", ResourceType = typeof(MaintCultureTextResources), Parent = "roles")]
        public async Task<ActionResult> Edit(int id)
        {
            var db = MaintDbContext;
            var role = await db.Roles.FirstOrDefaultAsync(x => x.RoleId == id);
            if (role == null)
            {
                return HttpNotFound();
            }
            var model = new RoleEditModel();
            model.FromEntity(role, false, ControllerContext);

            return Template(model.CreateTemplate(ControllerContext));
        }

        

        [HttpPost] 
        public async Task<ActionResult> Edit(RoleEditModel model, int id)
        {
            var role = await MaintDbContext.Roles.FirstOrDefaultAsync(x => x.RoleId == id);
            if (role == null) 
            {
                return HttpNotFound();
            }
            model.FromEntity(role, true, ControllerContext);
            if (!TryUpdateModel(role, model))
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