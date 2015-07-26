using System.ComponentModel.DataAnnotations;
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


        [Display(ResourceType = typeof(MaintCultureTextResources), Name = "CultureName")]
        [Required(ErrorMessageResourceName = "ValidationRequired", ErrorMessageResourceType = typeof(MaintCultureTextResources))]
        public string Name { get; set; }


        [Display(ResourceType = typeof(MaintCultureTextResources), Name = "CultureDisplayName")]
        [Required(ErrorMessageResourceName = "ValidationRequired", ErrorMessageResourceType = typeof(MaintCultureTextResources))]
        public string DisplayName { get; set; }

        [Display(ResourceType = typeof(MaintCultureTextResources), Name = "CultureIsEnabled")]
        public bool IsEnabled { get; set; }

        public Template CreateTemplate(RequestContext requestContext)
        {
            return new AdministrationSimpleEditTemplate(this)
            {
                Title = MaintCultureTextResources.CultureCreate,
                Description = MaintCultureTextResources.CultureCreateDescription,
                FormTitle = MaintCultureTextResources.CultureInfo,
                Fields = new[]
                {
                    new Field
                    {
                        Width = 6,
                        Label = MaintCultureTextResources.CultureName,
                        FieldName = "Name",
                        Control = new TextBox
                        {
                            MaxLength = 12,
                            Value = Name
                        }
                    },
                    new Field
                    {
                        Width = 6,
                        Label = MaintCultureTextResources.CultureDisplayName,
                        FieldName = "DisplayName",
                        Control = new TextBox
                        {
                            MaxLength = 12,
                            Value = DisplayName
                        }
                    },  
                    new Field
                    {
                        Width = 6,
                        Label = MaintCultureTextResources.CultureIsEnabled,
                        FieldName = "IsEnabled",
                        Control = new CheckBox()
                        {
                            Checked = IsEnabled,
                            Value = true.ToString(),
                            Text=MaintCultureTextResources.CultureIsEnabled,
                        }
                    }, 
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