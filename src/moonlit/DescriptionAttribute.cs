using System;
using System.Reflection;
using Moonlit.Reflection;

namespace Moonlit
{
    /// <summary>
    /// 描述性性,指示该元素的描述
    /// </summary>
    [global::System.AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = false)]
    public sealed class DescriptionAttribute : Attribute
    {
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; private set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="DescriptionAttribute"/> class.
        /// </summary>
        /// <param name="description">描述.</param>
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