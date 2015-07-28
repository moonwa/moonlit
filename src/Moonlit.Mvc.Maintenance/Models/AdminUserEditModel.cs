using System;
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
using MultiSelectList = Moonlit.Mvc.Controls.MultiSelectList;
using SelectList = Moonlit.Mvc.Controls.SelectList;

namespace Moonlit.Mvc.Maintenance.Models
{
    public class AdminUserEditModel 
    {
        public void SetInnerObject(User user)
        {
            UserName = user.UserName;
            LoginName = user.LoginName;
            IsSuper = user.IsSuper;
            IsEnabled = user.IsEnabled;
            DateOfBirth = user.DateOfBirth;
            Gender = user.Gender;

            Roles = user.Roles == null ? new int[0] : user.Roles.Select(x => x.RoleId).ToArray();
        }
        [TextBox()]
        [StringLength(12)]
        [Field(FieldWidth.W6)]
        [Display(ResourceType = typeof(MaintCultureTextResources), Name = "AdminUserUserName")]
        [Required(ErrorMessageResourceName = "ValidationRequired", ErrorMessageResourceType = typeof(MaintCultureTextResources))]
        public string UserName { get; set; }

        [Field(FieldWidth.W6)]
        [TextBox]
        [Display(ResourceType = typeof(MaintCultureTextResources), Name = "AdminUserLoginName")]
        [Required(ErrorMessageResourceName = "ValidationRequired", ErrorMessageResourceType = typeof(MaintCultureTextResources))]
        [StringLength(12)]
        public string LoginName { get; set; }
        [Field(FieldWidth.W6)]
        [Display(ResourceType = typeof(MaintCultureTextResources), Name = "AdminUserPassword")]
        public string Password { get; set; }

        [Display(ResourceType = typeof(MaintCultureTextResources), Name = "AdminUserGender")]
        [SelectList(typeof(EnumSelectListItemsProvider))]
        [Field(FieldWidth.W6)]
        public Gender? Gender { get; set; }

        [Display(ResourceType = typeof(MaintCultureTextResources), Name = "AdminUserDateOfBirth")]
        [DatePicker]
        [Field(FieldWidth.W6)]
        public DateTime? DateOfBirth { get; set; }

        [Display(ResourceType = typeof(MaintCultureTextResources), Name = "AdminUserIsEnabled")]
        [CheckBox]
        [Field(FieldWidth.W6)]
        public bool IsEnabled { get; set; }
        [Display(ResourceType = typeof(MaintCultureTextResources), Name = "AdminUserIsSuper")]
        [CheckBox]
        [Field(FieldWidth.W6)]
        public bool IsSuper { get; private set; }
        [Display(ResourceType = typeof(MaintCultureTextResources), Name = "AdminUserRoles")]
        [MultiSelectList(typeof(RolesSelectListItemsProvider))]
        [Field(FieldWidth.W6)]
        public int[] Roles { get; set; }
        public Template CreateTemplate(ControllerContext controllerContext)
        {
            return new AdministrationSimpleEditTemplate
            {
                Title = MaintCultureTextResources.AdminUserEdit,
                Description = MaintCultureTextResources.AdminUserEditDescription,
                FormTitle = MaintCultureTextResources.AdminUserInfo,
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