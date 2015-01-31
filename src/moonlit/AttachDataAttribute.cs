using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Moonlit
{
    /// <summary>
    /// author: 老赵 
    /// from:   http://www.cnblogs.com/JeffreyZhao/archive/2009/01/07/AttachDataExtensions.html
    /// </summary>
    /// <example>
    /// 
    /// </example>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class AttachDataAttribute : Attribute
    {
        public AttachDataAttribute(object key, object value)
        {
            this.Key = key;
            this.Value = value;
        }

        public object Key { get; private set; }

        public object Value { get; private set; }
    }
    /// <summary>
    /// author: 老赵 
    /// from:   http://www.cnblogs.com/JeffreyZhao/archive/2009/01/07/AttachDataExtensions.html
    /// </summary>
    /// <example>
    /// 
    /// </example>
    public static class AttachDataExtensions
    {
        public static object GetAttachedData(
            this ICustomAttributeProvider provider, object key)
        {
            var attributes = (AttachDataAttribute[])provider.GetCustomAttributes(
                typeof(AttachDataAttribute), false);
            return attributes.First(a => a.Key.Equals(key)).Value;
        }

        public static T GetAttachedData<T>(
            this ICustomAttributeProvider provider, object key)
        {
            return (T)provider.GetAttachedData(key);
        }

        public static object GetAttachedData(this Enum value, object key)
        {
            return value.GetType().GetField(value.ToString()).GetAttachedData(key);
        }

        public static T GetAttachedData<T>(this Enum value, object key)
        {
            return (T)value.GetAttachedData(key);
        }
    }
}
