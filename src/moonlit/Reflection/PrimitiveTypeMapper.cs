using System;
using System.Collections.Generic;
using System.Reflection;
using Moonlit.Linq.Expressions;

namespace Moonlit.Reflection
{
    public class PrimitiveTypeMapper : ITypeMapper
    {
        Dictionary<Type, DynamicMethodExecutor> _methods = new Dictionary<Type, DynamicMethodExecutor>();
        public PrimitiveTypeMapper()
        {
            BindingFlags ConvertMethodBindingFlags = BindingFlags.Public | BindingFlags.Static;
            Type typeConverter = typeof(Convert);
            Type[] oneObjectType = new Type[] { typeof(object) };
            _methods.Add(typeof(string), new DynamicMethodExecutor(typeConverter.GetMethod("ToString", ConvertMethodBindingFlags, null, oneObjectType, null)));
            _methods.Add(typeof(int), new DynamicMethodExecutor(typeConverter.GetMethod("ToInt32", ConvertMethodBindingFlags, null, oneObjectType, null)));
            _methods.Add(typeof(byte), new DynamicMethodExecutor(typeConverter.GetMethod("ToByte", ConvertMethodBindingFlags, null, oneObjectType, null)));
            _methods.Add(typeof(short), new DynamicMethodExecutor(typeConverter.GetMethod("ToInt16", ConvertMethodBindingFlags, null, oneObjectType, null)));
            _methods.Add(typeof(long), new DynamicMethodExecutor(typeConverter.GetMethod("ToInt64", ConvertMethodBindingFlags, null, oneObjectType, null)));
            _methods.Add(typeof(bool), new DynamicMethodExecutor(typeConverter.GetMethod("ToBoolean", ConvertMethodBindingFlags, null, oneObjectType, null)));
            _methods.Add(typeof(decimal), new DynamicMethodExecutor(typeConverter.GetMethod("ToDecimal", ConvertMethodBindingFlags, null, oneObjectType, null)));
            _methods.Add(typeof(float), new DynamicMethodExecutor(typeConverter.GetMethod("ToSingle", ConvertMethodBindingFlags, null, oneObjectType, null)));
            _methods.Add(typeof(double), new DynamicMethodExecutor(typeConverter.GetMethod("ToDouble", ConvertMethodBindingFlags, null, oneObjectType, null)));
            _methods.Add(typeof(DateTime), new DynamicMethodExecutor(typeConverter.GetMethod("ToDateTime", ConvertMethodBindingFlags, null, oneObjectType, null)));

            _methods.Add(typeof(byte?), new DynamicMethodExecutor(typeConverter.GetMethod("ToByte", ConvertMethodBindingFlags, null, oneObjectType, null)));
            _methods.Add(typeof(int?), new DynamicMethodExecutor(typeConverter.GetMethod("ToInt32", ConvertMethodBindingFlags, null, oneObjectType, null)));
            _methods.Add(typeof(short?), new DynamicMethodExecutor(typeConverter.GetMethod("ToInt16", ConvertMethodBindingFlags, null, oneObjectType, null)));
            _methods.Add(typeof(long?), new DynamicMethodExecutor(typeConverter.GetMethod("ToInt64", ConvertMethodBindingFlags, null, oneObjectType, null)));
            _methods.Add(typeof(bool?), new DynamicMethodExecutor(typeConverter.GetMethod("ToBoolean", ConvertMethodBindingFlags, null, oneObjectType, null)));
            _methods.Add(typeof(decimal?), new DynamicMethodExecutor(typeConverter.GetMethod("ToDecimal", ConvertMethodBindingFlags, null, oneObjectType, null)));
            _methods.Add(typeof(float?), new DynamicMethodExecutor(typeConverter.GetMethod("ToSingle", ConvertMethodBindingFlags, null, oneObjectType, null)));
            _methods.Add(typeof(double?), new DynamicMethodExecutor(typeConverter.GetMethod("ToDouble", ConvertMethodBindingFlags, null, oneObjectType, null)));
            _methods.Add(typeof(DateTime?), new DynamicMethodExecutor(typeConverter.GetMethod("ToDateTime", ConvertMethodBindingFlags, null, oneObjectType, null)));
            _methods.Add(typeof(byte[]), new DynamicMethodExecutor(typeConverter.GetMethod("ToDateTime", ConvertMethodBindingFlags, null, oneObjectType, null)));
        }

        public object Map(object value, Type type)
        {
            if (value == null || value == DBNull.Value)
                return null;
            DynamicMethodExecutor executor;
            if (_methods.TryGetValue(type, out executor))
                return executor.Execute(null, value);
            return null;
        }
    }
}