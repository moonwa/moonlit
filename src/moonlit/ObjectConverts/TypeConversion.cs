using System;
using System.Collections.Generic;
using System.Reflection;

namespace Moonlit.ObjectConverts
{
    public class TypeConversion
    {
        private static readonly Dictionary<Type, TypeConversion> Conversions = new Dictionary<Type, TypeConversion>();
        public static TypeConversion GetTypeConversion(Type type)
        {
            if (type == null) throw new ArgumentNullException("type");

            TypeConversion typeConversion = null;
            if (!Conversions.TryGetValue(type, out typeConversion))
            {
                lock (Conversions)
                {
                    if (!Conversions.TryGetValue(type, out typeConversion))
                    {
                        typeConversion = new TypeConversion(type);
                        Conversions[type] = typeConversion;
                    }
                }
            }
            return typeConversion;
        }
        private List<PropertyConversion> _properties = new List<PropertyConversion>();
        private Type _type;

        public TypeConversion(Type type)
        {
            _type = type;
            PropertyInfo[] properties = type.GetProperties(BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo propertyInfo in properties)
            {
                if (propertyInfo.GetSetMethod() != null)
                {
                    _properties.Add(new PropertyConversion(propertyInfo));
                }
            }
        } 

        public Type Type
        {
            get { return _type; }
        }

        public IEnumerable<PropertyConversion> Properties
        {
            get { return _properties; }
        }
    }
}