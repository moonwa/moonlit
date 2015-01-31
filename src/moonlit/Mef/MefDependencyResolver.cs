using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Moonlit.Mef
{
    public class MefDependencyResolver : IDependencyResolver
    {
        private ExportProvider _container;
        Dictionary<Type, Func<string, ExportProvider, object>> proxyList = new Dictionary<Type, Func<string, ExportProvider, object>>();
        Dictionary<Type, Func<ExportProvider, object>> defaultProxyList = new Dictionary<Type, Func<ExportProvider, object>>();
        Dictionary<Type, Func<ExportProvider, IEnumerable<object>>> allProxyList = new Dictionary<Type, Func<ExportProvider, IEnumerable<object>>>();

        public MefDependencyResolver(ExportProvider container)
        {
            _container = container;
        }
          
        public object Resolve(Type serviceType)
        {
            Func<ExportProvider, object> func;
            if (!defaultProxyList.TryGetValue(serviceType, out func))
            {
                var methodGetExportedValues = typeof(ExportProvider).GetMethod("GetExportedValueOrDefault", Type.EmptyTypes);
                var pContainer = Expression.Parameter(typeof(ExportProvider), "container");
                MethodCallExpression callGetExportedValueOrDefault = Expression.Call(pContainer, methodGetExportedValues.MakeGenericMethod(serviceType));
                LambdaExpression expression = Expression.Lambda(callGetExportedValueOrDefault, pContainer);
                func = (Func<ExportProvider, object>)expression.Compile();
                defaultProxyList[serviceType] = func;

            }
            return func(_container);
        }

        public object Resolve(Type serviceType, string key)
        {
            Func<string, ExportProvider, object> func;
            if (!proxyList.TryGetValue(serviceType, out func))
            {
                var methodGetExportedValues = typeof(ExportProvider).GetMethod("GetExportedValueOrDefault", new[] { typeof(string) });
                var pContainer = Expression.Parameter(typeof(ExportProvider), "container");
                var pKey = Expression.Parameter(typeof(string), "key");
                MethodCallExpression callGetExportedValueOrDefault = Expression.Call(pContainer, methodGetExportedValues.MakeGenericMethod(serviceType), pKey);
                LambdaExpression expression = Expression.Lambda(callGetExportedValueOrDefault, pKey, pContainer);
                func = (Func<string, ExportProvider, object>)expression.Compile();
                proxyList[serviceType] = func;
            }
            return func(key, _container);
        }

        public IEnumerable<object> ResolveAll(Type serviceType)
        {
            Func<ExportProvider, IEnumerable<object>> func;
            if (!allProxyList.TryGetValue(serviceType, out func))
            {
                var methodGetExportedValues = typeof(ExportProvider).GetMethod("GetExportedValues", Type.EmptyTypes);
                var pContainer = Expression.Parameter(typeof(ExportProvider), "container");
                MethodCallExpression callGetExportedValueOrDefault = Expression.Call(pContainer, methodGetExportedValues.MakeGenericMethod(serviceType));
                LambdaExpression expression = Expression.Lambda(callGetExportedValueOrDefault, pContainer);
                func = (Func<ExportProvider, IEnumerable<object>>)expression.Compile();
                allProxyList[serviceType] = func;
            }
            return func(_container);
        }

        public void Release(object service)
        {
        }
    }
}
