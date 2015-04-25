using System;
using System.Diagnostics;
using System.Windows.Input;

namespace Moonlit.UpdateAndRestarterPoxy
{
    [DebuggerDisplay("DelegateCommand: {Text}")]
    public class DelegateCommand<T> : ICommand
    {
        private readonly Action<T> _executeMethod;
        private readonly Func<T, bool> _canExecuteMethod;
        private readonly string _text;

        public DelegateCommand(Action<T> executeMethod, Func<T, bool> canExecuteMethod, string text)
        {
            _executeMethod = executeMethod;
            _canExecuteMethod = canExecuteMethod;
            _text = text;
        }

        public string Text
        {
            get { return _text; }
        }

        public bool CanExecute(object parameter)
        {
            return _canExecuteMethod((T)parameter);
        }

        public event EventHandler CanExecuteChanged = delegate { };

        public void RaiseCanExecuteChanged()
        {
            this.CanExecuteChanged(this, EventArgs.Empty);
        }

        public void Execute(object parameter)
        {
            _executeMethod((T)parameter);
        }
        public bool IsDefault { get; set; }
    }
}