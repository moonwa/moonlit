using System.Collections.Generic;
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
    public class CultureTextEditModel
    {
        public void SetInnerObject(CultureText cultureText)
        {
            Text = cultureText.Text;
            Name = cultureText.Name;
            Culture = cultureText.CultureId;
        }

        [Display(ResourceType = typeof(MaintCultureTextResources), Name = "CultureTextText")]
        [Required(ErrorMessageResourceName = "ValidationRequired", ErrorMessageResourceType = typeof(MaintCultureTextResources))]
        public string Text { get; set; }

        [Display(ResourceType = typeof(MaintCultureTextResources), Name = "CultureTextName")]
        public string Name { get; set; }
        [Display(ResourceType = typeof(MaintCultureTextResources), Name = "CultureTextCulture")]
        public int? Culture { get; set; }

        public Template CreateTemplate(RequestContext requestContext, IMaintDbRepository db)
        {
            List<SelectListItem> items = db.Cultures.Where(x => x.IsEnabled).ToList().Select(x => new SelectListItem()
            {
                Text = x.DisplayName,
                Value = x.CultureId.ToString(), 
            }).ToList();
            return new AdministrationSimpleEditTemplate(this)
            {
                Title = MaintCultureTextResources.CultureTextEdit,
                Description = MaintCultureTextResources.CultureTextEditDescription,
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
                            Value = Name,
                            Enabled = false,
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
                        Control = new SelectList(items, Culture.ToString())
                        {
                            Enabled = false,
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