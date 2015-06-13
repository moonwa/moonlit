using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Moonlit.Mvc
{
    public static class DependencyResolverExtensions
    {
        public static T GetService<T>(this System.Web.Mvc.IDependencyResolver dependencyResolver, bool throwException)
        {
            if (throwException)
            {
                return dependencyResolver.GetService<T>();
            }
            try
            {
                return dependencyResolver.GetService<T>();
            }
            catch (Exception)
            {
                return default(T);
            }
        }
    }
}
