using System.Threading.Tasks;
using System.Web.Mvc;
using Moonlit.Caching;
using Moonlit.Mvc.Maintenance.Models;
using Moonlit.Mvc.Maintenance.Properties;

namespace Moonlit.Mvc.Maintenance.Controllers
{
    [MoonlitAuthorize(Roles = MaintModule.PrivilegeAdminUser )]
    public class SiteController : MaintControllerBase
    {
        private readonly ICacheManager _cacheManager;
        private readonly CacheKeyManager _cacheKeyManager;

        public SiteController(ICacheManager cacheManager, CacheKeyManager cacheKeyManager)
        {
            _cacheManager = cacheManager;
            _cacheKeyManager = cacheKeyManager;
        }

        [RequestMapping("SiteSettings", "Site/Settings")]
        [SitemapNode(Parent = "Site", ResourceType = typeof(CultureTextResources), Text = "SiteSettings")]
        public ActionResult Settings()
        {
            SiteSettingsModel model = new SiteSettingsModel();
            model.SetInnerObject(new SiteModel(MaintDbContext.SystemSettings));
            return Template(model.CreateTemplate(Request.RequestContext, MaintDbContext));
        }
        [RequestMapping("Caches", "Site/Caches")]
        [SitemapNode(Parent = "Site", ResourceType = typeof(CultureTextResources), Text = "Cache")]
        public ActionResult Index(CacheListModel model)
        {
            return Template(model.CreateTemplate(Request.RequestContext, _cacheManager, _cacheKeyManager));
        }
        [RequestMapping("Caches_Clear", "Site/Caches")]
        [HttpPost]
        [FormAction("Clear")]
        public ActionResult ClearCache(CacheListModel model)
        {
            foreach (var cacheKey in _cacheKeyManager.AllKeys)
            {
                _cacheManager.Remove(cacheKey);
            }
            return Template(model.CreateTemplate(Request.RequestContext, _cacheManager, _cacheKeyManager));
        }
        [RequestMapping("SiteSettings_postback", "Site/Settings")]
        [HttpPost]
        public async Task<ActionResult> Settings(SiteSettingsModel model)
        {
            if (!ModelState.IsValid)
            {
                return Template(model.CreateTemplate(Request.RequestContext, MaintDbContext));
            }
            var db = MaintDbContext;
            var siteModel = new SiteModel(db.SystemSettings);
            siteModel.SiteName = model.SiteName;
            siteModel.DefaultCulture = (int)model.DefaultCulture;
            siteModel.Save(db);
            await db.SaveChangesAsync();
            MaintDomainService.ClearSystemSettingsCache();
            await SetFlashAsync(new FlashMessage
            {
                Text = CultureTextResources.SuccessToSave,
                MessageType = FlashMessageType.Success,
            });
            return RedirectToAction("Settings");
        }
    }
}