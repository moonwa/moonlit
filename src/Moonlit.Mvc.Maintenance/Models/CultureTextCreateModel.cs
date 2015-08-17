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
    public partial class CultureTextCreateModel : IEntityMapper<CultureText>
    {
        partial void OnTemplate(AdministrationSimpleEditTemplate template, ControllerContext controllerContext)
        {
            template.Buttons = new IClickable[]
            {
                new Button
                {
                    Text = MaintCultureTextResources.Save,
                    ActionName = ""
                }
            };
        }
    }
}