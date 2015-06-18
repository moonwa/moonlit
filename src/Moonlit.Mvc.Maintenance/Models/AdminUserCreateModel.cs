using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Routing;
using Moonlit.Mvc.Controls;
using Moonlit.Mvc.Maintenance.Domains;
using Moonlit.Mvc.Maintenance.Properties;
using Moonlit.Mvc.Templates;

namespace Moonlit.Mvc.Maintenance.Models
{
    public class AdminUserCreateModel
    {
        public AdminUserCreateModel()
        {
            IsEnabled = true;
        }

        [Display(ResourceType = typeof(CultureTextResources), Name = "AdminUserPassword")]
        [Required(ErrorMessageResourceName = "ValidationRequired", ErrorMessageResourceType = typeof(CultureTextResources))]
        public string Password { get; set; }

        [Display(ResourceType = typeof(CultureTextResources), Name = "AdminUserUserName")]
        [Required(ErrorMessageResourceName = "ValidationRequired", ErrorMessageResourceType = typeof(CultureTextResources))]
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

        public int[] RoleIds { get; set; }


        public Template CreateTemplate(RequestContext requestContext)
        {
            return new AdministrationSimpleEditTemplate(this)
            {
                Title = CultureTextResources.AdminUserCreate,
                Description = CultureTextResources.AdminUserCreateDescription,
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
                                    Selected = true
                                },
                                new SelectListItem
                                {
                                    Text = Domains.Gender.Female.ToDisplayString(),
                                    Value = ((int) Domains.Gender.Female).ToString(),
                                    Selected = false
                                }
                            }
                        }
                    },
                    new Field
                    {
                        Width = 6,
                        Label = CultureTextResources.AdminUserIsSuper,
                        FieldName = "IsSuper",
                        Control = new CheckBox()
                        {
                            Value = true.ToString(),
                            Checked = IsSuper,
                            Text=CultureTextResources.AdminUserIsSuper,
                        }
                    },
                    new Field
                    {
                        Width = 6,
                        Label = CultureTextResources.AdminUserIsEnabled,
                        FieldName = "IsEnabled",
                        Control = new CheckBox()
                        {
                            Value = true.ToString(),
                            Checked = IsEnabled,
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