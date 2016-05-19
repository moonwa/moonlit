using System.Collections.Generic;
using System.Linq;
using Moonlit.Mvc.Maintenance.Domains;

namespace Moonlit.Mvc.Maintenance
{
    public class UserLoader : IUserLoader
    {
        private readonly IPrivilegeLoader _privilegeLoader;
        private readonly MaintDbContext _maintDbRepository;

        public UserLoader(IPrivilegeLoader privilegeLoader, MaintDbContext maintDbRepository)
        {
            _privilegeLoader = privilegeLoader;
            _maintDbRepository = maintDbRepository;
        }

        public IUserPrincipal GetUserPrincipal(string userName)
        {
            var db = _maintDbRepository;
            var adminUser = db.Users.FirstOrDefault(x => x.LoginName == userName);
            if (adminUser == null || !adminUser.IsEnabled)
            {
                return null;
            }

            var privileges = GetPrivileges(adminUser);
            return new UserPrincipal(privileges.ToArray(), adminUser);
        }

        private IEnumerable<string> GetPrivileges(User user)
        {
            if (user.IsSuper)
            {
                return _privilegeLoader.Load().Items.Select(x => x.Name);
            }
            return user.Roles.ToList().SelectMany(x => x.PrivilegeArray );
        }

    }


}
