using System;
using System.Collections.Generic;

namespace Moonlit
{
    public static class DictionaryHelper
    {
        public static IDictionary<TKey, TValue> CopyFrom<TKey, TValue>(
            this IDictionary<TKey, TValue> source,
            IDictionary<TKey, TValue> copy)
        {
            foreach (var pair in copy)
            {
                source.Add(pair.Key, pair.Value);
            }

            return source;
        }

        public static IDictionary<string, object> Extend(params IDictionary<string, object>[] arguments)
        {
            StringComparer stringComparer = StringComparer.OrdinalIgnoreCase;
            var target = new Dictionary<string, object>(stringComparer);

            for (int i = 0; i < arguments.Length; i++)
            {
                var options = arguments[i];
                if (options != null)
                {
                    foreach (var kv in options)
                    {
                        var src = target.GetValue(kv.Key, (object)null);
                        var copy = options[kv.Key];
                        if (copy == null)
                        {
                            continue;
                        }
                        if (!IsPlainObject(copy))
                        {
                            var arr2 = copy as object[];
                            if (arr2 != null)
                            {
                                var arr1 = src as object[] ?? new object[arr2.Length];
                                if (arr1.Length < arr2.Length)
                                {
                                    Array.Resize(ref arr1, arr2.Length);
                                }
                                for (int j = 0; j < arr2.Length; j++)
                                {
                                    if (arr2[j] is IDictionary<string, object>)
                                    {
                                        arr1[j] = Extend(arr2[j] as IDictionary<string, object>);
                                    }
                                    else
                                    {
                                        arr1[j] = arr2[j];
                                    }
                                }
                            }
                            else
                            {
                                target[kv.Key] = Extend(copy as IDictionary<string, object>);
                            }
                        }
                        else
                        {
                            target[kv.Key] = copy;
                        }
                    }
                }
            }

            return target;
        }

        private static bool IsPlainObject(object copy)
        {
            if (copy == null)
            {
                return true;
            }
            return !(copy is IDictionary<string, object> || copy is object[]);
        }

        public static IDictionary<TKey, TValue> CopyFrom<TKey, TValue>(
            this IDictionary<TKey, TValue> source,
            IDictionary<TKey, TValue> copy,
            IEnumerable<TKey> keys)
        {
            foreach (var key in keys)
            {
                source.Add(key, copy[key]);
            }

            return source;
        }

        public static IDictionary<TKey, TValue> Merge<TKey, TValue>(this IDictionary<TKey, TValue> left, IDictionary<TKey, TValue> right, IEqualityComparer<TKey> comparer = null)
        {
            var dict = new Dictionary<TKey, TValue>(comparer);
            foreach (var key in left.Keys)
            {
                dict[key] = left[key];
            }
            foreach (var key in right.Keys)
            {
                dict[key] = right[key];
            }
            return dict;
        }

        public static IDictionary<TKey, TValue> RemoveKeys<TKey, TValue>(
            this IDictionary<TKey, TValue> source,
            IEnumerable<TKey> keys)
        {
            foreach (var key in keys)
            {
                source.Remove(key);
            }

            return source;
        }
        public static TResult GetValue<TKey, TKeyValue, TResult>(this IDictionary<TKey, TKeyValue> dict, TKey key, TResult defaultValue)
        {
            TKeyValue result;
            if (dict.TryGetValue(key, out result))
            {
                return (TResult)(object)result;
            }
            return defaultValue;
        }
    }
}