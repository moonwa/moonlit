using System;
using System.Diagnostics;

namespace Moonlit
{
    /// <summary>
    /// 通用参数
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [DebuggerStepThrough]
    public class EventArgs<T> : EventArgs
    {
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public T Value { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventArgs&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public EventArgs(T value)
        {
            this.Value = value;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="EventArgs&lt;T&gt;"/> class.
        /// </summary>
        public EventArgs()
        {

        }
    }
}