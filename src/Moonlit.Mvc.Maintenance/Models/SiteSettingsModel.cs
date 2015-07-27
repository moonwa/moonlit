using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using Moonlit.Mvc.Controls;
using Moonlit.Mvc.Maintenance.Domains;
using Moonlit.Mvc.Maintenance.Properties;
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

        [Display(ResourceType = typeof(MaintCultureTextResources), Name = "SiteSiteName")]
        [Required(ErrorMessageResourceName = "ValidationRequired", ErrorMessageResourceType = typeof(MaintCultureTextResources))]
        public string SiteName { get; set; }

        [Display(ResourceType = typeof(MaintCultureTextResources), Name = "SiteDefaultCulture")]
        [Required(ErrorMessageResourceName = "ValidationRequired", ErrorMessageResourceType = typeof(MaintCultureTextResources))]
        public int? DefaultCulture { get; set; }

        [Display(ResourceType = typeof(MaintCultureTextResources), Name = "SiteMaxSignInFailTimes")]
        [Required(ErrorMessageResourceName = "ValidationRequired", ErrorMessageResourceType = typeof(MaintCultureTextResources))]
        public int MaxSignInFailTimes { get; set; }
        public Template CreateTemplate(RequestContext requestContext, IMaintDbRepository db)
        {
            var cultures = db.Cultures.Where(x => x.IsEnabled).ToList().Select(x => new SelectListItem()
            {
                Text = x.DisplayName,
                Value = x.CultureId.ToString(),
            }).ToList();
            cultures.Insert(0, new SelectListItem());
            return new AdministrationSimpleEditTemplate(this)
            {
                Title = MaintCultureTextResources.SiteSettings,
                Description = MaintCultureTextResources.SiteSettingsDescription,
                FormTitle = MaintCultureTextResources.SiteInfo,
                Fields = new[]
                {
                    new Field
                    {
                        Width = 6,
                        Label = MaintCultureTextResources.SiteSiteName,
                        FieldName = "SiteName",
                        Control = new TextBox
                        {
                            MaxLength = 12,
                            Value = SiteName,
                        }
                    }, 
                    new Field
                    {
                        Width = 6,
                        Label = MaintCultureTextResources.SiteDefaultCulture,
                        FieldName = "DefaultCulture",
                        Control = new SelectList(cultures,DefaultCulture.ToString() )
                    } , 
                    new Field
                    {
                        Width = 6,
                        Label = MaintCultureTextResources.SiteMaxSignInFailTimes,
                        FieldName = "MaxSignInFailTimes",
                        Control = new TextBox
                        {
                            MaxLength = 12,
                            Value = MaxSignInFailTimes.ToString(),
                        }
                    } 
                },
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