using System;
using System.Linq;
using System.Security.Principal;

namespace Moonlit.Mvc
{
    public class UserPrincipal : IUserPrincipal
    {
        public virtual bool IsInRole(string role)
        {
            return this.Privileges.Any(x => string.Equals(role, x, StringComparison.OrdinalIgnoreCase));
        }

        public UserPrincipal(string[] privileges, IIdentity identity)
        {
            Privileges = privileges;
            Identity = identity;
        }

        public IIdentity Identity { get; private set; }

        public string[] Privileges { get; set; }
    }
}