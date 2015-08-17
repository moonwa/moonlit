using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using Moonlit.Collections;
using Moonlit.Mvc.Controls;
using Moonlit.Mvc.Maintenance.Domains;
using Moonlit.Mvc.Maintenance.Properties;
using Moonlit.Mvc.Maintenance.SelectListItemsProviders;
using Moonlit.Mvc.Templates;
using SelectList = Moonlit.Mvc.Controls.SelectList;

namespace Moonlit.Mvc.Maintenance.Models
{
    public partial class AdminUserEditModel
    {
        partial void OnTemplate(AdministrationSimpleEditTemplate template, ControllerContext controllerContext)
        {
            template.Buttons = new IClickable[]
            {
                new Button(MaintCultureTextResources.Save, ""),
            };
        }
        public IMaintDbRepository MaintDbContext { get; set; }
        private int[] MappingRolesFromEntity(User entity)
        {
            return entity.Roles.Select(x => x.RoleId).ToArray();
        }

        private ICollection<Role> MappingRolesToEntity(User entity)
        {
            return this.Roles.IsNullOrEmpty() ? new List<Role>() : MaintDbContext.Roles.Where(x => x.IsEnabled && Roles.Contains(x.RoleId)).ToList();
        }

        private string MappingPasswordFromEntity(User entity)
        {
            return string.Empty;
        }

        private string MappingPasswordToEntity(User entity)
        {
            if (!string.IsNullOrEmpty(Password))
            {
                return entity.HashPassword(Password);
            }
            return entity.Password;
        }
    }
    public partial class AdminUserCreateModel
    {
        partial void OnTemplate(AdministrationSimpleEditTemplate template, ControllerContext controllerContext)
        {
            template.Buttons = new IClickable[]
            {
                new Button(MaintCultureTextResources.Save, ""),
            };
        }

        public IMaintDbRepository MaintDbContext { get; set; }


        private int[] MappingRolesFromEntity(User entity)
        {
            return entity.Roles.Select(x => x.RoleId).ToArray();
        }

        private ICollection<Role> MappingRolesToEntity(User entity)
        {
            return this.Roles.IsNullOrEmpty() ? new List<Role>() : MaintDbContext.Roles.Where(x => x.IsEnabled && Roles.Contains(x.RoleId)).ToList();
        }

        private string MappingPasswordFromEntity(User entity)
        {
            return string.Empty;
        }

        private string MappingPasswordToEntity(User entity)
        {
            if (!string.IsNullOrEmpty(Password))
            {
                return entity.HashPassword(Password);
            }
            return entity.Password;
        }
    }

    public interface IEntityMapper<T>
    {
        void ToEntity(T user);
    }
}