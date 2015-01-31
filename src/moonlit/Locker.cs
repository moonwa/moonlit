using System.Collections.Generic;

namespace Moonlit
{
    public class Locker
    {
        public object GetLocker(string key)
        {
            object locker;
            if (!_lockers.TryGetValue(key, out locker))
            {
                lock (_lockers)
                {
                    if (!_lockers.TryGetValue(key, out locker))
                    {
                        locker = new object();
                        _lockers[key] = locker;
                    }
                }
            }
            return locker;
        }
        Dictionary<string, object> _lockers = new Dictionary<string, object>();
    }
}