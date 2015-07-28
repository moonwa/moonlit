using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using Moonlit.Mvc.Controls;
using Moonlit.Mvc.Maintenance.Domains;
using Moonlit.Mvc.Maintenance.Properties;
using Moonlit.Mvc.Maintenance.SelectListItemsProviders;
using Moonlit.Mvc.Templates;
using SelectList = Moonlit.Mvc.Controls.SelectList;

namespace Moonlit.Mvc.Maintenance.Models
{
    public class SiteSettingsModel 
    {
        public void SetInnerObject(SiteModel siteModel)
        {
            SiteName = siteModel.SiteName;
            MaxSignInFailTimes = siteModel.MaxSignInFailTimes;
            DefaultCulture = siteModel.DefaultCulture;
        }

        [TextBox]
        [Display(ResourceType = typeof(MaintCultureTextResources), Name = "SiteSiteName")]
        [Required(ErrorMessageResourceName = "ValidationRequired", ErrorMessageResourceType = typeof(MaintCultureTextResources))]
        [Field(FieldWidth.W6)]
        public string SiteName { get; set; }
        [SelectList(typeof(CultureSelectListItemsProvider))]
        [Field(FieldWidth.W6)]
        [Display(ResourceType = typeof(MaintCultureTextResources), Name = "SiteDefaultCulture")]
        [Required(ErrorMessageResourceName = "ValidationRequired", ErrorMessageResourceType = typeof(MaintCultureTextResources))]
        public int? DefaultCulture { get; set; }

        [TextBox]
        [Display(ResourceType = typeof(MaintCultureTextResources), Name = "SiteMaxSignInFailTimes")]
        [Required(ErrorMessageResourceName = "ValidationRequired", ErrorMessageResourceType = typeof(MaintCultureTextResources))]
        [Field(FieldWidth.W6)]
        public int MaxSignInFailTimes { get; set; }
        public Template CreateTemplate(ControllerContext controllerContext)
        {
            return new AdministrationSimpleEditTemplate
            {
                Title = MaintCultureTextResources.SiteSettings,
                Description = MaintCultureTextResources.SiteSettingsDescription,
                FormTitle = MaintCultureTextResources.SiteInfo,
                Fields = TemplateHelper.MakeFields(this, controllerContext),
                Buttons = new IClickable[]
                {
                    new Button
                    {
                        Text = MaintCultureTextResources.Save,
                        ActionName = ""
                    }
                }
            };
        }
    }
}