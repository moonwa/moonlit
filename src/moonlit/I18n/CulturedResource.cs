using System;
using System.Collections.Generic;

namespace Moonlit.I18n
{
    public class CulturedResource
    {
        private readonly string _culture;
        private readonly StringComparer _resKeyComparer;
        private readonly Dictionary<string, string> _items;

        public CulturedResource(string culture, StringComparer resKeyComparer)
        {
            _culture = culture;
            _resKeyComparer = resKeyComparer;
            _items = new Dictionary<string, string>(_resKeyComparer);
        }

        public string Culture
        {
            get { return _culture; }
        }

        public IEnumerable<string> Keys
        {
            get { return _items.Keys; }
        }

        public string Get(string resName)
        {
            var items = _items;
            string obj;
            if (items.TryGetValue(resName, out obj))
            {
                return obj;
            }
            return null;
        }

        public void Add(string key, string value)
        {
            _items[key] = value;
        }

    }
}