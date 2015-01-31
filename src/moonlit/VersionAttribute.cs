using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Moonlit
{
    /// <summary>
    /// 版本属性
    /// </summary>
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    public sealed class VersionAttribute : System.Attribute, IComparable<VersionAttribute>
    {
        /// <summary>
        /// 返回此实例的哈希代码。
        /// </summary>
        /// <returns>32 位有符号整数哈希代码。</returns>
        public override int GetHashCode()
        {
            return this.Version.GetHashCode();
        }
        /// <summary>
        /// 构造一个版本属性
        /// </summary>
        /// <param name="major">主要版本号</param>
        /// <param name="minor">次要版本号</param>
        /// <param name="build">内部版本号</param>
        /// <param name="revision">修订号</param>
        public VersionAttribute(int major, int minor, int build, int revision)
        {
            this.Version = new Version(major, minor, build, revision);
        }
        /// <summary>
        /// 构造一个版本属性
        /// </summary>
        /// <param name="major">主要版本号</param>
        /// <param name="minor">次要版本号</param>
        /// <param name="build">内部版本号</param>
        public VersionAttribute(int major, int minor, int build)
        {
            this.Version = new Version(major, minor, build);
        }
        /// <summary>
        /// 构造一个版本属性
        /// </summary>
        /// <param name="major">主要版本号</param>
        /// <param name="minor">次要版本号</param>
        public VersionAttribute(int major, int minor)
        {
            this.Version = new Version(major, minor);
        }
        /// <summary>
        /// 版本属性
        /// </summary>
        public Version Version { get; private set; }
        /// <summary>
        /// 获取一个类型的最高版本
        /// </summary>
        /// <param name="type">被检查的类型</param>
        /// <returns></returns>
        public static Version GetNewVersion(Type type)
        {
            object[] verAttrs = type.GetCustomAttributes(typeof(VersionAttribute), false);
            if (verAttrs.Length == 0)
            {
                return type.Assembly.GetName().Version;
            }
            VersionAttribute[] vers = new VersionAttribute[verAttrs.Length];
            Array.Copy(verAttrs, vers, vers.Length);
            VersionAttribute ver = vers.Max();
            return ver.Version;
        }
        /// <summary>
        /// 获取一个类型的最高版本
        /// </summary>
        /// <param name="type">被检查的类型</param>
        /// <returns></returns>
        public static VersionAttribute[] GetVersions(Type type)
        {
            object[] verAttrs = type.GetCustomAttributes(typeof(VersionAttribute), false);
            if (verAttrs.Length == 0)
            {
                Version v = type.Assembly.GetName().Version;
                return new VersionAttribute[] { new VersionAttribute(v.Major, v.Minor, v.Build, v.Revision) };
            }
            VersionAttribute[] vers = new VersionAttribute[verAttrs.Length];
            Array.Copy(verAttrs, vers, vers.Length);
            return vers;
        }
        /// <summary>
        /// 获取 / 设置 该版本功能
        /// </summary>
        /// <value>The feature.</value>
        public string Feature { get; set; }
        /// <summary>
        /// Implements the operator &gt;.
        /// </summary>
        /// <param name="v1">The v1.</param>
        /// <param name="v2">The v2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator >(VersionAttribute v1, VersionAttribute v2)
        {
            if (object.ReferenceEquals(v1, null) && object.ReferenceEquals(v2, null))
            {
                return false;
            }
            if (object.ReferenceEquals(v1, null))
            {
                return false;
            }
            if (object.ReferenceEquals(v2, null))
            {
                return true;
            }
            return v1.Version > v2.Version;
        }
        /// <summary>
        /// Implements the operator &lt;.
        /// </summary>
        /// <param name="v1">The v1.</param>
        /// <param name="v2">The v2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator <(VersionAttribute v1, VersionAttribute v2)
        {
            return !(v1.Version > v2.Version);
        }
        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="v1">The v1.</param>
        /// <param name="v2">The v2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(VersionAttribute v1, VersionAttribute v2)
        {
            if (object.ReferenceEquals(v1, null) && object.ReferenceEquals(v2, null))
            {
                return true;
            }
            if (object.ReferenceEquals(v1, null) || object.ReferenceEquals(v2, null))
            {
                return false;
            }
            return v1.Version == v2.Version;
        }
        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="v1">The v1.</param>
        /// <param name="v2">The v2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(VersionAttribute v1, VersionAttribute v2)
        {
            return !(v1 == v2);
        }
        /// <summary>
        /// 返回一个值，该值指示此实例是否与指定的对象相等。
        /// </summary>
        /// <param name="obj">要与此实例进行比较的 <see cref="T:System.Object"/> 或 null。</param>
        /// <returns>
        /// 如果 <paramref name="obj"/> 等于此实例的类型和值，则为 true；否则为 false。
        /// </returns>
        public override bool Equals(object obj)
        {
            if (!(obj is VersionAttribute))
            {
                return false;
            }
            return this.Version.Equals(((VersionAttribute)obj).Version);
        }

        #region IComparable<VersionAttribute> 成员

        /// <summary>
        /// 比较当前对象和同一类型的另一对象。
        /// </summary>
        /// <param name="other">与此对象进行比较的对象。</param>
        /// <returns>
        /// 一个 32 位有符号整数，指示要比较的对象的相对顺序。返回值的含义如下： 值 含义 小于零 此对象小于 <paramref name="other"/> 参数。零 此对象等于 <paramref name="other"/>。 大于零 此对象大于 <paramref name="other"/>。
        /// </returns>
        public int CompareTo(VersionAttribute other)
        {
            if (other == null)
            {
                return 1;
            }
            if (object.ReferenceEquals(this, other))
            {
                return 0;
            }
            return this.Version.CompareTo(other.Version);
        }

        #endregion
    }
}
