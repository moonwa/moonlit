using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Routing;
using Moonlit.Mvc.Controls;
using Moonlit.Mvc.Maintenance.Domains;
using Moonlit.Mvc.Maintenance.Properties;
using Moonlit.Mvc.Templates;

namespace Moonlit.Mvc.Maintenance.Models
{
    public class SiteSettingsModel
    {
        public void SetInnerObject(SiteModel siteModel)
        {
            SiteName = siteModel.SiteName;
            DefaultCulture = siteModel.DefaultCulture;
        }

        [Display(ResourceType = typeof(CultureTextResources), Name = "SiteSiteName")]
        [Required(ErrorMessageResourceName = "ValidationRequired", ErrorMessageResourceType = typeof(CultureTextResources))]
        public string SiteName { get; set; }

        [Display(ResourceType = typeof(CultureTextResources), Name = "SiteDefaultCulture")]
        [Required(ErrorMessageResourceName = "ValidationRequired", ErrorMessageResourceType = typeof(CultureTextResources))]
        public int? DefaultCulture { get; set; }

        public Template CreateTemplate(RequestContext requestContext, IMaintDbRepository db)
        {
            var cultures = db.Cultures.Where(x => x.IsEnabled).ToList().Select(x => new SelectListItem()
            {
                Text = x.DisplayName,
                Value = x.CultureId.ToString(),
                Selected = x.CultureId == this.DefaultCulture,
            }).ToList();
            cultures.Insert(0, new SelectListItem());
            return new AdministrationSimpleEditTemplate(this)
            {
                Title = CultureTextResources.SiteSettings,
                Description = CultureTextResources.SiteSettingsDescription,
                FormTitle = CultureTextResources.SiteInfo,
                Fields = new[]
                {
                    new Field
                    {
                        Width = 6,
                        Label = CultureTextResources.SiteSiteName,
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
                        Label = CultureTextResources.SiteDefaultCulture,
                        FieldName = "DefaultCulture",
                        Control = new SelectList
                        {
                            Items = cultures
                        }
                    } 
                },
                Buttons = new IClickable[]
                {
                    new Button
                    {
                        Text = CultureTextResources.Save,
                        ActionName = ""
                    }
                }
            };
        }
    }
}