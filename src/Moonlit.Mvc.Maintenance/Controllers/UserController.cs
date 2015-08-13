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

namespace Moonlit.Mvc.Maintenance.Controllers
{
    [MoonlitAuthorize(Roles = MaintModule.PrivilegeAdminUser)]
    public class UserController : MaintControllerBase
    {
        [RequestMapping("users", "user")]
        [SitemapNode(Parent = "BasicData", ResourceType = typeof(MaintCultureTextResources), Text = "AdminUserList")]
        [Display(Name = "用户管理", Description = "用户管理描述，这是一段很长的描述")]
        public ActionResult Index(AdminUserListModel model)
        {
            return Template(OnIndex(model));
        }

        private Template OnIndex(AdminUserListModel model)
        {
            var irepository = DependencyResolver.Current.GetService<IMaintDbRepository>();
            var query = irepository.Users.AsQueryable();
            if (!string.IsNullOrWhiteSpace(model.Keyword))
            {
                var keyword = model.Keyword.Trim();

                query = query.Where(x => x.LoginName.StartsWith(keyword) || x.UserName.StartsWith(keyword));
            }
            if (!string.IsNullOrWhiteSpace(model.UserName))
            {
                var userName = model.UserName.Trim();

                query = query.Where(x => x.UserName.StartsWith(userName));
            }
            if (model.IsEnabled != null)
            {
                query = query.Where(x => x.IsEnabled == model.IsEnabled);
            }
            var template = new AdministrationSimpleListTemplate(query)
            {
                Title = MaintCultureTextResources.AdminUserList,
                Description = MaintCultureTextResources.AdminUserListDescription,
                QueryPanelTitle = MaintCultureTextResources.PanelQuery,
                DefaultSort = model.OrderBy,
                DefaultPageSize = model.PageSize,
                Criteria = new FieldsBuilder().ForEntity(model, ControllerContext).Build(),
            };
            var urlHelper = new UrlHelper(this.ControllerContext.RequestContext);
            template.GlobalButtons = new IClickable[]
            {
                new Button(MaintCultureTextResources.Search, ""),
                new Link(MaintCultureTextResources.New, urlHelper.GetRequestMappingUrl("CreateUser"), LinkStyle.Button),
                new Button(MaintCultureTextResources.Disable, "Disable"),
                new Button(MaintCultureTextResources.Enable, "Enable"),
            };
            template.Table = new TableBuilder<User>().ForEntity(ControllerContext).Add(x => new ControlCollection()
            {
                Controls = new List<Control> {
                    new Link(MaintCultureTextResources.Edit, urlHelper.GetRequestMappingUrl("editUser", new { id = x.Target.UserId }), LinkStyle.Normal),
                }
            }, MaintCultureTextResources.Operation).Build();
            return template;
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
            return Template(OnIndex(model));
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
            return Template(OnIndex(model));
        }

        [RequestMapping("createuser", "user/create")]
        [SitemapNode(Text = "创建用户", Parent = "users")]
        public ActionResult Create()
        {
            var template = OnCreate(new User());
            return Template(template);
        }

        private AdministrationSimpleEditTemplate OnCreate(User user)
        {
            var template = new AdministrationSimpleEditTemplate
            {
                Title = MaintCultureTextResources.AdminUserCreate,
                Description = MaintCultureTextResources.AdminUserCreateDescription,
                FormTitle = MaintCultureTextResources.AdminUserInfo,
                Fields = new FieldsBuilder().ForEntity(user, ControllerContext).Build(),
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
        public async Task<ActionResult> Create([Bind(Include = "UserName,LoginName,Password,Gender,DateOfBirth,IsEnabled,CultureId")] User user, int[] roles)
        {
            if (!ModelState.IsValid)
            {
                return Template(OnCreate(user));
            }
            var db = MaintDbContext;
            var loginName = user.LoginName.Trim();
            if (await db.Users.AnyAsync(x => x.LoginName == loginName))
            {
                var errorMessage = string.Format(MaintCultureTextResources.ValidationDumplicate,
                    MaintCultureTextResources.AdminUserLoginName, loginName);
                ModelState.AddModelError("LoginName", string.Format(errorMessage, loginName));
                return Template(OnCreate(user));
            }

            user.Password = user.HashPassword(user.Password ?? "");
            if (roles != null)
            {
                user.Roles = db.Roles.Where(x => roles.Contains(x.RoleId)).ToList();
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

        [RequestMapping("editUser", "user/edit/{id}")]
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