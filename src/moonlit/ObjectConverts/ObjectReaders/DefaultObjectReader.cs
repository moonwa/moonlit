using System.Collections.Generic;

namespace Moonlit.ObjectConverts.ObjectReaders
{
    public class DefaultObjectReader : IObjectReader
    {
        public DefaultObjectReader(object value)
        {
            Value = value;
        }

        public object GetValue(string propertyName)
        {
            return null;
        }

        public object Value { get; private set; }
        public IEnumerable<string> Properties
        {
            get
            {
                yield break;
            }
        }

        public bool TryGetValue(string propertyName, out object obj)
        {
            obj = null;
            return true;
        }
    }
}