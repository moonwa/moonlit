using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml.Linq;

namespace Moonlit.Reflection
{
    public class XmlObjectTranslater
    {
        static readonly Dictionary<Type, Func<XElement, object>> Translaters = new Dictionary<Type, Func<XElement, object>>();
        public static T Translate<T>(XElement element)
        {
            return (T)Translate(typeof(T), element);
        }

        private static object Translate(Type type, XElement element)
        {
            if (!Translaters.ContainsKey(type))
            {
                lock (Translaters)
                {
                    if (!Translaters.ContainsKey(type))
                    {
                        var field2Actions = new Dictionary<string, Action<object, string>>(StringComparer.OrdinalIgnoreCase);

                        var po = Expression.Parameter(typeof(object), "o");
                        var ps = Expression.Parameter(typeof(string), "s");
                        foreach (PropertyInfo property in type.GetProperties())
                        {
                            if (property.GetSetMethod() != null)
                            {
                                // v = x.Attribute(XName.Get("propertyName"))
                                var toMethod = typeof(Convert).GetMethod("To" + property.PropertyType.Name, BindingFlags.Static | BindingFlags.Public, null, new[] { typeof(object) }, null);
                                // r = convert.toint32()
                                var convert = Expression.Call(toMethod, ps);

                                // r.propertyName = (propertyType)v; 
                                Expression setValue = Expression.Call(Expression.Convert(po, type), property.GetSetMethod(), convert);

                                var lambda = Expression.Lambda<Action<object, string>>(setValue, new[] { po, ps });
                                var action = lambda.Compile();
                                field2Actions.Add(property.Name, action);
                            }
                        }
                        Func<XElement, object> func = (x) =>
                                                          {
                                                              var o = Activator.CreateInstance(type);
                                                              foreach (var attr in x.Attributes())
                                                              {
                                                                  Action<object, string> action;
                                                                  if (!field2Actions.TryGetValue(attr.Name.LocalName, out action))
                                                                      continue;

                                                                  action(o, (string)attr);
                                                              }
                                                              foreach (var ele in x.Elements())
                                                              {
                                                                  Action<object, string> action;
                                                                  if (!field2Actions.TryGetValue(ele.Name.LocalName, out action))
                                                                      continue;

                                                                  action(o, ele.Value);
                                                              }
                                                              return o;
                                                          };
                        Translaters.Add(type, func);
                    }
                }
            }
            return Translaters[type](element);
        }
    }
}