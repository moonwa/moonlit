using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moonlit.Extensions
{
    public static class DomainExtensions
    {
        public static IEnumerable<Type> FindTypes(this AppDomain domain, Func<Type, bool> predicate)
        {
            foreach (var assembly in domain.GetAssemblies().Where(x => !x.IsDynamic))
            {
                foreach (var exportedType in assembly.GetExportedTypes().Where(predicate))
                {
                    yield return exportedType;
                }
            }
        }
    }
}
