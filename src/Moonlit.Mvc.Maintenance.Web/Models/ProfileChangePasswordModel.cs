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
    public class ProfileChangePasswordModel 
    {
        public void SetInnerObject(User user)
        {

        }
        [PasswordBox]
        [Field(FieldWidth.W4)]
        [Display(ResourceType = typeof(MaintCultureTextResources), Name = "ProfileChangePasswordOldPassword")]
        [Required(ErrorMessageResourceName = "ValidationRequired", ErrorMessageResourceType = typeof(MaintCultureTextResources))]
        public string OldPassword { get; set; }
        [PasswordBox]
        [Field(FieldWidth.W4)]
        [Display(ResourceType = typeof(MaintCultureTextResources), Name = "ProfileChangePasswordNewPassword")]
        [Required(ErrorMessageResourceName = "ValidationRequired", ErrorMessageResourceType = typeof(MaintCultureTextResources))]
        public string NewPassword { get; set; }
        [PasswordBox]
        [Field(FieldWidth.W4)]
        [Display(ResourceType = typeof(MaintCultureTextResources), Name = "ProfileChangePasswordConfirmPassword")]
        [System.ComponentModel.DataAnnotations.Compare("NewPassword", ErrorMessageResourceName = "ValidationCompare", ErrorMessageResourceType = typeof(MaintCultureTextResources))]
        public string ConfirmPassword { get; set; }


        public Template CreateTemplate(ControllerContext controllerContext)
        {
            return new AdministrationSimpleEditTemplate
            {
                Title = MaintCultureTextResources.ProfileChangePassword,
                Description = MaintCultureTextResources.ProfileChangePasswordDescription,
                FormTitle = MaintCultureTextResources.ProfileChangePasswordInfo,
                Fields = new FieldsBuilder().ForEntity(this, controllerContext).Build(),
                Buttons = new List<IClickable>
                {
                    new Button(MaintCultureTextResources.Save)
                }
            };
        }
   
    }
}