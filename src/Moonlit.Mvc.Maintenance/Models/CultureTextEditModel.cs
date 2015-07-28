using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using Moonlit.Mvc.Controls;
using Moonlit.Mvc.Maintenance.Domains;
using Moonlit.Mvc.Maintenance.Properties;
using Moonlit.Mvc.Maintenance.SelectListItemsProviders;
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


        [Field(FieldWidth.W6)]
        [TextBox]
        [Display(ResourceType = typeof(MaintCultureTextResources), Name = "CultureTextName")]
        public string Name { get; set; }

        [Field(FieldWidth.W6)]
        [SelectList(typeof(CultureSelectListItemsProvider))]
        [Display(ResourceType = typeof(MaintCultureTextResources), Name = "CultureTextCulture")]
        public int? Culture { get; set; }

        [Field(FieldWidth.W12)]
        [MultiLineTextBox]
        [Display(ResourceType = typeof(MaintCultureTextResources), Name = "CultureTextText")]
        [Required(ErrorMessageResourceName = "ValidationRequired", ErrorMessageResourceType = typeof(MaintCultureTextResources))]
        public string Text { get; set; }

        public Template CreateTemplate(ControllerContext controllerContext)
        {
            return new AdministrationSimpleEditTemplate
            {
                Title = MaintCultureTextResources.CultureTextEdit,
                Description = MaintCultureTextResources.CultureTextEditDescription,
                FormTitle = MaintCultureTextResources.CultureTextInfo,
                Fields = TemplateHelper.MakeFields(this, controllerContext),
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