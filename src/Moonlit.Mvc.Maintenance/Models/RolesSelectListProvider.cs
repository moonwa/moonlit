using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Moonlit.Mvc.Maintenance.Domains;
using SelectListItem = Moonlit.Mvc.Controls.SelectListItem;

namespace Moonlit.Mvc.Maintenance.Models
{
    public class RolesSelectListProvider
    {
        public List<SelectListItem> GetItems()
        {
            var repository = DependencyResolver.Current.GetService<IMaintDbRepository>();
            return repository.Roles.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.RoleId.ToString()
            }).ToList();
        }
    }
}
