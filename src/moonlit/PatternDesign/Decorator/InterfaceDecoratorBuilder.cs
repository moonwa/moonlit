using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Moonlit.PatternDesign
{
    /// <summary>
    /// 
    /// </summary>
    public class InterfaceDecoratorBuilder : DecoratorBuilder
    {
        public InterfaceDecoratorBuilder(Type targetType)
            : base(targetType)
        {
        }
        /// <summary>
        /// 获得类型的名称.
        /// </summary>
        /// <param name="targetType">Type of the target.</param>
        /// <returns></returns>
        protected override string GetTypeName(Type targetType)
        {
            return targetType.Name.Substring(1);
        }

        /// <summary>
        /// 获取必须要实现的成员.
        /// </summary>
        /// <param name="targetType">Type of the target.</param>
        /// <returns></returns>
        protected override List<MemberInfo> GetMustInheritMembers(Type targetType)
        {
            return new List<MemberInfo>(targetType.GetMethods(BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.Public));
        }
    }
}
