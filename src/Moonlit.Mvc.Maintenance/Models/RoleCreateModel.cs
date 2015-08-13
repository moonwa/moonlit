using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using Moonlit.Mvc.Controls;
using Moonlit.Mvc.Maintenance.Properties;
using Moonlit.Mvc.Maintenance.SelectListItemsProviders;
using Moonlit.Mvc.Templates;
using MultiSelectList = Moonlit.Mvc.Controls.MultiSelectList;

namespace Moonlit.Mvc.Maintenance.Models
{
    public class RoleCreateModel
    {
        public RoleCreateModel()
        {
            IsEnabled = true;
            Privileges = new string[0];
        }

        [Field(FieldWidth.W6)]
        [TextBox]
        [Display(ResourceType = typeof(MaintCultureTextResources), Name = "RoleName")]
        [Required(ErrorMessageResourceName = "ValidationRequired", ErrorMessageResourceType = typeof(MaintCultureTextResources))]
        public string Name { get; set; }

        [Display(ResourceType = typeof(MaintCultureTextResources), Name = "RolePrivileges")]
        [MultiSelectList(typeof(PrivilegeSelectListItemsProvider))]
        [Required(ErrorMessageResourceName = "ValidationRequired", ErrorMessageResourceType = typeof(MaintCultureTextResources))]
        [Field(FieldWidth.W6)]
        public string[] Privileges { get; set; }

        [Display(ResourceType = typeof(MaintCultureTextResources), Name = "RoleIsEnabled")]
        [Field(FieldWidth.W6)]
        [CheckBox]
        public bool IsEnabled { get; set; }

        public Template CreateTemplate(ControllerContext controllerContext)
        {
            return new AdministrationSimpleEditTemplate
            {
                Title = MaintCultureTextResources.RoleCreate,
                Description = MaintCultureTextResources.RoleCreateDescription,
                FormTitle = MaintCultureTextResources.RoleInfo,
                Fields = new FieldsBuilder().ForEntity(this, controllerContext).Build(),
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