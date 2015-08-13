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
using Moonlit.Mvc.Url;

namespace Moonlit.Mvc.Maintenance.Controllers
{
    [MoonlitAuthorize(Roles = MaintModule.PrivilegeRole)]
    public class RoleController : MaintControllerBase
    {

        [RequestMapping("roles", "role")]
        [SitemapNode(Parent = "BasicData", ResourceType = typeof(MaintCultureTextResources), Text = "RoleList")]
        [Display(Name = "角色管理", Description = "角色管理描述，这是一段很长的描述")]
        public ActionResult Index(RoleListModel model)
        {
            return Template(OnIndex(model));
        }

        private Template OnIndex(RoleListModel model)
        {
            var urlHelper = new UrlHelper(ControllerContext.RequestContext);
            var query = MaintDbContext.Roles.AsQueryable();
            if (!string.IsNullOrWhiteSpace(model.Keyword))
            {
                var keyword = model.Keyword.Trim();

                query = query.Where(x => x.Name.StartsWith(keyword) || x.Name.StartsWith(keyword));
            }
            if (model.IsEnabled != null)
            {
                query = query.Where(x => x.IsEnabled == model.IsEnabled);
            }

            return new AdministrationSimpleListTemplate(query)
            {
                Title = MaintCultureTextResources.RoleList,
                Description = MaintCultureTextResources.RoleListDescription,
                QueryPanelTitle = MaintCultureTextResources.PanelQuery,
                Criteria = new FieldsBuilder().ForEntity(model, ControllerContext).Build(),
                DefaultSort = "Name",
                DefaultPageSize = 10,
                DefaultPageIndex = 1,
                Table = new TableBuilder<Role>().Add(x => new CheckBox()
                {
                    Name = "ids",
                    Value = ((Role)x.Target).RoleId.ToString()
                }, "").ForEntity(ControllerContext)
                    .Add(x =>
                    {
                        return new ControlCollection()
                        {
                            Controls = new List<Control>
                            {
                                new Link(MaintCultureTextResources.Edit, urlHelper.GetRequestMappingUrl("editrole", new {id = ((Role) x.Target).RoleId}), LinkStyle.Normal)
                            }
                        };
                    }, MaintCultureTextResources.Operation).Build(),

                GlobalButtons = new IClickable[]
                {
                    new Button(MaintCultureTextResources.Search, ""),
                    new Link(MaintCultureTextResources.New, RequestMappings.Current.GetRequestMapping("createrole").MakeUrl(urlHelper, null), LinkStyle.Button),
                    new Button(MaintCultureTextResources.Disable, "Disable"),
                    new Button(MaintCultureTextResources.Enable, "Enable"),
                }
            };
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
            return Template(OnIndex(model));
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
            return Template(OnIndex(model));
        }

        [RequestMapping("createrole", "role/create")]
        [SitemapNode(Text = "创建用户", Parent = "roles")]
        public ActionResult Create()
        {

            return Template(OnCreate(new Role()));
        }

        private Template OnCreate(Role role)
        {
            return new AdministrationSimpleEditTemplate
            {
                Title = MaintCultureTextResources.RoleCreate,
                Description = MaintCultureTextResources.RoleCreateDescription,
                FormTitle = MaintCultureTextResources.RoleInfo,
                Fields = new FieldsBuilder().ForEntity(role, ControllerContext).Build(),
                Buttons = new IClickable[]
                {
                    new Button(MaintCultureTextResources.Save, ""),
                }
            };
        }

        [HttpPost]
        public async Task<ActionResult> Create([Bind(Include = "Name,IsEnabled,PrivilegeArray")] Role role)
        {
            if (!ModelState.IsValid)
            {
                return Template(OnCreate(role));
            }
            var db = MaintDbContext;

            db.Add(role);
            await db.SaveChangesAsync();
            await SetFlashAsync(new FlashMessage
            {
                Text = MaintCultureTextResources.SuccessToSave,
                MessageType = FlashMessageType.Success,
            });
            return RedirectToAction("Create");
        }

        [RequestMapping("editrole", "role/edit/{id}")]
        [SitemapNode(Text = "AdminUserEdit", ResourceType = typeof(MaintCultureTextResources), Parent = "roles")]
        public async Task<ActionResult> Edit(int id)
        {
            var db = MaintDbContext;
            var role = await db.Roles.FirstOrDefaultAsync(x => x.RoleId == id);
            if (role == null)
            {
                return HttpNotFound();
            }


            return Template(OnEdit(role));
        }

        private Template OnEdit(Role role)
        {
            return new AdministrationSimpleEditTemplate
            {
                Title = MaintCultureTextResources.RoleEdit,
                Description = MaintCultureTextResources.RoleEditDescription,
                FormTitle = MaintCultureTextResources.RoleInfo,
                Fields = new FieldsBuilder().ForEntity(role, ControllerContext).Build(),
                Buttons = new IClickable[]
                {
                    new Button
                    {
                        Text = MaintCultureTextResources.Save,
                        ActionName = ""
                    }
                }
            };
        }

        [HttpPost]
        [ActionName("Edit")]
        public async Task<ActionResult> Edit_Postback(int id)
        {
            var role = await MaintDbContext.Roles.FirstOrDefaultAsync(x => x.RoleId == id);
            if (role == null)
            {
                return HttpNotFound();
            }

            if (!TryUpdateModel(role, null, new[] { "Name", "IsEnabled", "PrivilegeArray" }))
            {
                return Template(OnEdit(role));
            }
            await MaintDbContext.SaveChangesAsync();
            await SetFlashAsync(new FlashMessage
            {
                Text = MaintCultureTextResources.SuccessToSave,
                MessageType = FlashMessageType.Success,
            });

            return Template(OnEdit(role));
        }
    }
}