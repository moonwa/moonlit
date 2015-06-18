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


        [Display(ResourceType = typeof(CultureTextResources), Name = "CultureName")]
        [Required(ErrorMessageResourceName = "ValidationRequired", ErrorMessageResourceType = typeof(CultureTextResources))]
        public string Name { get; set; }


        [Display(ResourceType = typeof(CultureTextResources), Name = "CultureDisplayName")]
        [Required(ErrorMessageResourceName = "ValidationRequired", ErrorMessageResourceType = typeof(CultureTextResources))]
        public string DisplayName { get; set; }

        [Display(ResourceType = typeof(CultureTextResources), Name = "CultureIsEnabled")]
        public bool IsEnabled { get; set; }

        public Template CreateTemplate(RequestContext requestContext)
        {
            return new AdministrationSimpleEditTemplate(this)
            {
                Title = CultureTextResources.CultureCreate,
                Description = CultureTextResources.CultureCreateDescription,
                FormTitle = CultureTextResources.CultureInfo,
                Fields = new[]
                {
                    new Field
                    {
                        Width = 6,
                        Label = CultureTextResources.CultureName,
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
                        Label = CultureTextResources.CultureDisplayName,
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
                        Label = CultureTextResources.CultureIsEnabled,
                        FieldName = "IsEnabled",
                        Control = new CheckBox()
                        {
                            Checked = IsEnabled,
                            Value = true.ToString(),
                            Text=CultureTextResources.CultureIsEnabled,
                        }
                    }, 
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