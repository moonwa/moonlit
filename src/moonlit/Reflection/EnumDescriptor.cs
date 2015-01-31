using System;
using System.Collections.Generic;
using System.Text;

namespace Moonlit.Reflection
{
    /// <summary>
    /// 对枚举进行辅助操作
    /// </summary>
    public static class EnumDescriptor
    {
        /// <summary>
        /// 将一个元素转换为枚举类型
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="value">枚举值</param>
        /// <returns></returns>
        public static T Parse<T>(string value)
        {
            return Parse<T>(value, true);
        }
        /// <summary>
        /// 将一个元素转换为枚举类型
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="value">枚举值</param>
        /// <param name="ignoreCase">是否忽略大小写</param>
        /// <returns></returns>
        public static T Parse<T>(string value, bool ignoreCase)
        {
            return (T)Enum.Parse(typeof(T), value, ignoreCase);
        }
    }
}
