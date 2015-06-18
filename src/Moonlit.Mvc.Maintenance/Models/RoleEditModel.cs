using System;
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
    public class RoleEditModel
    {
        public void SetInnerObject(Role role)
        {
            Name = role.Name;
            IsEnabled = role.IsEnabled;
            Privileges = role.GetPrivileges();
        }


        [Display(ResourceType = typeof(CultureTextResources), Name = "RoleName")]
        [Required(ErrorMessageResourceName = "ValidationRequired", ErrorMessageResourceType = typeof(CultureTextResources))]
        public string Name { get; set; }

        [Display(ResourceType = typeof(CultureTextResources), Name = "RolePrivileges")]
        [Required(ErrorMessageResourceName = "ValidationRequired", ErrorMessageResourceType = typeof(CultureTextResources))]
        public string[] Privileges { get; set; }

        [Display(ResourceType = typeof(CultureTextResources), Name = "RoleIsEnabled")]
        public bool IsEnabled { get; set; }

        public Template CreateTemplate(RequestContext requestContext, IPrivilegeLoader privilegeLoader)
        {
            var allPrivileges = privilegeLoader.Load();
            Privileges = Privileges ?? new string[0];
            var selectListItems = allPrivileges.Items.Select(x => new SelectListItem
            {
                Text = x.Text,
                Value = x.Name,
                Selected = this.Privileges.Any(y => string.Equals(x.Name, y, StringComparison.OrdinalIgnoreCase)),
            }).ToArray();
            return new AdministrationSimpleEditTemplate(this)
            {
                Title = CultureTextResources.RoleEdit,
                Description = CultureTextResources.RoleEditDescription,
                FormTitle = CultureTextResources.RoleInfo,
                Fields = new[]
                {
                    new Field
                    {
                        Width = 6,
                        Label = CultureTextResources.RoleName,
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
                        Label = CultureTextResources.RolePrivileges,
                        FieldName = "Privileges",
                        Control = new MultiSelectList
                        {
                            Items =selectListItems
                        }
                    },
                    new Field
                    {
                        Width = 6,
                        Label = CultureTextResources.RoleIsEnabled,
                        FieldName = "IsEnabled",
                        Control = new CheckBox()
                        {
                            Checked = IsEnabled,
                            Value = true.ToString(),
                            Text=CultureTextResources.RoleIsEnabled,
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