using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Moonlit.Mvc.Maintenance.Domains;

namespace Moonlit.Mvc.Maintenance.SelectListItemsProviders
{
    public class PrivilegeSelectListProvider : ISelectListProvider
    {
        public List<SelectListItem> GetSelectList(ModelMetadata modelMetadata, object model)
        {
            var privilegeLoader = DependencyResolver.Current.GetService<IPrivilegeLoader>();
            var allPrivileges = privilegeLoader.Load();

            return allPrivileges.Items.Select(x => new SelectListItem
            {
                Text = x.Text,
                Value = x.Name.ToLowerInvariant(),
            }).ToList();
        }
    }
    public class CultureSelectListProvider : ISelectListProvider
    {
        public List<SelectListItem> GetSelectList(ModelMetadata modelMetadata, object model)
        {
            var repository = DependencyResolver.Current.GetService<IMaintDbRepository>();
            return repository.Cultures.Where(x=>x.IsEnabled).ToList().Select(x => new SelectListItem
            {
                Text = x.DisplayName,
                Value = x.CultureId.ToString()
            }).ToList();
        }
    }
    public class RoleSelectListProvider : ISelectListProvider
    {
        public List<SelectListItem> GetSelectList(ModelMetadata modelMetadata, object model)
        {
            var repository = DependencyResolver.Current.GetService<IMaintDbRepository>();
            return repository.Roles.Where(x => x.IsEnabled).ToList().Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.RoleId.ToString()
            }).ToList();
        }
    }
}
