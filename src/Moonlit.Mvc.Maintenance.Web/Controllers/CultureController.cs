using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Moonlit.Mvc.Maintenance.Domains;
using Moonlit.Mvc.Maintenance.Models;
using Moonlit.Mvc.Maintenance.Properties;

namespace Moonlit.Mvc.Maintenance.Controllers
{
    [MoonlitAuthorize(Roles = MaintPrivileges.PrivilegeCulture)]
    public class CultureController : MaintControllerBase
    {

        [SitemapNode(Parent = "BasicData", Name = "Cultures", Text = "CultureIndex", ResourceType = typeof(MaintCultureTextResources))]
        public ActionResult Index(CultureIndexModel model)
        {
            return Template(model.CreateTemplate(ControllerContext));
        }
        [FormAction("Disable")]
        [ActionName("Index")]
        [HttpPost]
        public ActionResult Disable(CultureIndexModel model, int[] ids)
        {
            if (ids != null && ids.Length > 0)
            {
                foreach (var culture in MaintDbContext.Cultures.Where(x => x.IsEnabled && ids.Contains(x.CultureId)).ToList())
                {
                    culture.IsEnabled = false;
                }
                MaintDbContext.SaveChanges();
            }
            return Template(model.CreateTemplate(ControllerContext));
        }
        [FormAction("Enable")]
        [ActionName("Index")]
        [HttpPost]
        public ActionResult Enable(CultureIndexModel model, int[] ids)
        {
            if (ids != null && ids.Length > 0)
            {
                foreach (var adminUser in MaintDbContext.Cultures.Where(x => !x.IsEnabled && ids.Contains(x.CultureId)).ToList())
                {
                    adminUser.IsEnabled = true;
                }
                MaintDbContext.SaveChanges();
            }
            return Template(model.CreateTemplate(ControllerContext));
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
            var entity = new Culture();
            if (!TryUpdateModel(entity, model))
            {
                return Template(model.CreateTemplate(ControllerContext));
            }
            var db = MaintDbContext;
            var name = model.Name.Trim(); ;
            if (db.Cultures.Any(x => x.Name == name))
            {
                var errorMessage = string.Format(MaintCultureTextResources.ValidationDumplicate,
                    MaintCultureTextResources.CultureName, name);

                ModelState.AddModelError("Name", string.Format(errorMessage, name));
                return Template(model.CreateTemplate(ControllerContext));
            }

            db.Cultures.Add(entity);
            await db.SaveChangesAsync();
            await SetFlashAsync(new FlashMessage
            {
                Text = MaintCultureTextResources.SuccessToSave,
                MessageType = FlashMessageType.Success,
            });
            return Create( );
        }

        [SitemapNode(Text = "CultureTextEdit", Parent = "cultures", ResourceType = typeof(MaintCultureTextResources))]
        public async Task<ActionResult> Edit(int id)
        {
            var db = MaintDbContext;
            var entity = await db.Cultures.FirstOrDefaultAsync(x => x.CultureId == id);
            if (entity == null)
            {
                return HttpNotFound();
            }
            var model = new CultureEditModel();
            model.FromEntity(entity, false, ControllerContext);

            return Template(model.CreateTemplate(ControllerContext));
        }
        [HttpPost]
        public async Task<ActionResult> Edit(CultureEditModel model, int id)
        {
            var db = MaintDbContext;
            var entity = await db.Cultures.FirstOrDefaultAsync(x => x.CultureId == id);
            if (entity == null)
            {
                return HttpNotFound();
            }
            model.FromEntity(entity, true, ControllerContext);

            if (!TryUpdateModel(entity, model))
            {
                return Template(model.CreateTemplate(ControllerContext));
            }
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