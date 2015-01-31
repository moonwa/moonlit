using System;
using System.Collections.Generic;
using System.Text;

namespace Moonlit.Reflection
{
    /// <summary>
    /// ��ö�ٽ��и�������
    /// </summary>
    public static class EnumDescriptor
    {
        /// <summary>
        /// ��һ��Ԫ��ת��Ϊö������
        /// </summary>
        /// <typeparam name="T">ö������</typeparam>
        /// <param name="value">ö��ֵ</param>
        /// <returns></returns>
        public static T Parse<T>(string value)
        {
            return Parse<T>(value, true);
        }
        /// <summary>
        /// ��һ��Ԫ��ת��Ϊö������
        /// </summary>
        /// <typeparam name="T">ö������</typeparam>
        /// <param name="value">ö��ֵ</param>
        /// <param name="ignoreCase">�Ƿ���Դ�Сд</param>
        /// <returns></returns>
        public static T Parse<T>(string value, bool ignoreCase)
        {
            return (T)Enum.Parse(typeof(T), value, ignoreCase);
        }
    }
}
