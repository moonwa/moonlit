using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Reflection.Emit;
using Moonlit.Diagnostics;

namespace Moonlit.Reflection.Emit
{
    public class GeneratedObject
    {
        private static Dictionary<Type, GeneratedObject> _type2Generated = new Dictionary<Type, GeneratedObject>();
        public static GeneratedObject CreateGeneratedObject<T>()
        {
            Type type = typeof(T);
            return CreateGeneratedObject(type);

        }

        public static GeneratedObject CreateGeneratedObject(Type type)
        {
            if (!_type2Generated.ContainsKey(type))
            {
                _type2Generated.Add(type, new GeneratedObject(type));
            }
            return _type2Generated[type];
        }

        private GeneratedObject(Type innerType)
        {
            InnerType = innerType;
        }

        private Type InnerType { get; set; }
        private Func<object> DefaultConstructor;
        /// <summary>
        /// 创建对象
        /// </summary>
        /// <returns></returns>
        public object Create()
        {
            if (DefaultConstructor == null)
            {
                var constructor = InnerType.GetConstructor(new Type[0]);
                if (constructor == null)
                {
                    throw new ValidationException(string.Format("类型 {0} 没有指定默认的构造函数", InnerType.AssemblyQualifiedName));
                }

                DynamicMethod methodBuilder = new DynamicMethod(InnerType.FullName + "_DefaultConstructor",
                    MethodAttributes.Public | MethodAttributes.Static, CallingConventions.Standard,
                    InnerType,
                    new Type[0],
                    typeof(GeneratedObject).Module, true);
                var ilGenerator = methodBuilder.GetILGenerator();
                LocalBuilder localBuilder = ilGenerator.DeclareLocal(InnerType);
                ilGenerator.Emit(OpCodes.Newobj, constructor);
                ilGenerator.Emit(OpCodes.Nop);
                ilGenerator.Emit(OpCodes.Stloc_0);
                ilGenerator.Emit(OpCodes.Ldloc_0);
                ilGenerator.Emit(OpCodes.Ret, localBuilder);
                DefaultConstructor = (Func<object>)methodBuilder.CreateDelegate(typeof(Func<object>));
                //typeBuilder.CreateType();
                //builder.Save("a.dll");
            }
            return DefaultConstructor();
        }
    }
}
