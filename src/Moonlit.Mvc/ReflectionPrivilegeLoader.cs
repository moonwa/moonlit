using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Compilation;

namespace Moonlit.Mvc
{
    public class ReflectionPrivilegeLoader : IPrivilegeLoader
    {
        static List<PrivilegeAttribute> _privileges = new List<PrivilegeAttribute>();
        static ReflectionPrivilegeLoader()
        {
            foreach (var referencedAssembly in BuildManager.GetReferencedAssemblies().Cast<Assembly>())
            {
                var privilegeAttrs = referencedAssembly.GetCustomAttributes<PrivilegeAttribute>();
                _privileges.AddRange(privilegeAttrs);
            }
        }
        public Privileges Load()
        {
            return new Privileges
            {
                Items = _privileges.Select(x => new Privilege()
                {
                    Descriptin = x.GetDescription(),
                    Name = x.Name,
                    Text = x.GetText(),
                    Group = x.GetGroup(),
                }).ToList()
            };
        }
    }
}