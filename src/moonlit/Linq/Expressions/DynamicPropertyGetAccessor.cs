using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Moonlit.Linq.Expressions
{
    /// <summary>
    /// author: 老赵 
    /// from:   http://www.cnblogs.com/JeffreyZhao/archive/2009/01/09/DynamicPropertyAccessor-and-FastEval.html
    /// </summary>
    public class DynamicPropertyGetAccessor
    {
        private Func<object, object> _getter;

        public DynamicPropertyGetAccessor(Type type, string propertyName)
            : this(type.GetProperty(propertyName))
        { }

        public DynamicPropertyGetAccessor(PropertyInfo propertyInfo)
        {
            // target: (object)((({TargetType})instance).{Property})

            // preparing parameter, object type
            ParameterExpression instance = Expression.Parameter(
                typeof(object), "instance");

            // ({TargetType})instance
            Expression instanceCast = Expression.Convert(
                instance, propertyInfo.ReflectedType);

            // (({TargetType})instance).{Property}
            Expression propertyAccess = Expression.Property(
                instanceCast, propertyInfo);

            // (object)((({TargetType})instance).{Property})
            UnaryExpression castPropertyValue = Expression.Convert(
                propertyAccess, typeof(object));

            // Lambda expression
            Expression<Func<object, object>> lambda =
                Expression.Lambda<Func<object, object>>(
                    castPropertyValue, instance);

            this._getter = lambda.Compile();
        }

        public object GetValue(object o)
        {
            return this._getter(o);
        }
    }
}