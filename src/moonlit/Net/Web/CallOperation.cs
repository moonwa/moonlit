using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Moonlit.Net.Web
{
    ///<summary>
    ///</summary>
    ///<typeparam name="T"></typeparam>
    [System.Diagnostics.DebuggerStepThrough]
    public class CallOperation<T> : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public event Callback<T> Completed;

        protected void OnCompleted()
        {
            this.IsCompleted = true;
            this.Completed(this);
        }
        [DebuggerStepThrough]
        protected void OnPropertyChanged(string propertyName)
        {
            var eventHandler = PropertyChanged;
            if (eventHandler != null) eventHandler(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool _isCompleted;

        ///<summary>
        ///</summary>
        public bool IsCompleted
        {
            get { return _isCompleted; }
            set
            {
                if (value != _isCompleted)
                {
                    _isCompleted = value;
                    OnPropertyChanged("IsCompleted");
                }
            }
        }
        ///<summary>
        ///</summary>
        public CallOperation(Callback<T> completed)
        {
            if (completed != null)
                this.Completed += completed;
            Errors = new List<Error>();
        }

        private T _data;

        ///<summary>
        ///</summary>
        public T Data
        {
            get { return _data; }
            set
            {
                _data = value;
                OnPropertyChanged("Data");
            }
        }

        ///<summary>
        ///</summary>
        public bool HasError
        {
            get { return Errors.Count != 0; }
        }

        ///<summary>
        ///</summary>
        ///<param name="propertyName"></param>
        ///<param name="message"></param>
        public void AddError(string propertyName, string message)
        {
            Errors.Add(new Error { ErrorMessage = message, PropertyName = propertyName });
        }

        ///<summary>
        ///</summary>
        public List<Error> Errors
        {
            get;
            set;
        }
    }
}