using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Routing;
using Moonlit.Mvc.Controls;
using Moonlit.Mvc.Maintenance.Domains;
using Moonlit.Mvc.Maintenance.Properties;
using Moonlit.Mvc.Templates;
using SelectListItem = Moonlit.Mvc.Controls.SelectListItem;

namespace Moonlit.Mvc.Maintenance.Models
{
    public class CultureTextEditModel
    {
        public void SetInnerObject(CultureText cultureText)
        {
            Text = cultureText.Text;
            Name = cultureText.Name;
            Culture = cultureText.CultureId;
        }

        [Display(ResourceType = typeof(CultureTextResources), Name = "CultureTextText")]
        [Required(ErrorMessageResourceName = "ValidationRequired", ErrorMessageResourceType = typeof(CultureTextResources))]
        public string Text { get; set; }

        [Display(ResourceType = typeof(CultureTextResources), Name = "CultureTextName")]
        public string Name { get; set; }
        [Display(ResourceType = typeof(CultureTextResources), Name = "CultureTextCulture")]
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
                Title = CultureTextResources.CultureTextEdit,
                Description = CultureTextResources.CultureTextEditDescription,
                FormTitle = CultureTextResources.CultureTextInfo,
                Fields = new[]
                {
                    new Field
                    {
                        Width = 6,
                        Label = CultureTextResources.CultureTextName,
                        FieldName = "Name",
                        Control = new TextBox
                        {
                            MaxLength = 12,
                            Value = Name,
                            Enabled = false,
                        }
                    },
                    new Field
                    {
                        Width = 6,
                        Label = CultureTextResources.CultureTextText,
                        FieldName = "Text",
                        Control = new TextBox
                        {
                            MaxLength = 12,
                            Value = Text
                        }
                    },
                    new Field
                    {
                        Width = 6,
                        Label = CultureTextResources.CultureTextCulture,
                        FieldName = "Culture",
                        Control = new SelectList()
                        {
                            Enabled = false,
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