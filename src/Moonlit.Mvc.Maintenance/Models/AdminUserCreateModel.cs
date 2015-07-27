using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Web.Routing;
using Moonlit.Mvc.Controls;
using Moonlit.Mvc.Maintenance.Domains;
using Moonlit.Mvc.Maintenance.Properties;
using Moonlit.Mvc.Templates;
using SelectList = Moonlit.Mvc.Controls.SelectList;

namespace Moonlit.Mvc.Maintenance.Models
{
    public class AdminUserCreateModel
    {
        public AdminUserCreateModel()
        {
            IsEnabled = true;
        }

        [Display(ResourceType = typeof(MaintCultureTextResources), Name = "AdminUserPassword")]
        [Required(ErrorMessageResourceName = "ValidationRequired", ErrorMessageResourceType = typeof(MaintCultureTextResources))]
        public string Password { get; set; }

        [Display(ResourceType = typeof(MaintCultureTextResources), Name = "AdminUserUserName")]
        [Required(ErrorMessageResourceName = "ValidationRequired", ErrorMessageResourceType = typeof(MaintCultureTextResources))]
        public string UserName { get; set; }

        [Display(ResourceType = typeof(MaintCultureTextResources), Name = "AdminUserLoginName")]
        [Required(ErrorMessageResourceName = "ValidationRequired", ErrorMessageResourceType = typeof(MaintCultureTextResources))]
        public string LoginName { get; set; }

        [Display(ResourceType = typeof(MaintCultureTextResources), Name = "AdminUserGender")]
        public Gender? Gender { get; set; }

        [Display(ResourceType = typeof(MaintCultureTextResources), Name = "AdminUserDateOfBirth")]
        public DateTime? DateOfBirth { get; set; }

        [Display(ResourceType = typeof(MaintCultureTextResources), Name = "AdminUserIsEnabled")]
        public bool IsEnabled { get; set; }
        [Display(ResourceType = typeof(MaintCultureTextResources), Name = "AdminUserIsSuper")]
        public bool IsSuper { get; set; }

        public int[] RoleIds { get; set; }


        public Template CreateTemplate(RequestContext requestContext)
        {
            return new AdministrationSimpleEditTemplate(this)
            {
                Title = MaintCultureTextResources.AdminUserCreate,
                Description = MaintCultureTextResources.AdminUserCreateDescription,
                FormTitle = MaintCultureTextResources.AdminUserInfo,
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
                            Value = LoginName
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
                            Value = UserName
                        }
                    },
                    new Field
                    {
                        Width = 6,
                        Label = MaintCultureTextResources.AdminUserPassword,
                        FieldName = "Password",
                        Control = new PasswordBox
                        { 
                        }
                    },
                    new Field
                    {
                        Width = 6,
                        Label = MaintCultureTextResources.AdminUserGender,
                        FieldName = "Gender",
                        Control = new SelectList(new []
                        {
                            new SelectListItem
                                {
                                    Text = Domains.Gender.Male.ToDisplayString(),
                                    Value = ((int) Domains.Gender.Male).ToString(),
                                },
                                new SelectListItem
                                {
                                    Text = Domains.Gender.Female.ToDisplayString(),
                                    Value = ((int) Domains.Gender.Female).ToString(),
                                }
                        }, Gender .ToIntString())
                    },
                    new Field
                    {
                        Width = 6,
                        Label = MaintCultureTextResources.AdminUserIsSuper,
                        FieldName = "IsSuper",
                        Control = new CheckBox()
                        {
                            Value = true.ToString(),
                            Checked = IsSuper,
                            Text=MaintCultureTextResources.AdminUserIsSuper,
                        }
                    },
                    new Field
                    {
                        Width = 6,
                        Label = MaintCultureTextResources.AdminUserIsEnabled,
                        FieldName = "IsEnabled",
                        Control = new CheckBox()
                        {
                            Value = true.ToString(),
                            Checked = IsEnabled,
                            Text=MaintCultureTextResources.AdminUserIsEnabled,
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