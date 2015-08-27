using System.Threading.Tasks;
using System.Web.Mvc;
using Moonlit.Caching;
using Moonlit.Mvc.Maintenance.Models;
using Moonlit.Mvc.Maintenance.Properties;

namespace Moonlit.Mvc.Maintenance.Controllers
{
    [MoonlitAuthorize(Roles = MaintPrivileges.PrivilegeAdminUser)]
    public class SiteController : MaintControllerBase
    {
        private readonly ICacheManager _cacheManager;
        private readonly CacheKeyManager _cacheKeyManager;

        public SiteController(ICacheManager cacheManager, CacheKeyManager cacheKeyManager)
        {
            _cacheManager = cacheManager;
            _cacheKeyManager = cacheKeyManager;
        }


        [SitemapNode(Parent = "Site", ResourceType = typeof(MaintCultureTextResources), Text = "SiteSettings")]
        public ActionResult Settings()
        {
            SiteSettingsModel model = new SiteSettingsModel();
            model.FromEntity(new SiteModel(MaintDbContext.SystemSettings), false, ControllerContext);
            return Template(model.CreateTemplate(ControllerContext));
        }


        [SitemapNode(Parent = "Site", ResourceType = typeof(MaintCultureTextResources), Text = "Cache")]
        public ActionResult Cache(CacheIndexModel model)
        {
            return Template(model.CreateTemplate(ControllerContext, _cacheManager, _cacheKeyManager));
        }

        public const string FormActionNameClear = "clear";

        [FormAction(FormActionNameClear)]
        [ActionName("Cache")]
        [HttpPost]
        public ActionResult ClearCache(CacheIndexModel model)
        {
            foreach (var cacheKey in _cacheKeyManager.AllKeys)
            {
                _cacheManager.Remove(cacheKey);
            }
            return Template(model.CreateTemplate(ControllerContext, _cacheManager, _cacheKeyManager));
        }
        [HttpPost]
        public async Task<ActionResult> Settings(SiteSettingsModel model)
        {
            var siteModel = new SiteModel(MaintDbContext.SystemSettings);
            model.FromEntity(siteModel, true, ControllerContext);
            if (!TryUpdateModel(siteModel, model))
            {
                return Template(model.CreateTemplate(ControllerContext));
            }
            var db = MaintDbContext;
            siteModel.Save(db);
            await db.SaveChangesAsync();
            MaintDomainService.ClearSystemSettingsCache();
            await SetFlashAsync(new FlashMessage
            {
                Text = MaintCultureTextResources.SuccessToSave,
                MessageType = FlashMessageType.Success,
            });
            return Settings();
        }
    }
}