using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Routing;
using Moonlit.Mvc.Controls;
using Moonlit.Mvc.Maintenance.Domains;
using Moonlit.Mvc.Maintenance.Properties;
using Moonlit.Mvc.Templates;
using SelectListItem = Moonlit.Mvc.Controls.SelectListItem;

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

            //            RoleIds = adminUser.Roles.Select(x => x.RoleId).ToArray();
        }


        [Display(ResourceType = typeof(CultureTextResources), Name = "AdminUserPassword")]
        public string Password { get; set; }

//        [Display(ResourceType = typeof(CultureTextResources), Name = "AdminUserUserName")]
//        [Required(ErrorMessageResourceName = "ValidationRequired", ErrorMessageResourceType = typeof(CultureTextResources))]
        public string UserName { get; set; }

        [Display(ResourceType = typeof(CultureTextResources), Name = "AdminUserLoginName")]
        [Required(ErrorMessageResourceName = "ValidationRequired", ErrorMessageResourceType = typeof(CultureTextResources))]
        public string LoginName { get; set; }

        [Display(ResourceType = typeof(CultureTextResources), Name = "AdminUserGender")]
        public Gender? Gender { get; set; }

        [Display(ResourceType = typeof(CultureTextResources), Name = "AdminUserDateOfBirth")]
        public DateTime? DateOfBirth { get; set; }

        [Display(ResourceType = typeof(CultureTextResources), Name = "AdminUserIsEnabled")]
        public bool IsEnabled { get; set; }
        [Display(ResourceType = typeof(CultureTextResources), Name = "AdminUserIsSuper")]
        public bool IsSuper { get; set; }
        public Template CreateTemplate(RequestContext requestContext)
        {
            return new AdministrationSimpleEditTemplate(this)
            {
                Title = CultureTextResources.AdminUserEdit,
                Description = CultureTextResources.AdminUserEditDescription,
                FormTitle = CultureTextResources.AdminUserInfo,
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
                            Value = LoginName
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
                            Value = UserName
                        }
                    },
                    new Field
                    {
                        Width = 6,
                        Label = CultureTextResources.AdminUserPassword,
                        FieldName = "Password",
                        Control = new PasswordBox
                        { 
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
                        Label = CultureTextResources.AdminUserIsEnabled,
                        FieldName = "IsEnabled",
                        Control = new CheckBox()
                        {
                            Checked = IsEnabled,
                            Value = true.ToString(),
                            Text=CultureTextResources.AdminUserIsEnabled,
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