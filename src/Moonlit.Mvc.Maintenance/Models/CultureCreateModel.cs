using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Web.Routing;
using Moonlit.Mvc.Controls;
using Moonlit.Mvc.Maintenance.Properties;
using Moonlit.Mvc.Templates;

namespace Moonlit.Mvc.Maintenance.Models
{
    public class CultureCreateModel 
    {
        public CultureCreateModel()
        {
            IsEnabled = true;
        }


        [TextBox]
        [Field(FieldWidth.W6)]
        [Display(ResourceType = typeof(MaintCultureTextResources), Name = "CultureName")]
        [Required(ErrorMessageResourceName = "ValidationRequired", ErrorMessageResourceType = typeof(MaintCultureTextResources))]
        public string Name { get; set; }

        [TextBox]
        [Field(FieldWidth.W6)]
        [Display(ResourceType = typeof(MaintCultureTextResources), Name = "CultureDisplayName")]
        [Required(ErrorMessageResourceName = "ValidationRequired", ErrorMessageResourceType = typeof(MaintCultureTextResources))]
        public string DisplayName { get; set; }

        [CheckBox]
        [Field(FieldWidth.W6)]
        [Display(ResourceType = typeof(MaintCultureTextResources), Name = "CultureIsEnabled")]
        public bool IsEnabled { get; set; }

        public Template CreateTemplate(ControllerContext controllerContext)
        {
            return new AdministrationSimpleEditTemplate
            {
                Title = MaintCultureTextResources.CultureCreate,
                Description = MaintCultureTextResources.CultureCreateDescription,
                FormTitle = MaintCultureTextResources.CultureInfo,
                Fields = TemplateHelper.MakeFields(this, controllerContext),
                Buttons =   new IClickable[]
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