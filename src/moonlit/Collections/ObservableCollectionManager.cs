

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace MWW.Collections
{
    public class ObservableCollectionManager
    {
        private readonly INotifyCollectionChanged _source;
        private bool _suspend;
        private List<object> _oldItems;
        public ObservableCollectionManager(INotifyCollectionChanged source)
        {
            _source = source;
            _source.CollectionChanged += SourceCollectionChanged;
        }

        void SourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.RaiseCollectionChanged(QNotifyCollectionChangedEventArgs.Create(e));
        }

        public void Suspend()
        {
            if (_suspend)
                throw new InvalidOperationException("cannot suspend twice");
            _suspend = true;
            _source.CollectionChanged -= SourceCollectionChanged;

            var enumerable = _source as IEnumerable;
            if (enumerable != null)
                _oldItems = enumerable.Cast<object>().ToList();
        }
        public void Resume()
        {
            if (!_suspend)
                throw new InvalidOperationException("cannot resume twice");
            _suspend = false;
            _source.CollectionChanged += SourceCollectionChanged;

            var enumerable = _source as IEnumerable;
            if (enumerable != null && _oldItems != null)
            {
                var objectItems = enumerable.Cast<object>().ToList();
                var removedItems = _oldItems.Except(objectItems).ToList();
                var newItems = objectItems.Except(_oldItems).ToList();
                this.RaiseCollectionChanged(new QNotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, newItems));
                this.RaiseCollectionChanged(new QNotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, removedItems));
            }
        }

        private void RaiseCollectionChanged(QNotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            this.CollectionChanged(_source, notifyCollectionChangedEventArgs);
        }

        public event QNotifyCollectionChangedEventHandler CollectionChanged = delegate { };
    }
    public delegate void QNotifyCollectionChangedEventHandler(object sender, QNotifyCollectionChangedEventArgs e);

    public class QNotifyCollectionChangedEventArgs : EventArgs
    {
        public IList<object> Items
        {
            get;
            private set;
        }
        public NotifyCollectionChangedAction Action
        {
            get;
            private set;
        }

        public QNotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, IEnumerable items)
        {
            Action = action;
            if (items != null)
                Items = new List<object>(items.Cast<object>());
        }

        public static QNotifyCollectionChangedEventArgs Create(NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    return new QNotifyCollectionChangedEventArgs(e.Action, e.NewItems);
                case NotifyCollectionChangedAction.Remove:
                    return new QNotifyCollectionChangedEventArgs(e.Action, e.OldItems);
                case NotifyCollectionChangedAction.Replace:
                    return new QNotifyCollectionChangedEventArgs(e.Action, null);
                case NotifyCollectionChangedAction.Reset:
                    return new QNotifyCollectionChangedEventArgs(e.Action, null);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
