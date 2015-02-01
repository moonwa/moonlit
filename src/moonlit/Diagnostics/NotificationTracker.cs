using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Moonlit.Diagnostics
{
    /// <summary>
    /// track the properties changed for INotifyPropertyChanged
    /// </summary>
    /// <example>
    /// INotifyPropertyChanged changed;
    /// var tracker = new NotificationTracker(changed);
    /// tracker.ChangedProperties
    /// </example>
    public class NotificationTracker
    { 
        private readonly List<string> _notifications = new List<string>();

        public NotificationTracker(INotifyPropertyChanged changer)
        { 
            changer.PropertyChanged += (o, e) => { this._notifications.Add(e.PropertyName); };
        }

        public static void A()
        {
             
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
            get { return this._notifications.ToArray(); }
        }

        public void Reset()
        {
            this._notifications.Clear();
        }
    }
}
