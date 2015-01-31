using System;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using System.Linq;

namespace Moonlit
{
    /// <summary>
    /// 类型辅助描述
    /// </summary>
    public static class TypeExtension
    {
        #region IsInstance

        /// <summary>
        /// 测试一个类是否实现某个接口
        /// </summary>
        /// <param name="t">被判断的类型</param>
        /// <param name="interfaceType">被判断的接口</param>
        /// <returns></returns>
        public static bool IsInterface(this Type t, Type interfaceType)
        {
            Type[] faces = t.GetInterfaces();
            foreach (Type face in faces)
            {
                if (face == interfaceType)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 测试一个类是否实现某个接口
        /// </summary>
        /// <param name="t">被判断的类型</param>
        /// <returns></returns>
        public static bool IsInterface<T>(this Type t)
        {
            return IsInterface(t, typeof(T));
        }

        #endregion
        public static bool IsAnonymousType(this Type thisType)
        {
            return thisType.Name.Contains("AnonymousType");
        }
        public static Type ToWithoutNullableType(this Type thisType)
        {
            if (thisType.IsGenericType)
            {
                if (typeof(Nullable<>).IsAssignableFrom(thisType.GetGenericTypeDefinition()))
                {
                    return thisType.GetGenericArguments()[0];
                }
            }
            return thisType;
        }

        public static object GetDefaultValue(this Type thisType)
        {
            if (thisType.IsValueType)
                return Activator.CreateInstance(thisType);
            return null;
        }
        public static Type ExtractGenericInterfaceArgument(this Type thisType, Type interfaceType, int generatedTypeIndex)
        {
            Type type = thisType.ExtractGenericInterface(interfaceType);
            if (type == null) return null;

            return type.GetGenericArguments()[generatedTypeIndex];
        }
        public static IEnumerable<Type> GetBaseTypes(this Type thisType)
        {
            if (thisType == null) throw new ArgumentNullException("thisType");
            thisType = thisType.BaseType;
            while (thisType != null)
            {
                yield return thisType;
            }
        }
        public static Type ExtractGenericInterface(this Type thisType, Type interfaceType)
        {
            if (thisType == null) throw new ArgumentNullException("thisType");
            var interfaces = thisType.GetInterfaces().AsEnumerable();
            if (thisType.IsInterface)
                interfaces = interfaces.Union(new[] { thisType });
            foreach (var @interface in interfaces)
            {
                if (@interface.IsGenericType)
                {
                    if (@interface.GetGenericTypeDefinition() == interfaceType)
                    {
                        return @interface;
                    }
                }
                else
                {
                    if (@interface == interfaceType)
                    {
                        return @interface;
                    }
                }
            }

            return null;
        }

        public static string GetCSharpName(this Type thisType)
        {
            if (!thisType.IsGenericType) return thisType.FullName;

            var name = thisType.GetGenericTypeDefinition().FullName;
            name = name.Substring(0, name.IndexOf("`")) + "<";
            bool isFirst = true;
            foreach (var genericArgument in thisType.GetGenericArguments())
            {
                if (isFirst)
                {
                    isFirst = false;
                }
                else
                {
                    name += ",";
                }
                name += GetCSharpName(genericArgument);
            }
            return name + ">";
        }
        public static bool IsInheritFrom(this Type thisType, Type otherType)
        {
            if (thisType.IsGenericType)
            {
                thisType = thisType.GetGenericTypeDefinition();
            }
            while (thisType != typeof(object))
            {
                if (thisType == otherType)
                {
                    return true;
                }
                thisType = thisType.BaseType;
            }
            return false;
        }

        #region ForEach

        /// <summary>
        /// 遍历当前目录下所有类型
        /// </summary>
        /// <param name="action"></param>
        public static void ForEachType(Action<Type> action)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            string[] filenames = Directory.GetFiles(path, "*.dll");
            foreach (string filename in filenames)
            {
                AssemblyName assemblyName = AssemblyName.GetAssemblyName(filename);
                Assembly assembly = Assembly.Load(assemblyName);

                Type[] types = assembly.GetTypes();
                foreach (Type type in types)
                {
                    action(type);
                }
            }
        }

        /// <summary>
        /// 遍历当前目录下所有派生自T接口的类型
        /// </summary>
        /// <param name="action"></param>
        public static void ForEachTypeIsInterface<T>(Action<Type> action)
        {
            ForEachType(
                (t) =>
                {
                    if (IsInterface<T>(t))
                        action(t);
                });
        }

        #endregion


    }
}