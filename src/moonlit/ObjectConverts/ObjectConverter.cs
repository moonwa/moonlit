using System;
using System.Collections.Generic;
using System.Linq;
using Moonlit.ObjectConverts.ObjectConverters;
using Moonlit.ObjectConverts.ObjectReaders;

namespace Moonlit.ObjectConverts
{
    public class ObjectConverter
    {
        private static readonly SortedList<int, IObjectConverter> _defaultObjectConverters = new SortedList<int, IObjectConverter>();
        private static object _propertyNotExisting;
        private readonly SortedList<int, IObjectConverter> _currentObjectConverters = new SortedList<int, IObjectConverter>();

        public ObjectConverter()
        {
            _readerFactory = new CompositeObjectReaderFactory();

            RegisterReaderFactory(int.MaxValue - 3, new DataReaderObjectReaderFactory());
            RegisterReaderFactory(int.MaxValue - 2, new DictionaryObjectReaderFactory());
            RegisterReaderFactory(int.MaxValue - 1, new ClassObjectReaderFactory());
            this.PropertyComparer = StringComparison.Ordinal;
        }
        static ObjectConverter()
        {
            _propertyNotExisting = new object();
            RegisterDefaultConverters(int.MaxValue - 10000, new BooleanObjectConverter());
            RegisterDefaultConverters(new DateTimeObjectConverter());
            RegisterDefaultConverters(new ByteObjectConverter());
            RegisterDefaultConverters(new DecimalObjectConverter());
            RegisterDefaultConverters(new DoubleObjectConverter());
            RegisterDefaultConverters(new Int16ObjectConverter());
            RegisterDefaultConverters(new Int32ObjectConverter());
            RegisterDefaultConverters(new Int64ObjectConverter());
            RegisterDefaultConverters(new SingleObjectConverter());
            RegisterDefaultConverters(new DoubleObjectConverter());
            RegisterDefaultConverters(new DecimalObjectConverter());
            RegisterDefaultConverters(new NullableBooleanObjectConverter());
            RegisterDefaultConverters(new NullableDateTimeObjectConverter());
            RegisterDefaultConverters(new NullableByteObjectConverter());
            RegisterDefaultConverters(new NullableDecimalObjectConverter());
            RegisterDefaultConverters(new NullableDoubleObjectConverter());
            RegisterDefaultConverters(new NullableInt16ObjectConverter());
            RegisterDefaultConverters(new NullableInt32ObjectConverter());
            RegisterDefaultConverters(new NullableInt64ObjectConverter());
            RegisterDefaultConverters(new NullableSingleObjectConverter());
            RegisterDefaultConverters(new NullableDoubleObjectConverter());
            RegisterDefaultConverters(new NullableDecimalObjectConverter());
            RegisterDefaultConverters(new StringObjectConverter());
            RegisterDefaultConverters(new EnumObjectConverter());
            RegisterDefaultConverters(new DictionaryObjectConverter());
            RegisterDefaultConverters(new ArrayObjectConverter());
            RegisterDefaultConverters(new ListObjectConverter());
            RegisterDefaultConverters(new ClassObjectConverter());
            RegisterDefaultConverters(new DefaultObjectConverter());
        }

        public static void RegisterDefaultConverters(IObjectConverter objectConverter)
        {
            RegisterDefaultConverters(_defaultObjectConverters.Last().Key + 1, objectConverter);
        }
        public static void RegisterDefaultConverters(int order, IObjectConverter objectConverter)
        {
            _defaultObjectConverters.Add(order, objectConverter);
        }


        private readonly CompositeObjectReaderFactory _readerFactory;


        public object MapObject(object srcValue, Type dstType)
        {
            var reader = this._readerFactory.CreateReader(srcValue) ?? new DefaultObjectReader(srcValue);

            var convertArgs = new ConvertArgs(reader, this, dstType);
            ConvertCore(convertArgs);
            return convertArgs.ConvertedObject;
        }
        public object MapObject(object srcValue, object dstValue)
        {
            if (dstValue == null)
            {
                return null;
            }

            var reader = this._readerFactory.CreateReader(srcValue) ?? new DefaultObjectReader(srcValue);

            var convertArgs = new ConvertArgs(reader, this, dstValue.GetType()) { ConvertedObject = dstValue };
            ConvertCore(convertArgs);
            return dstValue;
        }

        private void ConvertCore(ConvertArgs convertArgs)
        {
            foreach (var currentObjectConverter in _currentObjectConverters)
            {
                if (currentObjectConverter.Value.TryConvert(convertArgs))
                {
                    return;
                }
            }
            foreach (var objectConverter in _defaultObjectConverters)
            {
                if (objectConverter.Value.TryConvert(convertArgs))
                {
                    return;
                }
            }
        }

        internal Type GetDestinationType(object srcObject, Type dstType)
        {
            if (srcObject == null)
            {
                return dstType;
            }

            if (dstType.IsAssignableFrom(srcObject.GetType()))
            {
                return srcObject.GetType();
            }
            return dstType;
        }

        public void RegisterReaderFactory(int order, IObjectReaderFactory objectReaderFactory)
        {
            this._readerFactory.Register(order, objectReaderFactory);
        }

        public void RegisterConverter(int order, IObjectConverter objectConverter)
        {
            _currentObjectConverters.Add(order, objectConverter);
        }

        internal object CreateInstance(IObjectReader reader, Type destinationType)
        {
            return CreateObjectCore(reader, destinationType);
        }

        protected virtual object CreateObjectCore(IObjectReader reader, Type destinationType)
        {
            return Activator.CreateInstance(destinationType);
        }

        public object GetValue(IObjectReader reader, string propertyName)
        {
            if (reader.Properties != null)
            {
                var property = reader.Properties.FirstOrDefault(x => string.Equals(x, propertyName, PropertyComparer));
                if (property != null)
                {
                    return reader.GetValue(property);
                }
            }
            else
            {
                object obj;
                if (reader.TryGetValue(propertyName, out obj))
                    return obj;
            }
            
            if (!IsIgnoreNotExistingProperty)
                throw new FieldNotFoundException(propertyName);
            
            return _propertyNotExisting;
        }

        public StringComparison PropertyComparer { get; set; }
        public bool IsIgnoreNotExistingProperty { get; set; }

        public static object PropertyNotExisting
        {
            get { return _propertyNotExisting; }
        }
    }
}
