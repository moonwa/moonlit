using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Moonlit.Reflection
{
    public interface IProxy
    {

    }
    public class EmptyProxyFactory
    {
        private static AssemblyBuilder _assemblyBuilder;
        private static ModuleBuilder _moduleBuilder;
        Dictionary<Type, Type> _proxies = new Dictionary<Type, Type>();
        public EmptyProxyFactory()
        {
            var name = new AssemblyName("Moonlit.Proxy_" + Guid.NewGuid().ToString("N"));
            _assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(name, AssemblyBuilderAccess.Run);
            _moduleBuilder = _assemblyBuilder.DefineDynamicModule("default");
        }
        public static bool IsProxy(Type proxyType)
        {
            return typeof(IProxy).IsAssignableFrom(proxyType);
        }
        public Type CreateInterfaceProxy<T>()
        {
            return CreateInterfaceProxy(typeof(T));
        }
        public Type CreateInterfaceProxy(Type baseType)
        {
            if (baseType == null) throw new ArgumentNullException("baseType");
            if (baseType.IsClass && baseType.IsSealed)
            {
                throw new ArgumentNullException("the type " + baseType.FullName + " cannot be a sealed type.");
            }

            Type implType;
            if (!_proxies.TryGetValue(baseType, out implType))
            {
                lock (_proxies)
                {
                    if (!_proxies.TryGetValue(baseType, out implType))
                    {

                        implType = CreateEmptyProxy(baseType);
                        _proxies[baseType] = implType;
                    }
                }
            }
            return implType;
        }

        private static Type CreateEmptyProxy(Type baseType)
        {

            var type = _moduleBuilder.DefineType(baseType + "Impl_" + Guid.NewGuid().ToString("N"),
                                                 TypeAttributes.Class | TypeAttributes.Public,
                                                 baseType.IsInterface ? typeof(object) : baseType);
            if (baseType.IsInterface)
            {
                type.AddInterfaceImplementation(baseType);
            }
            type.AddInterfaceImplementation(typeof(IProxy));
            foreach (var methodInfo in baseType.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                if (baseType.IsInterface || baseType.IsAbstract)
                {
                    var method = type.DefineMethod(methodInfo.Name,
                                                   MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.Virtual | MethodAttributes.NewSlot,
                                                   methodInfo.ReturnType,
                                                   methodInfo.GetParameters().Select(x => x.ParameterType).ToArray()
                        );

                    method.InitLocals = true;

                    var il = method.GetILGenerator();
                    if (methodInfo.ReturnType != typeof(void))
                    {
                        il.DeclareLocal(method.ReturnType);
                        il.Emit(OpCodes.Ldloc_0);
                    }
                    il.Emit(OpCodes.Ret);
                }
            }
            return type.CreateType();
        }
    }
}
