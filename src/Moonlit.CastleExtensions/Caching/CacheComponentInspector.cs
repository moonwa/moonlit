using System;
using System.Linq;
using System.Reflection;
using Castle.Core;
using Castle.MicroKernel;
using Castle.MicroKernel.ModelBuilder.Inspectors;
using Moonlit.Caching;
using Moonlit.CastleExtensions.Caching;

namespace Moonlit.CastleExtensions.Caching
{
    [Transient]
    public class CacheComponentInspector : MethodMetaInspector
    {
        private static readonly String TransactionNodeName = "mww.cache";

        public override void ProcessModel(IKernel kernel, Castle.Core.ComponentModel model)
        {
            var existCacheAttr = model.Implementation.GetMethods().Any(x => x.GetCustomAttribute<CacheAttribute>(true) != null);
            if (!existCacheAttr) return;

            model.Dependencies.Add(new DependencyModel(null, typeof(CacheInterceptor), false));
            model.Interceptors.AddFirst(new InterceptorReference(typeof(CacheInterceptor)));
        } 
        protected override string ObtainNodeName()
        {
            return TransactionNodeName;
        }
    }
}
