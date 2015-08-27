using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Moonlit.Mvc.Maintenance.Domains;
using Moonlit.Mvc.Maintenance.Models;
using Moonlit.Mvc.Maintenance.Properties;
using Moonlit.Mvc.Maintenance.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Moonlit.Mvc.Maintenance.Controllers
{
    [MoonlitAuthorize(Roles = MaintPrivileges.PrivilegeCultureText)]
    public class CultureTextController : MaintControllerBase
    { 
        private readonly IMaintDomainService _maintDomainService;

        public CultureTextController(IMaintDomainService maintDomainService)
        {
            _maintDomainService = maintDomainService;
        }

        [SitemapNode(Text = "CultureTextIndex", Name = "CultureTexts", Parent = "BasicData", ResourceType = typeof(MaintCultureTextResources))]
        public ActionResult Index(CultureTextIndexModel model)
        {
            return Template(model.CreateTemplate(ControllerContext));
        }
        [FormAction("Delete")]
        [ActionName("index")]
        [HttpPost]
        public ActionResult Delete(CultureTextIndexModel model, int[] ids)
        {
            if (ids != null && ids.Length > 0)
            {
                foreach (var cultureText in MaintDbContext.CultureTexts.Where(x => ids.Contains(x.CultureTextId)).ToList())
                {
                    MaintDbContext.CultureTexts.Remove(cultureText);
                }
                MaintDbContext.SaveChanges();
                _maintDomainService.ClearCultureTextsCache();
            }
            return Template(model.CreateTemplate(ControllerContext));
        }

        [FormAction("Export")]
        [ActionName("Index")]
        [HttpPost]
        public ActionResult Export(CultureTextIndexModel model, int[] ids)
        {
            var db = MaintDbContext;

            var culture = db.Cultures.FirstOrDefault(x => x.CultureId == (int?)model.Culture);
            if (culture == null)
            {
                throw new Exception("请先设置语言");
            }
            var cultureTexts = db.CultureTexts.Where(x => x.CultureId == (int?)model.Culture && x.Text != null);
            var obj = cultureTexts.ToList().ToDictionary(x => x.Name, x => x.Text);
            var text = JsonConvert.SerializeObject(obj);

            return File(Encoding.UTF8.GetBytes(text), "text/plain", culture.Name + ".lang");
        }

        [SitemapNode(Text = "CultureTextImport", Parent = "culturetexts", ResourceType = typeof(MaintCultureTextResources))]
        public ActionResult Import()
        {
            var model = new CultureTextImportModel();
            return Template(model.CreateTemplate(ControllerContext));
        }

        [HttpPost]
        public async Task<ActionResult> Import(CultureTextImportModel model)
        {
            if (!ModelState.IsValid)
            {
                return Template(model.CreateTemplate(ControllerContext));
            }
            var db = MaintDbContext;


            var culture = db.Cultures.FirstOrDefault(x => x.IsEnabled && x.CultureId == (int?)model.Culture);
            var cultureTexts = db.CultureTexts.Where(x => x.CultureId == (int?)model.Culture).ToList();
            var newCultureTexts = JsonConvert.DeserializeObject(model.Content) as JObject;
            foreach (KeyValuePair<string, JToken> newCultureText in newCultureTexts)
            {
                var oldCultureText = cultureTexts.FirstOrDefault(x => string.Equals(x.Name, newCultureText.Key, StringComparison.OrdinalIgnoreCase));
                if (oldCultureText == null)
                {
                    oldCultureText = new CultureText
                    {
                        Text = newCultureText.Value.ToString(),
                        CultureId = culture.CultureId,
                        Name = newCultureText.Key,
                    };
                    db.CultureTexts.Add(oldCultureText);
                }
                else
                {
                    if (model.Overwrite)
                    {
                        oldCultureText.Text = newCultureText.Value.ToString();
                    }
                }
            }
            await db.SaveChangesAsync();
            _maintDomainService.ClearCultureTextsCache();
            await SetFlashAsync(new FlashMessage
            {
                Text = MaintCultureTextResources.SuccessToSave,
                MessageType = FlashMessageType.Success,
            });
            return Template(model.CreateTemplate(ControllerContext));
        }

        [SitemapNode(Text = "CultureTextCreate", Parent = "culturetexts", ResourceType = typeof(MaintCultureTextResources))]
        public ActionResult Create(int cultureId = 0)
        {
            var model = new CultureTextCreateModel();
            model.CultureId = cultureId;
            return Template(model.CreateTemplate(ControllerContext));
        }
         
        [HttpPost]
        public async Task<ActionResult> Create(CultureTextCreateModel model)
        {
            var db = MaintDbContext;
            CultureText cultureText= new CultureText();
            if (!TryUpdateModel(cultureText, model))
            {
                return Template(model.CreateTemplate(ControllerContext));
            }
            var name = model.Name;
            if (await db.CultureTexts.AnyAsync(x => x.Name == name))
            {
                var errorMessage = string.Format(MaintCultureTextResources.ValidationDumplicate,
                    MaintCultureTextResources.CultureTextName, name);

                ModelState.AddModelError("Name", string.Format(errorMessage, name));
                return Template(model.CreateTemplate(ControllerContext));
            }
            var culture = db.Cultures.FirstOrDefault(x => x.CultureId == model.CultureId && x.IsEnabled);
            if (culture == null)
            {
                return HttpNotFound();
            }

            db.CultureTexts.Add(cultureText);
            await db.SaveChangesAsync();
            await SetFlashAsync(new FlashMessage
            {
                Text = MaintCultureTextResources.SuccessToSave,
                MessageType = FlashMessageType.Success,
            });
            _maintDomainService.ClearCultureTextsCache();
            return Create( );
        }

        [SitemapNode(Text = "CultureTextEdit", Parent = "culturetexts", ResourceType = typeof(MaintCultureTextResources))]
        public async Task<ActionResult> Edit(int id)
        {
            var db = MaintDbContext;
            var entity = await db.CultureTexts.FirstOrDefaultAsync(x => x.CultureTextId == id) ;
            if (entity == null)
            {
                return HttpNotFound();
            }
            var model = new CultureTextEditModel();
            model.FromEntity(entity, false, ControllerContext);

            return Template(model.CreateTemplate(ControllerContext));
        } 
        [HttpPost]
        [SitemapNode(Text = "编辑词条", Parent = "culturetexts")]
        public async Task<ActionResult> Edit(CultureTextEditModel model, int id)
        {
            var db = MaintDbContext;
            var entity = await db.CultureTexts.FirstOrDefaultAsync(x => x.CultureTextId == id);
            if (entity == null)
            {
                return HttpNotFound();
            }
            model.FromEntity(entity, true,ControllerContext);
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
            _maintDomainService.ClearCultureTextsCache();
            return Template(model.CreateTemplate(ControllerContext));
        }
    }
}