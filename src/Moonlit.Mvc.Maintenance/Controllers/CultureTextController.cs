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
    [Authorize(Roles = MaintModule.PrivilegeCultureText, Order = 1000)]
    public class CultureTextController : MaintControllerBase
    {
        private readonly IMaintDomainService _maintDomainService;

        public CultureTextController(IMaintDomainService maintDomainService)
        {
            _maintDomainService = maintDomainService;
        }

        [RequestMapping("culturetexts", "culturetext")]
        [SitemapNode(Text = "CultureTextList", Parent = "BasicData", ResourceType = typeof(CultureTextResources))]
        public ActionResult Index(CultureTextListModel model)
        {
            return Template(model.CreateTemplate(Request.RequestContext, MaintDbContext));
        }
        [RequestMapping("culturetexts_delete", "culturetext")]
        [FormAction("delete")]
        [ActionName("Index")]
        [HttpPost]
        public ActionResult Delete(CultureTextListModel model, int[] ids)
        {
            if (ids != null && ids.Length > 0)
            {
                foreach (var cultureText in MaintDbContext.CultureTexts.Where(x => ids.Contains(x.CultureTextId)).ToList())
                {
                    MaintDbContext.Remove(cultureText);
                }
                MaintDbContext.SaveChanges();
                _maintDomainService.ClearCultureTextsCache();
            }
            return Template(model.CreateTemplate(Request.RequestContext, MaintDbContext));
        }
        [RequestMapping("culturetexts_Export", "culturetext")]
        [FormAction("Export")]
        [ActionName("Index")]
        [HttpPost]
        public ActionResult Export(CultureTextListModel model, int[] ids)
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

        [RequestMapping("importculturetext", "culturetext/import")]
        [SitemapNode(Text = "CultureTextImport", Parent = "culturetexts", ResourceType = typeof(CultureTextResources))]
        public ActionResult Import()
        {
            var model = new CultureTextImportModel();
            return Template(model.CreateTemplate(Request.RequestContext, MaintDbContext));
        }

        [RequestMapping("importculturetext_postback", "culturetext/import")]
        [HttpPost]
        public async Task<ActionResult> Import(CultureTextImportModel model)
        {
            if (!ModelState.IsValid)
            {
                return Template(model.CreateTemplate(Request.RequestContext, MaintDbContext));
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
                    db.Add(oldCultureText);
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
                Text = CultureTextResources.SuccessToSave,
                MessageType = FlashMessageType.Success,
            });
            return Template(model.CreateTemplate(Request.RequestContext, MaintDbContext));
        }
        [RequestMapping("createculturetext", "culturetext/create")]
        [SitemapNode(Text = "CultureTextCreate", Parent = "culturetexts", ResourceType = typeof(CultureTextResources))]
        public ActionResult Create()
        {
            var model = new CultureTextCreateModel();
            return Template(model.CreateTemplate(Request.RequestContext, MaintDbContext));
        }

        [RequestMapping("createculturetext_postback", "culturetext/create")]
        [HttpPost]
        public async Task<ActionResult> Create(CultureTextCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                return Template(model.CreateTemplate(Request.RequestContext, MaintDbContext));
            }
            var db = MaintDbContext;
            var name = model.Name.Trim();
            var cultureText = await db.CultureTexts.FirstOrDefaultAsync(x => x.Name == name);
            if (cultureText != null)
            {
                var errorMessage = string.Format(CultureTextResources.ValidationDumplicate,
                    CultureTextResources.CultureTextName, name);

                ModelState.AddModelError("Name", string.Format(errorMessage, name));
                return Template(model.CreateTemplate(Request.RequestContext, MaintDbContext));
            }
            var culture = db.Cultures.FirstOrDefault(x => x.CultureId == model.Culture && x.IsEnabled);
            if (culture == null)
            {
                return HttpNotFound();
            }
            cultureText = new CultureText
            {
                Name = name,
                Text = model.Text.Trim(),
                CultureId = culture.CultureId,
            };

            db.Add(cultureText);
            await db.SaveChangesAsync();
            await SetFlashAsync(new FlashMessage
            {
                Text = CultureTextResources.SuccessToSave,
                MessageType = FlashMessageType.Success,
            });
            _maintDomainService.ClearCultureTextsCache();
            return RedirectToAction("Create");
        }

        [RequestMapping("editculturetext", "culturetext/edit/{id}")]
        [SitemapNode(Text = "CultureTextEdit", Parent = "culturetexts", ResourceType = typeof(CultureTextResources))]
        public async Task<ActionResult> Edit(int id)
        {
            var db = MaintDbContext;
            var adminUser = await db.CultureTexts.FirstOrDefaultAsync(x => x.CultureTextId == id) ;
            if (adminUser == null)
            {
                return HttpNotFound();
            }
            var model = new CultureTextEditModel();
            model.SetInnerObject(adminUser);

            return Template(model.CreateTemplate(Request.RequestContext, MaintDbContext));
        }
        [RequestMapping("editculturetext_postback", "culturetext/edit/{id}")]
        [HttpPost]
        [SitemapNode(Text = "编辑词条", Parent = "culturetexts")]
        public async Task<ActionResult> Edit(CultureTextEditModel model, int id)
        {
            if (!ModelState.IsValid)
            {
                return Template(model.CreateTemplate(Request.RequestContext, MaintDbContext));
            }
            var db = MaintDbContext;
            var cultureText = await db.CultureTexts.FirstOrDefaultAsync(x => x.CultureTextId == id);
            if (cultureText == null)
            {
                return HttpNotFound();
            }
            cultureText.Text = model.Text.Trim();
            await db.SaveChangesAsync();
            await SetFlashAsync(new FlashMessage
            {
                Text = CultureTextResources.SuccessToSave,
                MessageType = FlashMessageType.Success,
            });
            _maintDomainService.ClearCultureTextsCache();
            return Template(model.CreateTemplate(Request.RequestContext, MaintDbContext));
        }
    }
}