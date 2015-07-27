using System;
using System.Collections.Generic;
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

            RoleIds = user.Roles == null ? new int[0] : user.Roles.Select(x => x.RoleId).ToArray();
        }

        public int[] RoleIds { get; set; }


        [Display(ResourceType = typeof(MaintCultureTextResources), Name = "AdminUserPassword")]
        public string Password { get; set; }

        //        [Display(ResourceType = typeof(MaintCultureTextResources), Name = "AdminUserUserName")]
        //        [Required(ErrorMessageResourceName = "ValidationRequired", ErrorMessageResourceType = typeof(MaintCultureTextResources))]
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
        public Template CreateTemplate(RequestContext requestContext)
        {
            var roles = new RolesSelectListProvider().GetItems();
            foreach (var role in roles)
            {
                role.Selected = RoleIds.Select(x => x.ToString()).Contains(role.Value);
            }
            return new AdministrationSimpleEditTemplate(this)
            {
                Title = MaintCultureTextResources.AdminUserEdit,
                Description = MaintCultureTextResources.AdminUserEditDescription,
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
                        Label = MaintCultureTextResources.AdminUserIsEnabled,
                        FieldName = "IsEnabled",
                        Control = new CheckBox()
                        {
                            Checked = IsEnabled,
                            Value = true.ToString(),
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
                    new Field
                    {
                        Width = 6,
                        Label = MaintCultureTextResources.AdminUserDateOfBirth,
                        FieldName = "RoleIds",
                        Control = new MultiSelectList()
                        {
                            Items = roles,
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