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
        [SitemapNode(Parent = "Site", ResourceType = typeof(MaintCultureTextResources), Text = "SiteSettings")]
        public ActionResult Settings()
        {
            SiteSettingsModel model = new SiteSettingsModel();
            model.FromEntity(new SiteModel(Database.SystemSettings), false, ControllerContext);
            return Template(model.CreateTemplate(ControllerContext));
        }


        public const string FormActionNameClear = "clear";

        [HttpPost]
        public async Task<ActionResult> Settings(SiteSettingsModel model)
        {
            var siteModel = new SiteModel(Database.SystemSettings);
            model.FromEntity(siteModel, true, ControllerContext);
            if (!TryUpdateModel(siteModel, model))
            {
                return Template(model.CreateTemplate(ControllerContext));
            }
            var db = Database;
            siteModel.Save(db);
            await db.SaveChangesAsync(); 
            await SetFlashAsync(new FlashMessage
            {
                Text = MaintCultureTextResources.SuccessToSave,
                MessageType = FlashMessageType.Success,
            });
            return Settings();
        }
    }
}