using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Routing;
using Moonlit.Mvc.Controls;
using Moonlit.Mvc.Maintenance.Domains;
using Moonlit.Mvc.Maintenance.Properties;
using Moonlit.Mvc.Templates;

namespace Moonlit.Mvc.Maintenance.Models
{
    public class CultureTextImportModel
    {  
        [Display(ResourceType = typeof(CultureTextResources), Name = "Content")]
        [Required(ErrorMessageResourceName = "ValidationRequired", ErrorMessageResourceType = typeof(CultureTextResources))]
        public string Content { get; set; }
        [Display(ResourceType = typeof(CultureTextResources), Name = "Overwrite")]
        [Required(ErrorMessageResourceName = "ValidationRequired", ErrorMessageResourceType = typeof(CultureTextResources))]
        public bool Overwrite { get; set; }
        [Display(ResourceType = typeof(CultureTextResources), Name = "CultureTextCulture")]
        [Required(ErrorMessageResourceName = "ValidationRequired", ErrorMessageResourceType = typeof(CultureTextResources))]
        public int? Culture { get; set; }

        public Template CreateTemplate(RequestContext requestContext, IMaintDbRepository db)
        {
            List<SelectListItem> items = db.Cultures.Where(x => x.IsEnabled).ToList().Select(x => new SelectListItem()
            {
                Text = x.DisplayName,
                Value = x.CultureId.ToString(),
                Selected = x.CultureId == this.Culture,
            }).ToList();
            return new AdministrationSimpleEditTemplate(this)
            {
                Title = CultureTextResources.CultureTextImport,
                Description = CultureTextResources.CultureTextImportDescription,
                FormTitle = CultureTextResources.CultureTextInfo,
                Fields = new[]
                {
                    new Field
                    {
                        Width = 6,
                        Label = CultureTextResources.Content,
                        FieldName = "Content",
                        Control = new MultiLineTextBox
                        {
                            MaxLength = 12,
                            Value = Content
                        }
                    },
                    new Field
                    {
                        Width = 6,
                        Label = CultureTextResources.Overwrite,
                        FieldName = "Overwrite",
                        Control = new CheckBox()
                        {
                            Checked = Overwrite,
                            Value = true.ToString()
                        }
                    },
                    new Field
                    {
                        Width = 6,
                        Label = CultureTextResources.CultureTextCulture,
                        FieldName = "Culture",
                        Control = new SelectList()
                        {
                            Items = items,
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