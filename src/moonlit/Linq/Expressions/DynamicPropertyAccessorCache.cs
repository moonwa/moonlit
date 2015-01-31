using System;
using System.Collections.Generic;

namespace Moonlit.Linq.Expressions
{
    public class DynamicPropertyAccessorCache
    {
        private readonly object _mutex = new object();
        private readonly Dictionary<Type, Dictionary<string, DynamicPropertyGetAccessor>> _getterCache =
            new Dictionary<Type, Dictionary<string, DynamicPropertyGetAccessor>>();

        public DynamicPropertyGetAccessor GetAccessor(Type type, string propertyName)
        {
            DynamicPropertyGetAccessor getAccessor;
            Dictionary<string, DynamicPropertyGetAccessor> typeCache;

            if (this._getterCache.TryGetValue(type, out typeCache))
            {
                if (typeCache.TryGetValue(propertyName, out getAccessor))
                {
                    return getAccessor;
                }
            }

            lock (_mutex)
            {
                if (!this._getterCache.ContainsKey(type))
                {
                    this._getterCache[type] = new Dictionary<string, DynamicPropertyGetAccessor>();
                }

                getAccessor = new DynamicPropertyGetAccessor(type, propertyName);
                this._getterCache[type][propertyName] = getAccessor;

                return getAccessor;
            }
        }
    }
}