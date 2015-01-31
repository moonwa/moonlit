using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Moonlit.Diagnostics
{
    public class NotificationTracker
    {
        private INotifyPropertyChanged changer;
        private System.Collections.Generic.List<string> notifications = new List<string>();

        public NotificationTracker(INotifyPropertyChanged changer)
        {
            this.changer = changer;
            changer.PropertyChanged += (o, e) => { this.notifications.Add(e.PropertyName); };
        }

        /// <summary>
        /// Returns the changed properties in order fired.
        /// </summary>
        /// <remarks>
        /// Returns string[] since this will often be used with CollectionAssert() which
        /// does not work well with string[].
        /// </remarks>
        public string[] ChangedProperties
        {
            get { return this.notifications.ToArray(); }
        }

        public void Reset()
        {
            this.notifications.Clear();
        }
    }
}
