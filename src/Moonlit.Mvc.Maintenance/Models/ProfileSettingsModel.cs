using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using Moonlit.Mvc.Controls;
using Moonlit.Mvc.Maintenance.Domains;
using Moonlit.Mvc.Maintenance.Properties;
using Moonlit.Mvc.Maintenance.SelectListItemsProviders;
using Moonlit.Mvc.Templates;
using SelectList = Moonlit.Mvc.Controls.SelectList;

namespace Moonlit.Mvc.Maintenance.Models
{
    public class ProfileSettingsModel 
    {
        public void SetInnerObject(User user)
        {
            UserName = user.UserName;
            LoginName = user.LoginName;
            IsSuper = user.IsSuper;
            DateOfBirth = user.DateOfBirth;
            Gender = user.Gender;
            Culture = user.CultureId;
        }
        [Field(FieldWidth.W6)]
        [SelectList(typeof(CultureSelectListItemsProvider))]
        [Display(ResourceType = typeof(MaintCultureTextResources), Name = "AdminUserCulture")]
        public int? Culture { get; set; }
        [Display(ResourceType = typeof(MaintCultureTextResources), Name = "AdminUserLoginName")]
        [Field(FieldWidth.W6)]
        [TextBox]
        public string LoginName { get; set; }
        [Field(FieldWidth.W6)]
        [TextBox]
        [Display(ResourceType = typeof(MaintCultureTextResources), Name = "AdminUserUserName")]
        [Required(ErrorMessageResourceName = "ValidationRequired", ErrorMessageResourceType = typeof(MaintCultureTextResources))]
        public string UserName { get; set; }


        [Field(FieldWidth.W6)]
        [SelectList(typeof(EnumSelectListProvider))]
        [Display(ResourceType = typeof(MaintCultureTextResources), Name = "AdminUserGender")]
        public Gender? Gender { get; set; }

        [DatePicker]
        [Field(FieldWidth.W6)]
        [Display(ResourceType = typeof(MaintCultureTextResources), Name = "AdminUserDateOfBirth")]
        public DateTime? DateOfBirth { get; set; }

        [CheckBox]
        [Field(FieldWidth.W6)]
        [Display(ResourceType = typeof(MaintCultureTextResources), Name = "AdminUserIsSuper")]
        public bool IsSuper { get; set; }
        public Template CreateTemplate(ControllerContext controllerContext)
        {
            return new AdministrationSimpleEditTemplate
            {
                Title = MaintCultureTextResources.ProfileSettings,
                Description = MaintCultureTextResources.ProfileSettingsDescription,
                FormTitle = MaintCultureTextResources.ProfileInfo,
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