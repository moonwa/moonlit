using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moonlit.Collections
{
    public class IgnoreCaseableDictionary<TValue>
    {
        Dictionary<string, TValue> _values = new Dictionary<string, TValue>();
        Dictionary<string, TValue> _ignoreCaseValues = new Dictionary<string, TValue>(StringComparer.OrdinalIgnoreCase);

        public TValue this[string key]
        {
            set
            {
                _values[key] = value;
                _ignoreCaseValues[key] = value;
            }
        }

        public bool TryGetValue(string key, bool ignoreCase, out TValue value)
        {
            Dictionary<string, TValue> dict = ignoreCase ? _ignoreCaseValues : _values;
            return dict.TryGetValue(key, out value);
        }
    }
}
