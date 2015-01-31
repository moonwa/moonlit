using System;
using System.Reflection;
using Moonlit.Reflection;

namespace Moonlit
{
    /// <summary>
    /// ��������,ָʾ��Ԫ�ص�����
    /// </summary>
    [global::System.AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = false)]
    public sealed class DescriptionAttribute : Attribute
    {
        /// <summary>
        /// ����
        /// </summary>
        public string Description { get; private set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="DescriptionAttribute"/> class.
        /// </summary>
        /// <param name="description">����.</param>
        public DescriptionAttribute(string description)
        {
            this.Description = description;
        }

        public string GetDescription()
        {
            return Description;
        }
    }
}