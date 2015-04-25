using System.ComponentModel;

namespace Moonlit.UpdateAndRestarterPoxy
{
    public abstract class BaseObject : INotifyPropertyChanged
    {
        public virtual event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}