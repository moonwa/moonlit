using System;
using System.Collections.Generic;

namespace Moonlit.ObjectConverts.ObjectReaders
{
    internal class DictionaryObjectReaderFactory : IObjectReaderFactory
    {
        public IObjectReader CreateReader(object obj)
        {
            IDictionary<string, object> di = obj as IDictionary<string, object>;
            if (di != null)
            {
                return new DictionaryObjectReader(di);
            }
            return null;
        }
    }
    internal class DictionaryObjectReader : IObjectReader
    {
        private readonly IDictionary<string, object> _dict;

        public DictionaryObjectReader(IDictionary<string, object> dict)
        {
            if (dict == null) throw new ArgumentNullException("dict");
            _dict = dict;
        }

        public object Value { get { return _dict; } }
        public IEnumerable<string> Properties { get { return _dict.Keys; } }
        public bool TryGetValue(string propertyName, out object obj)
        {
            return _dict.TryGetValue(propertyName, out obj);
        }

        public object GetValue(string propertyName)
        {
            object value = null;
            if (_dict.TryGetValue(propertyName, out value))
            {
                return value;
            }
            return null;
        }
    }
}