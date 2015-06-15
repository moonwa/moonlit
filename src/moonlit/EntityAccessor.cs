using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Moonlit.Linq.Expressions;

namespace Moonlit
{

    public class EntityAccessor
    {
        private static readonly Dictionary<Type, EntityAccessor> _mappers = new Dictionary<Type, EntityAccessor>();

        private readonly Dictionary<string, DynamicMethodExecutor> _getters;
        private readonly Dictionary<string, DynamicMethodExecutor> _setters;
        private readonly Type _type;

        private EntityAccessor(Type type)
        {
            _type = type;

            _getters = new Dictionary<string, DynamicMethodExecutor>();
            _setters = new Dictionary<string, DynamicMethodExecutor>();
            PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.FlattenHierarchy);
            foreach (PropertyInfo propertyInfo in properties)
            {
                if (propertyInfo.CanWrite)
                {
                    MethodInfo set = propertyInfo.GetSetMethod();
                    if (set != null) _setters.Add(propertyInfo.Name, new DynamicMethodExecutor(set));
                }
                if (propertyInfo.CanRead)
                {
                    MethodInfo get = propertyInfo.GetGetMethod();
                    if (get != null) _getters.Add(propertyInfo.Name, new DynamicMethodExecutor(get));
                }
            }
        }

        public static EntityAccessor GetAccessor(Type type)
        {
            if (!_mappers.ContainsKey(type))
            {
                lock (_mappers)
                {
                    if (!_mappers.ContainsKey(type))
                    {
                        _mappers.Add(type, new EntityAccessor(type));
                    }
                }
            }
            return _mappers[type];
        }

        public object GetProperty(object instance, string propertyName)
        {
            if (_getters.ContainsKey(propertyName))
            {
                return _getters[propertyName].Execute(instance);
            }
            throw new ArgumentOutOfRangeException(propertyName + " not existing!");
        }

        public object SetProperty(object instance, string propertyName, object value)
        {
            if (_setters.ContainsKey(propertyName))
                return _setters[propertyName].Execute(instance, value);
            throw new ArgumentOutOfRangeException(propertyName + " not existing!");
        }

        public bool HasPropertyGetter(string propertyName)
        {
            return _getters.ContainsKey(propertyName);
        }
        public bool HasPropertySetter(string propertyName)
        {
            return _setters.ContainsKey(propertyName);
        }
    }


}
