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
    public class CultureTextImportModel
    {  
        [Display(ResourceType = typeof(MaintCultureTextResources), Name = "Content")]
        [Required(ErrorMessageResourceName = "ValidationRequired", ErrorMessageResourceType = typeof(MaintCultureTextResources))]
        public string Content { get; set; }
        [Display(ResourceType = typeof(MaintCultureTextResources), Name = "Overwrite")]
        [Required(ErrorMessageResourceName = "ValidationRequired", ErrorMessageResourceType = typeof(MaintCultureTextResources))]
        public bool Overwrite { get; set; }
        [Display(ResourceType = typeof(MaintCultureTextResources), Name = "CultureTextCulture")]
        [Required(ErrorMessageResourceName = "ValidationRequired", ErrorMessageResourceType = typeof(MaintCultureTextResources))]
        public int? Culture { get; set; }

        public Template CreateTemplate(RequestContext requestContext, IMaintDbRepository db)
        {
            List<SelectListItem> cultures = db.Cultures.Where(x => x.IsEnabled).ToList().Select(x => new SelectListItem()
            {
                Text = x.DisplayName,
                Value = x.CultureId.ToString(),
            }).ToList();
            return new AdministrationSimpleEditTemplate(this)
            {
                Title = MaintCultureTextResources.CultureTextImport,
                Description = MaintCultureTextResources.CultureTextImportDescription,
                FormTitle = MaintCultureTextResources.CultureTextInfo,
                Fields = new[]
                {
                    new Field
                    {
                        Width = 6,
                        Label = MaintCultureTextResources.Content,
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
                        Label = MaintCultureTextResources.Overwrite,
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
                        Label = MaintCultureTextResources.CultureTextCulture,
                        FieldName = "Culture",
                        Control = new SelectList(cultures, Culture.ToString()) ,
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