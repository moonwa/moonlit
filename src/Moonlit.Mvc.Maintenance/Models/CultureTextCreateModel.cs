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
    public class CultureTextCreateModel
    {
        [Display(ResourceType = typeof(CultureTextResources), Name = "CultureTextText")]
        [Required(ErrorMessageResourceName = "ValidationRequired", ErrorMessageResourceType = typeof(CultureTextResources))]
        public string Text { get; set; }

        [Display(ResourceType = typeof(CultureTextResources), Name = "CultureTextName")]
        [Required(ErrorMessageResourceName = "ValidationRequired", ErrorMessageResourceType = typeof(CultureTextResources))]
        public string Name { get; set; }
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
                Title = CultureTextResources.CultureTextCreate,
                Description = CultureTextResources.CultureTextCreateDescription,
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
                            Value = Name
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