using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Moonlit.Mvc.Maintenance.Domains;
using Moonlit.Mvc.Maintenance.Models;
using Moonlit.Mvc.Maintenance.Properties;

namespace Moonlit.Mvc.Maintenance.Controllers
{
    [Authorize(Roles = MaintModule.PrivilegeCulture, Order = 1000)]
    public class CultureController : MaintControllerBase
    {
        [RequestMapping("cultures", "culture")]
        [SitemapNode(Parent = "BasicData", Text = "CultureList", ResourceType = typeof(CultureTextResources))]
        public ActionResult Index(CultureListModel model)
        {
            return Template(model.CreateTemplate(Request.RequestContext, MaintDbContext));
        }
        [RequestMapping("cultures_disable", "culture")]
        [FormAction("disable")]
        [ActionName("Index")]
        [HttpPost]
        public ActionResult Disable(CultureListModel model, int[] ids)
        {
            if (ids != null && ids.Length > 0)
            {
                foreach (var culture in MaintDbContext.Cultures.Where(x => x.IsEnabled && ids.Contains(x.CultureId)).ToList())
                {
                    culture.IsEnabled = false;
                }
                MaintDbContext.SaveChanges();
            }
            return Template(model.CreateTemplate(Request.RequestContext, MaintDbContext));
        }
        [RequestMapping("cultures_enable", "culture")]
        [FormAction("enable")]
        [ActionName("Index")]
        [HttpPost]
        public ActionResult Enable(CultureListModel model, int[] ids)
        {
            if (ids != null && ids.Length > 0)
            {
                foreach (var adminUser in MaintDbContext.Cultures.Where(x => !x.IsEnabled && ids.Contains(x.CultureId)).ToList())
                {
                    adminUser.IsEnabled = true;
                }
                MaintDbContext.SaveChanges();
            }
            return Template(model.CreateTemplate(Request.RequestContext, MaintDbContext));
        }

        [RequestMapping("createculture", "culture/create")]
        [SitemapNode(Text = "CultureTextCreate", Parent = "cultures", ResourceType = typeof(CultureTextResources))]
        public ActionResult Create()
        {
            var model = new CultureCreateModel();
            return Template(model.CreateTemplate(Request.RequestContext));
        }

        [RequestMapping("createculture_postback", "culture/create")]
        [HttpPost]
        public async Task<ActionResult> Create(CultureCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                return Template(model.CreateTemplate(Request.RequestContext));
            }
            var db = MaintDbContext;
            var name = model.Name.Trim();
            var culture = await db.Cultures.FirstOrDefaultAsync(x => x.Name == name);
            if (culture != null)
            {
                var errorMessage = string.Format(CultureTextResources.ValidationDumplicate,
                    CultureTextResources.CultureName, name);

                ModelState.AddModelError("Name", string.Format(errorMessage, name));
                return Template(model.CreateTemplate(Request.RequestContext));
            }

            culture = new Culture
            {
                Name = name,
                DisplayName = model.DisplayName.Trim(),
                IsEnabled = model.IsEnabled,
            };

            db.Add(culture);
            await db.SaveChangesAsync();
            await SetFlashAsync(new FlashMessage
            {
                Text = CultureTextResources.SuccessToSave,
                MessageType = FlashMessageType.Success,
            });
            return RedirectToAction("Create");
        }

        [RequestMapping("editculture", "culture/edit/{id}")]
        [SitemapNode(Text = "CultureTextEdit", Parent = "cultures", ResourceType = typeof(CultureTextResources))]
        public async Task<ActionResult> Edit(int id)
        {
            var db = MaintDbContext;
            var adminUser = await db.Cultures.FirstOrDefaultAsync(x => x.CultureId == id);
            if (adminUser == null)
            {
                return HttpNotFound();
            }
            var model = new CultureEditModel();
            model.SetInnerObject(adminUser);

            return Template(model.CreateTemplate(Request.RequestContext));
        }
        [RequestMapping("editculture_postback", "culture/edit/{id}")]
        [HttpPost]
        public async Task<ActionResult> Edit(CultureEditModel model, int id)
        {
            if (!ModelState.IsValid)
            {
                return Template(model.CreateTemplate(Request.RequestContext));
            }
            var db = MaintDbContext;
            var adminUser = await db.Cultures.FirstOrDefaultAsync(x => x.CultureId == id);
            if (adminUser == null)
            {
                return HttpNotFound();
            }
            adminUser.DisplayName = model.DisplayName;
            adminUser.Name = model.Name.Trim();
            adminUser.IsEnabled = model.IsEnabled;
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