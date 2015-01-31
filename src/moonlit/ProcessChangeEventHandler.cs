using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moonlit
{
    /// <summary>
    /// 回调
    /// </summary>
    public delegate void ProcessChangedEventHandler (ProcessChangedEventArgs e);
    /// <summary>
    /// 进度改变参数
    /// </summary>
    public class ProcessChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the total.
        /// </summary>
        /// <value>The total.</value>
        public double Total { get; private set; }
        /// <summary>
        /// Gets or sets the current.
        /// </summary>
        /// <value>The current.</value>
        public double Current { get; private set; }
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ProcessChangedEventArgs"/> is cancel.
        /// </summary>
        /// <value><c>true</c> if cancel; otherwise, <c>false</c>.</value>
        public bool Cancel { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessChangedEventArgs"/> class.
        /// </summary>
        /// <param name="current">The current.</param>
        /// <param name="total">The total.</param>
        public ProcessChangedEventArgs(double current, double total)
        {
            this.Current = current;
            this.Total = total;
            this.Cancel = false;
        }
    }
}
