using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Moonlit.Mvc.Maintenance.Domains;
using Moonlit.Mvc.Maintenance.Models;
using Moonlit.Mvc.Maintenance.Properties;

namespace Moonlit.Mvc.Maintenance.Controllers
{
    [MoonlitAuthorize(Roles = MaintModule.PrivilegeCulture)]
    public class CultureController : MaintControllerBase
    {

        [SitemapNode(Parent = "BasicData", Text = "CultureList", ResourceType = typeof(MaintCultureTextResources))]
        public ActionResult Index(CultureListModel model)
        {
            return Template(model.CreateTemplate(ControllerContext, MaintDbContext));
        }
        [FormAction("Disable")]
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
            return Template(model.CreateTemplate(ControllerContext, MaintDbContext));
        }
        [FormAction("Enable")]
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
            return Template(model.CreateTemplate(ControllerContext, MaintDbContext));
        }

        [SitemapNode(Text = "CultureTextCreate", Parent = "cultures", ResourceType = typeof(MaintCultureTextResources))]
        public ActionResult Create()
        {
            var model = new CultureCreateModel();
            return Template(model.CreateTemplate(ControllerContext));
        }

        [HttpPost]
        public async Task<ActionResult> Create(CultureCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                return Template(model.CreateTemplate(ControllerContext));
            }
            var db = MaintDbContext;
            var name = model.Name.Trim();
            var culture = await db.Cultures.FirstOrDefaultAsync(x => x.Name == name);
            if (culture != null)
            {
                var errorMessage = string.Format(MaintCultureTextResources.ValidationDumplicate,
                    MaintCultureTextResources.CultureName, name);

                ModelState.AddModelError("Name", string.Format(errorMessage, name));
                return Template(model.CreateTemplate(ControllerContext));
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
                Text = MaintCultureTextResources.SuccessToSave,
                MessageType = FlashMessageType.Success,
            });
            return RedirectToAction("Create");
        }

        [SitemapNode(Text = "CultureTextEdit", Parent = "cultures", ResourceType = typeof(MaintCultureTextResources))]
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

            return Template(model.CreateTemplate(ControllerContext));
        } 
        [HttpPost]
        public async Task<ActionResult> Edit(CultureEditModel model, int id)
        {
            if (!ModelState.IsValid)
            {
                return Template(model.CreateTemplate(ControllerContext));
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
                Text = MaintCultureTextResources.SuccessToSave,
                MessageType = FlashMessageType.Success,
            });

            return Template(model.CreateTemplate(ControllerContext));
        }
    }
}