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
        [Display(ResourceType = typeof(MaintCultureTextResources), Name = "CultureTextText")]
        [Required(ErrorMessageResourceName = "ValidationRequired", ErrorMessageResourceType = typeof(MaintCultureTextResources))]
        public string Text { get; set; }

        [Display(ResourceType = typeof(MaintCultureTextResources), Name = "CultureTextName")]
        [Required(ErrorMessageResourceName = "ValidationRequired", ErrorMessageResourceType = typeof(MaintCultureTextResources))]
        public string Name { get; set; }
        [Display(ResourceType = typeof(MaintCultureTextResources), Name = "CultureTextCulture")]
        [Required(ErrorMessageResourceName = "ValidationRequired", ErrorMessageResourceType = typeof(MaintCultureTextResources))]
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
                Title = MaintCultureTextResources.CultureTextCreate,
                Description = MaintCultureTextResources.CultureTextCreateDescription,
                FormTitle = MaintCultureTextResources.CultureTextInfo,
                Fields = new[]
                {
                    new Field
                    {
                        Width = 6,
                        Label = MaintCultureTextResources.CultureTextName,
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
                        Label = MaintCultureTextResources.CultureTextText,
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
                        Label = MaintCultureTextResources.CultureTextCulture,
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
                        Text = MaintCultureTextResources.Save,
                        ActionName = ""
                    }
                }
            };
        }
    }
}