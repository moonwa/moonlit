using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Routing;
using Moonlit.Mvc.Controls;
using Moonlit.Mvc.Maintenance.Domains;
using Moonlit.Mvc.Maintenance.Properties;
using Moonlit.Mvc.Templates;

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

        [Display(ResourceType = typeof(MaintCultureTextResources), Name = "AdminUserCulture")]
        public int? Culture { get; set; }
        [Display(ResourceType = typeof(MaintCultureTextResources), Name = "AdminUserLoginName")]
        public string LoginName { get; set; }

        [Display(ResourceType = typeof(MaintCultureTextResources), Name = "AdminUserUserName")]
        [Required(ErrorMessageResourceName = "ValidationRequired", ErrorMessageResourceType = typeof(MaintCultureTextResources))]
        public string UserName { get; set; }


        [Display(ResourceType = typeof(MaintCultureTextResources), Name = "AdminUserGender")]
        public Gender? Gender { get; set; }

        [Display(ResourceType = typeof(MaintCultureTextResources), Name = "AdminUserDateOfBirth")]
        public DateTime? DateOfBirth { get; set; }

        [Display(ResourceType = typeof(MaintCultureTextResources), Name = "AdminUserIsSuper")]
        public bool IsSuper { get; set; }
        public Template CreateTemplate(RequestContext requestContext, IMaintDbRepository db)
        {
            var cultures = db.Cultures.Where(x => x.IsEnabled).ToList().Select(x => new SelectListItem()
            {
                Text = x.DisplayName,
                Value = x.CultureId.ToString(),
                Selected = x.CultureId == this.Culture,
            }).ToList();
            cultures.Insert(0, new SelectListItem());
            return new AdministrationSimpleEditTemplate(this)
            {
                Title = MaintCultureTextResources.ProfileSettings,
                Description = MaintCultureTextResources.ProfileSettingsDescription,
                FormTitle = MaintCultureTextResources.ProfileInfo,
                Fields = new[]
                {
                    new Field
                    {
                        Width = 6,
                        Label = MaintCultureTextResources.AdminUserLoginName,
                        FieldName = "LoginName",
                        Control = new TextBox
                        {
                            MaxLength = 12,
                            Value = LoginName,
                            Enabled = false,
                        }
                    },
                    new Field
                    {
                        Width = 6,
                        Label = MaintCultureTextResources.AdminUserUserName,
                        FieldName = "UserName",
                        Control = new TextBox
                        {
                            MaxLength = 12,
                            Value = UserName,
                        }
                    }, 
                    new Field
                    {
                        Width = 6,
                        Label = MaintCultureTextResources.AdminUserGender,
                        FieldName = "Gender",
                        Control = new SelectList
                        {
                            Items = new[]
                            {
                                new SelectListItem
                                {
                                    Text = Domains.Gender.Male.ToDisplayString(),
                                    Value = ((int) Domains.Gender.Male).ToString(),
                                    Selected = Gender == Domains.Gender.Male
                                },
                                new SelectListItem
                                {
                                    Text = Domains.Gender.Female.ToDisplayString(),
                                    Value = ((int) Domains.Gender.Female).ToString(),
                                    Selected = Gender == Domains.Gender.Female
                                }
                            }
                        }
                    }, 
                    new Field
                    {
                        Width = 6,
                        Label = MaintCultureTextResources.AdminUserDateOfBirth,
                        FieldName = "DateOfBirth",
                        Control = new DatePicker
                        {
                            Value = DateOfBirth
                        }
                    }, 
                    new Field
                    {
                        Width = 6,
                        Label = MaintCultureTextResources.AdminUserCulture,
                        FieldName = "Culture",
                        Control = new SelectList()
                        {
                            Items = cultures
                        }
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
    public class ProfileChangePasswordModel
    {
        public void SetInnerObject(User user)
        {

        }

        [Display(ResourceType = typeof(MaintCultureTextResources), Name = "ProfileChangePasswordOldPassword")]
        [Required(ErrorMessageResourceName = "ValidationRequired", ErrorMessageResourceType = typeof(MaintCultureTextResources))]
        public string OldPassword { get; set; }
        [Display(ResourceType = typeof(MaintCultureTextResources), Name = "ProfileChangePasswordNewPassword")]
        [Required(ErrorMessageResourceName = "ValidationRequired", ErrorMessageResourceType = typeof(MaintCultureTextResources))]
        public string NewPassword { get; set; }
        [Display(ResourceType = typeof(MaintCultureTextResources), Name = "ProfileChangePasswordConfirmPassword")]
        [Compare("NewPassword", ErrorMessageResourceName = "ValidationCompare", ErrorMessageResourceType = typeof(MaintCultureTextResources))]
        public string ConfirmPassword { get; set; }


        public Template CreateTemplate(RequestContext requestContext, IMaintDbRepository db)
        {

            return new AdministrationSimpleEditTemplate(this)
            {
                Title = MaintCultureTextResources.ProfileChangePassword,
                Description = MaintCultureTextResources.ProfileChangePasswordDescription,
                FormTitle = MaintCultureTextResources.ProfileChangePasswordInfo,
                Fields = new[]
                {
                    new Field
                    {
                        Width = 6,
                        Label = MaintCultureTextResources.ProfileChangePasswordOldPassword,
                        FieldName = "OldPassword",
                        Control = new PasswordBox()
                        { 
                        }
                    },
                    new Field
                    {
                        Width = 6,
                        Label = MaintCultureTextResources.ProfileChangePasswordNewPassword,
                        FieldName = "NewPassword",
                        Control = new PasswordBox
                        { 
                        }
                    }, 
                    new Field
                    {
                        Width = 6,
                        Label = MaintCultureTextResources.ProfileChangePasswordConfirmPassword,
                        FieldName = "ConfirmPassword",
                        Control = new PasswordBox
                        {
                             
                        }
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