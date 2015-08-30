using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Web.Routing;
using Moonlit.Mvc.Controls;
using Moonlit.Mvc.Maintenance.Domains;
using Moonlit.Mvc.Maintenance.Properties;
using Moonlit.Mvc.Templates;

namespace Moonlit.Mvc.Maintenance.Models
{
    public partial class CultureCreateModel : IEntityMapper<Culture>
    {
        public CultureCreateModel()
        {
            IsEnabled = true;
        }


        partial void OnTemplate(AdministrationSimpleEditTemplate template, ControllerContext controllerContext)
        {
            template.Buttons = new List<IClickable>
            {
                new Button(  MaintCultureTextResources.Save ),
            };
        }
    }
}