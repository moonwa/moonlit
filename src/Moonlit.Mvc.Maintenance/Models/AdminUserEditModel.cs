using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Moonlit.Collections;
using Moonlit.Mvc.Controls;
using Moonlit.Mvc.Maintenance.Domains;
using Moonlit.Mvc.Maintenance.Properties;
using Moonlit.Mvc.Templates;

namespace Moonlit.Mvc.Maintenance.Models
{
    public partial class AdminUserEditModel : IEntityMapper<User>
    {
        partial void OnTemplate(AdministrationSimpleEditTemplate template, ControllerContext controllerContext)
        {
            template.Buttons = new IClickable[]
            {
                new Button(MaintCultureTextResources.Save, ""),
            };
        }
        public IMaintDbRepository MaintDbContext { get; set; }
        private int[] MappingRolesFromEntity(User entity, ControllerContext controllerContext)
        {
            return entity.Roles.Select(x => x.RoleId).ToArray();
        }

        private ICollection<Role> MappingRolesToEntity(User entity, ControllerContext controllerContext)
        {
            return this.Roles.IsNullOrEmpty() ? new List<Role>() : MaintDbContext.Roles.Where(x => x.IsEnabled && Roles.Contains(x.RoleId)).ToList();
        }

        private string MappingPasswordFromEntity(User entity, ControllerContext controllerContext)
        {
            return string.Empty;
        }

        private string MappingPasswordToEntity(User entity, ControllerContext controllerContext)
        {
            if (!string.IsNullOrEmpty(Password))
            {
                return entity.HashPassword(Password);
            }
            return entity.Password;
        }
    }
}