using System.Collections.Generic;
using System.ComponentModel;

namespace Moonlit
{
    public class PropertyChangeTracker
    {
        private readonly INotifyPropertyChanged _changer;
        private readonly List<string> _notifications = new List<string>();

        public PropertyChangeTracker(INotifyPropertyChanged changer)
        {
            this._changer = changer;
            changer.PropertyChanged += (o, e) => _notifications.Add(e.PropertyName);
        }

        public void Reset()
        {
            _notifications.Clear();
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
            get { return _notifications.ToArray(); }
        }
    }
}
