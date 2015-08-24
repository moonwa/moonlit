using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using Moonlit.Collections;
using Moonlit.Mvc.Controls;
using Moonlit.Mvc.Maintenance.Controllers;
using Moonlit.Mvc.Maintenance.Domains;
using Moonlit.Mvc.Maintenance.Properties;
using Moonlit.Mvc.Maintenance.SelectListItemsProviders;
using Moonlit.Mvc.Templates;

namespace Moonlit.Mvc.Maintenance.Models
{
    public class AdminUserCreateModel : IFromEntity<User>, IToEntity<User>
    {
        [Display(
            ResourceType = typeof(MaintCultureTextResources),
            Name = "AdminUserUserName"
            )]
        [Field(FieldWidth.W4)]
        [Mapping]
        public string UserName { get; set; }

        [Display(
            ResourceType = typeof(MaintCultureTextResources),
            Name = "AdminUserLoginName"
            )]
        [Field(FieldWidth.W4)]
        [Mapping]
        public string LoginName { get; set; }

        [Display(
            ResourceType = typeof(MaintCultureTextResources),
            Name = "AdminUserPassword"
            )]
        [Field(FieldWidth.W4)]
        [PasswordBox]
        public string Password { get; set; }

        [Display(
            ResourceType = typeof(MaintCultureTextResources),
            Name = "AdminUserGender"
            )]
        [Field(FieldWidth.W4)]
        [Mapping]
        public Gender? Gender { get; set; }

        [Display(
            ResourceType = typeof(MaintCultureTextResources),
            Name = "AdminUserDateOfBirth"
            )]
        [Field(FieldWidth.W4)]
        [Mapping]
        public DateTime? DateOfBirth { get; set; }

        [Display(
            ResourceType = typeof(MaintCultureTextResources),
            Name = "AdminUserCulture"
            )]
        [Field(FieldWidth.W4)]
        [SelectList(typeof(CultureSelectListProvider))]
        [Mapping]
        public int CultureId { get; set; }

        [Display(
            ResourceType = typeof(MaintCultureTextResources),
            Name = "AdminUserIsSuper"
            )]
        [Field(FieldWidth.W4)]
        [Mapping(OnlyNotPostback = false)]
        [ReadOnly(true)]
        public bool IsSuper { get; set; }

        [Display(
            ResourceType = typeof(MaintCultureTextResources),
            Name = "AdminUserRoles"
            )]
        [Field(FieldWidth.W4)]
        [MultiSelectList(typeof(RoleSelectListProvider))]
        public int[] Roles { get; set; }

        [Display(
            ResourceType = typeof(MaintCultureTextResources),
            Name = "AdminUserIsEnabled"
            )]
        [Field(FieldWidth.W4)]
        [Mapping]
        public bool IsEnabled { get; set; }

        public Template CreateTemplate(ControllerContext controllerContext)
        {
            var template = new AdministrationSimpleEditTemplate
            {
                Title = MaintCultureTextResources.AdminUserCreate,
                Description = MaintCultureTextResources.AdminUserCreateDescription,
                FormTitle = MaintCultureTextResources.AdminUserInfo,
                Fields = new FieldsBuilder().ForEntity(this, controllerContext).Build(),
                Buttons = new IClickable[]
                {
                    new Button(MaintCultureTextResources.Save, "")
                },
            };
            return template;
        }

        public void OnFromEntity(User entity, FromEntityContext context)
        {
            if (!context.IsPostback)
            {
                this.Roles = entity.Roles.Select(x => x.RoleId).ToArray();
            }
        }

        public void OnToEntity(User entity, ToEntityContext context)
        {
            var database = ((MaintControllerBase) context.ControllerContext.Controller).MaintDbContext;
            entity.Roles = Roles.IsNullOrEmpty() ? new List<Role>() : database.Roles.Where(x => x.IsEnabled && Roles.Contains(x.RoleId)).ToList();

            if (!string.IsNullOrEmpty(Password))
            {
                entity.Password = entity.HashPassword(Password);
            }
        }
    }
}