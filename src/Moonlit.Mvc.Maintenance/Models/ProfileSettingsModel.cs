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

        [Display(ResourceType = typeof(CultureTextResources), Name = "AdminUserCulture")]
        public int? Culture { get; set; }
        [Display(ResourceType = typeof(CultureTextResources), Name = "AdminUserLoginName")]
        public string LoginName { get; set; }

        [Display(ResourceType = typeof(CultureTextResources), Name = "AdminUserUserName")]
        [Required(ErrorMessageResourceName = "ValidationRequired", ErrorMessageResourceType = typeof(CultureTextResources))]
        public string UserName { get; set; }


        [Display(ResourceType = typeof(CultureTextResources), Name = "AdminUserGender")]
        public Gender? Gender { get; set; }

        [Display(ResourceType = typeof(CultureTextResources), Name = "AdminUserDateOfBirth")]
        public DateTime? DateOfBirth { get; set; }

        [Display(ResourceType = typeof(CultureTextResources), Name = "AdminUserIsSuper")]
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
                Title = CultureTextResources.ProfileSettings,
                Description = CultureTextResources.ProfileSettingsDescription,
                FormTitle = CultureTextResources.ProfileInfo,
                Fields = new[]
                {
                    new Field
                    {
                        Width = 6,
                        Label = CultureTextResources.AdminUserLoginName,
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
                        Label = CultureTextResources.AdminUserUserName,
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
                        Label = CultureTextResources.AdminUserGender,
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
                        Label = CultureTextResources.AdminUserDateOfBirth,
                        FieldName = "DateOfBirth",
                        Control = new DatePicker
                        {
                            Value = DateOfBirth
                        }
                    }, 
                    new Field
                    {
                        Width = 6,
                        Label = CultureTextResources.AdminUserCulture,
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
                        Text = CultureTextResources.Save,
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

        [Display(ResourceType = typeof(CultureTextResources), Name = "ProfileChangePasswordOldPassword")]
        [Required(ErrorMessageResourceName = "ValidationRequired", ErrorMessageResourceType = typeof(CultureTextResources))]
        public string OldPassword { get; set; }
        [Display(ResourceType = typeof(CultureTextResources), Name = "ProfileChangePasswordNewPassword")]
        [Required(ErrorMessageResourceName = "ValidationRequired", ErrorMessageResourceType = typeof(CultureTextResources))]
        public string NewPassword { get; set; }
        [Display(ResourceType = typeof(CultureTextResources), Name = "ProfileChangePasswordConfirmPassword")]
        [Compare("NewPassword", ErrorMessageResourceName = "ValidationCompare", ErrorMessageResourceType = typeof(CultureTextResources))]
        public string ConfirmPassword { get; set; }


        public Template CreateTemplate(RequestContext requestContext, IMaintDbRepository db)
        {

            return new AdministrationSimpleEditTemplate(this)
            {
                Title = CultureTextResources.ProfileChangePassword,
                Description = CultureTextResources.ProfileChangePasswordDescription,
                FormTitle = CultureTextResources.ProfileChangePasswordInfo,
                Fields = new[]
                {
                    new Field
                    {
                        Width = 6,
                        Label = CultureTextResources.ProfileChangePasswordOldPassword,
                        FieldName = "OldPassword",
                        Control = new PasswordBox()
                        { 
                        }
                    },
                    new Field
                    {
                        Width = 6,
                        Label = CultureTextResources.ProfileChangePasswordNewPassword,
                        FieldName = "NewPassword",
                        Control = new PasswordBox
                        { 
                        }
                    }, 
                    new Field
                    {
                        Width = 6,
                        Label = CultureTextResources.ProfileChangePasswordConfirmPassword,
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
                        Text = CultureTextResources.Save,
                        ActionName = ""
                    }
                }
            };
        }
    }
}