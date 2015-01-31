using System;
using System.Collections.Generic;

namespace Moonlit.ObjectConverts.ObjectReaders
{
    public class ClassObjectConverter : IObjectConverter
    {
        public bool TryConvert(ConvertArgs args)
        {
            if (!args.DestinationType.IsClass)
            {
                return false;
            }
            if (args.ConvertedObject == null)
            {
                args.ConvertedObject = args.Converter.CreateInstance(args.Reader, args.DestinationType);
            }
            var typeConversion = TypeConversion.GetTypeConversion(args.ConvertedObject.GetType());
            var accessor = EntityAccessor.GetAccessor(args.ConvertedObject.GetType());
            foreach (var propertyConversion in typeConversion.Properties)
            {
                var propertyName = propertyConversion.PropertyInfo.Name;

                if (!accessor.HasPropertySetter(propertyName))
                {
                    continue;
                }

                object propertyValue = args.Converter.GetValue(args.Reader, propertyName);
                if (propertyValue != ObjectConverter.PropertyNotExisting)
                {
                    var value = args.Converter.MapObject(propertyValue, propertyConversion.PropertyInfo.PropertyType);
                    accessor.SetProperty(args.ConvertedObject, propertyName, value);
                }
            }
            return true;
        }
    }
    internal class ClassObjectReaderFactory : IObjectReaderFactory
    {
        public IObjectReader CreateReader(object o)
        {
            if (o == null)
            {
                return null;
            }
            if (Type.GetTypeCode(o.GetType()) == TypeCode.Object)
            {
                return new ClassObjectReader(o);
            }
            return null;
        }

    }
    internal class ClassObjectReader : IObjectReader
    {
        public object Value { get; private set; }
        public IEnumerable<string> Properties
        {
            get
            {
                var entityAccessor = EntityAccessor.GetAccessor(_valueType);
                var typeConversion = TypeConversion.GetTypeConversion(_valueType);
                foreach (var property in typeConversion.Properties)
                {
                    var propertyName = property.PropertyInfo.Name;
                    if (entityAccessor.HasPropertyGetter(propertyName))
                    {
                        yield return propertyName;
                    }
                }
            }
        }

        public bool TryGetValue(string propertyName, out object obj)
        {
            obj = null;
            var entityAccessor = EntityAccessor.GetAccessor(_valueType);
            if (entityAccessor.HasPropertyGetter(propertyName))
            {
                obj = entityAccessor.GetProperty(Value, propertyName);
                return true;
            }
            return false;
        }

        private Type _valueType;
        public ClassObjectReader(object value)
        {
            Value = value;
            _valueType = value.GetType();
        }

        public object GetValue(string propertyName)
        {
            var entityAccessor = EntityAccessor.GetAccessor(_valueType);
            return entityAccessor.GetProperty(Value, propertyName);
        }
    }
}