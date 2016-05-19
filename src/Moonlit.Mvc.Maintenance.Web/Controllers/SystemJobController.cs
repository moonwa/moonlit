using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Web.Mvc;
using Moonlit.Mvc.Maintenance.Domains;
using Moonlit.Mvc.Maintenance.Models;
using Moonlit.Mvc.Maintenance.Properties;

namespace Moonlit.Mvc.Maintenance.Controllers
{
    public class SystemJobController : MaintControllerBase
    {
        [MoonlitAuthorize(Roles = MaintPrivileges.PrivilegeSite)]
        [SitemapNode(Text = "SystemJobIndex", Name = "SystemJob", Parent = "Site", ResourceType = typeof(MaintCultureTextResources))]
        public ActionResult Index(SystemJobIndexModel model)
        {
            return Template(model.CreateTemplate(ControllerContext, Database));
        }

        [MoonlitAuthorize(Roles = MaintPrivileges.PrivilegeSite)]
        [SitemapNode(Text = "SystemJobEdit", Parent = "SystemJobs", ResourceType = typeof (MaintCultureTextResources))]
        public ActionResult Edit(long id)
        {
            var entity = Database.SystemJobs.FirstOrDefault(x=>x.SystemJobId == id);
            if (entity == null )
            {
                return HttpNotFound();
            }
            var model = new SystemJobEditModel();
            model.FromEntity(entity, false, ControllerContext);
            return Template(model.CreateTemplate(ControllerContext));
        }



        [MoonlitAuthorize(Roles = MaintPrivileges.PrivilegeSite)]
        [HttpPost]
        public async Task<ActionResult> Edit(SystemJobEditModel model, long id)
        {
            var entity = Database.SystemJobs.FirstOrDefault(x => x.SystemJobId == id);
            if (entity == null /*TODO: check entity should be edit*/)
            {
                return HttpNotFound();
            }
            model.FromEntity(entity, true, ControllerContext);
            if (!TryUpdateModel(entity, model))
            {
                return Template(model.CreateTemplate(ControllerContext));
            }
            using (var trans = new TransactionScope())
            {
                await Database.SaveChangesAsync();
                trans.Complete();
            }
            await SetFlashAsync(new FlashMessage
            {
                Text = MaintCultureTextResources.SuccessToSave,
                MessageType = FlashMessageType.Success,
            });
            return Template(model.CreateTemplate(ControllerContext));
        }

        [ActionName("Index")]
        [HttpPost]
        [FormAction("Abort")]
        public async Task<ActionResult> Abort(SystemJobIndexModel model, long[] ids)
        {
            foreach (var item in Database.SystemJobs.Where(x => ids.Contains(x.SystemJobId) && x.Status == SystemJobStatus.Init))
            {
                 item.Status = SystemJobStatus.Abort;
            }
            await Database.SaveChangesAsync();
            await SetFlashAsync(new FlashMessage
            {
                Text = MaintCultureTextResources.SuccessToSave,
                MessageType = FlashMessageType.Success,
            });

            return Index(model);
        }
    }
}
