using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using Moonlit.Mvc.Controls;
using Moonlit.Mvc.Maintenance.Domains;
using Moonlit.Mvc.Maintenance.Properties;
using Moonlit.Mvc.Maintenance.SelectListItemsProviders;
using Moonlit.Mvc.Templates;
using MultiSelectList = Moonlit.Mvc.Controls.MultiSelectList;

namespace Moonlit.Mvc.Maintenance.Models
{
    public class RoleEditModel 
    {
        public void SetInnerObject(Role role)
        {
            Name = role.Name;
            IsEnabled = role.IsEnabled;
            Privileges = role.GetPrivileges();
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
                Title = MaintCultureTextResources.RoleEdit,
                Description = MaintCultureTextResources.RoleEditDescription,
                FormTitle = MaintCultureTextResources.RoleInfo,
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