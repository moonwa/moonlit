using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Moonlit.Mvc.Maintenance.Domains;
using Moonlit.Mvc.Maintenance.Models;
using Moonlit.Mvc.Maintenance.Properties;

namespace Moonlit.Mvc.Maintenance.Controllers
{
    [Authorize(Roles = MaintModule.PrivilegeRole, Order = 1000)]
    public class RoleController : MaintControllerBase
    {
        private readonly IPrivilegeLoader _privilegeLoader;

        public RoleController(IPrivilegeLoader privilegeLoader)
        {
            _privilegeLoader = privilegeLoader;
        }

        [RequestMapping("roles", "role")]
        [SitemapNode(Parent = "BasicData", ResourceType = typeof(CultureTextResources), Text = "RoleList")]
        [Display(Name = "角色管理", Description = "角色管理描述，这是一段很长的描述")]
        public ActionResult Index(RoleListModel model)
        {
            return Template(model.CreateTemplate(Request.RequestContext, MaintDbContext));
        }
        [RequestMapping("roles_disable", "role")]
        [FormAction("disable")]
        [ActionName("Index")]
        [HttpPost]
        public ActionResult Disable(RoleListModel model, int[] ids)
        {
            if (ids != null && ids.Length > 0)
            {
                foreach (var role in MaintDbContext.Roles.Where(x => x.IsEnabled && !x.IsBuildIn && ids.Contains(x.RoleId)).ToList())
                {
                    role.IsEnabled = false;
                }
                MaintDbContext.SaveChanges();
            }
            return Template(model.CreateTemplate(Request.RequestContext, MaintDbContext));
        }
        [RequestMapping("roles_enable", "role")]
        [FormAction("enable")]
        [ActionName("Index")]
        [HttpPost]
        public ActionResult Enable(RoleListModel model, int[] ids)
        {
            if (ids != null && ids.Length > 0)
            {
                foreach (var role in MaintDbContext.Roles.Where(x => !x.IsEnabled && !x.IsBuildIn && ids.Contains(x.RoleId)).ToList())
                {
                    role.IsEnabled = true;
                }
                MaintDbContext.SaveChanges();
            }
            return Template(model.CreateTemplate(Request.RequestContext, MaintDbContext));
        }

        [RequestMapping("createrole", "role/create")]
        [SitemapNode(Text = "创建用户", Parent = "roles")]
        public ActionResult Create()
        {
            var model = new RoleCreateModel();
            return Template(model.CreateTemplate(Request.RequestContext, _privilegeLoader));
        }

        [RequestMapping("createrole_postback", "role/create")]
        [HttpPost]
        public async Task<ActionResult> Create(RoleCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                return Template(model.CreateTemplate(Request.RequestContext, _privilegeLoader));
            }
            var db = MaintDbContext;

            var role = new Role
            {
                Name = model.Name,
                IsEnabled = model.IsEnabled,
            };
            role.SetPrivileges(model.Privileges);
            db.Add(role);
            await db.SaveChangesAsync();
            await SetFlashAsync(new FlashMessage
            {
                Text = CultureTextResources.SuccessToSave,
                MessageType = FlashMessageType.Success,
            });
            return RedirectToAction("Create");
        }

        [RequestMapping("editrole", "role/edit/{id}")]
        [SitemapNode(Text = "AdminUserEdit", ResourceType = typeof(CultureTextResources), Parent = "roles")]
        public async Task<ActionResult> Edit(int id)
        {
            var db = MaintDbContext;
            var role = await db.Roles.FirstOrDefaultAsync(x => x.RoleId == id) ;
            if (role == null)
            {
                return HttpNotFound();
            }
            var model = new RoleEditModel();
            model.SetInnerObject(role);

            return Template(model.CreateTemplate(Request.RequestContext, _privilegeLoader));
        }
        [RequestMapping("editrole_postback", "role/edit/{id}")]
        [HttpPost]
        public async Task<ActionResult> Edit(RoleEditModel model, int id)
        {
            if (!ModelState.IsValid)
            {
                return Template(model.CreateTemplate(Request.RequestContext, _privilegeLoader));
            }
            var db = MaintDbContext;
            var role = await db.Roles.FirstOrDefaultAsync(x => x.RoleId == id);
            if (role == null)
            {
                return HttpNotFound();
            }
            role.Name = model.Name.TrimSafty();
            role.IsEnabled = model.IsEnabled;
            role.SetPrivileges(model.Privileges);
            await db.SaveChangesAsync();
            await SetFlashAsync(new FlashMessage
            {
                Text = CultureTextResources.SuccessToSave,
                MessageType = FlashMessageType.Success,
            });

            return Template(model.CreateTemplate(Request.RequestContext, _privilegeLoader));
        }
    }
}