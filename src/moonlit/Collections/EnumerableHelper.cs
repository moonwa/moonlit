using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Moonlit.Collections
{
    public static class EnumerableHelper
    {
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> collection)
        {
            return collection == null || !collection.Any();
        }

        public static TResult FirstOrDefault<T, TResult>(this IEnumerable<T> query, Func<T, bool> predicate, Func<T, TResult> selector)
        {
            if (query == null) throw new ArgumentNullException("query");
            if (predicate == null) throw new ArgumentNullException("predicate");
            if (selector == null) throw new ArgumentNullException("selector");

            foreach (var item in query)
            {
                if (predicate(item))
                {
                    return selector(item);
                }
            }

            return default(TResult);
        }

        private static readonly Dictionary<string, Func<IEnumerable, IEnumerable>> OrderByActions
            = new Dictionary<string, Func<IEnumerable, IEnumerable>>();

        public static IEnumerable OrderBy(this IEnumerable items, string orderby)
        {
            if (items == null) throw new ArgumentNullException("items");
            if (@orderby == null) throw new ArgumentNullException("orderby");

            Type enumerableType = items.GetType();
            var cacheKey = enumerableType.FullName + "_" + orderby;
            var action = OrderByActions[cacheKey];
            if (action != null)
                return action(items);

            Type collectionType = enumerableType.ExtractGenericInterface(typeof(IEnumerable<>));

            if (collectionType == null)
            {
                return items;
            }

            Type itemType = collectionType.GetGenericArguments()[0];

            action = GetAction(itemType, enumerableType, orderby);
            OrderByActions[cacheKey] = action;
            return action(items);
        }
        public static IEnumerable<T> OrderBy<T>(this IEnumerable<T> items, string orderby)
        {
            if (items == null) throw new ArgumentNullException("items");
            if (@orderby == null) throw new ArgumentNullException("orderby");


            Type enumerableType = items.GetType();
            var cacheKey = enumerableType.FullName + "_" + orderby;
            var action = OrderByActions[cacheKey];
            if (action != null)
                return (IEnumerable<T>)action(items);

            Type itemType = typeof(T);
            action = GetAction(itemType, enumerableType, orderby);
            OrderByActions[cacheKey] = action;
            return (IEnumerable<T>)action(items);
        }

        private static Func<IEnumerable, IEnumerable> GetAction(Type itemType, Type collectionType, string orderby)
        {
            var actionName = "OrderBy";

            if (@orderby.ToLower().EndsWith(" desc"))
            {
                actionName = "OrderByDescending";
                orderby = orderby.Substring(0, orderby.Length - 5).Trim();
            }
            PropertyInfo propertyInfo = null;
            foreach (var p in itemType.GetProperties())
            {
                if (string.Equals(p.Name, orderby, StringComparison.OrdinalIgnoreCase))
                {
                    propertyInfo = p;
                    break;
                }
            }
            if (propertyInfo == null) return null;

            ParameterExpression pCollection = Expression.Parameter(collectionType, "q");
            var px = Expression.Parameter(itemType, "x");
            var property = Expression.Property(px, orderby);
            var lambda1 = Expression.Lambda(property, px);

            MethodCallExpression callOrderBy = Expression.Call(typeof(Enumerable), actionName, new Type[] { itemType, propertyInfo.PropertyType }, pCollection, lambda1);
            LambdaExpression lambda = Expression.Lambda(callOrderBy, pCollection);
            Delegate func = lambda.Compile();
            return (x) => (IEnumerable)func.DynamicInvoke(x);
        }


    }
}