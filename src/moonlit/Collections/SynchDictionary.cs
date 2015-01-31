using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moonlit.Collections
{
    public class SynchDictionary<TKey, TValue>
    {
        readonly Dictionary<TKey, TValue> _dictioanry = new Dictionary<TKey, TValue>();
        public TValue this[TKey index]
        {
            get
            {
                lock (_dictioanry)
                {
                    if (_dictioanry.ContainsKey(index))
                        return _dictioanry[index];
                    return default(TValue);
                }
            }
            set
            {
                lock (_dictioanry)
                {
                    if (_dictioanry.ContainsKey(index))
                        _dictioanry[index] = value;
                    else
                        _dictioanry.Add(index, value);
                }
            }
        }
    }
}
