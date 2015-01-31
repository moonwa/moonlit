using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Moonlit.ObjectConverts.ObjectConverters
{
    public class ListObjectConverter : IObjectConverter
    {
        Dictionary<Type, Type> _type2ElementType = new Dictionary<Type, Type>();


        public bool TryConvert(ConvertArgs args)
        {
            if (!CacheElementType(args.DestinationType))
            {
                return false;
            }
            var enumerableItems = args.Reader.Value as IEnumerable;
            if (enumerableItems == null)
            {
                return false;
            }
            IList list = (IList)Activator.CreateInstance(args.DestinationType);

            var elementType = _type2ElementType[args.DestinationType];
            foreach (var obj in enumerableItems)
            {
                Type toElementType = args.Converter.GetDestinationType(obj, elementType);
                var dstObject = args.Converter.MapObject(obj, toElementType);
                list.Add(dstObject);
            }

            args.ConvertedObject = list;
            return true;
        }

        public bool CacheElementType(Type propertyType)
        {
            if (_type2ElementType.ContainsKey(propertyType))
            {
                return true;
            }

            var interf = propertyType.GetInterfaces()
                  .FirstOrDefault(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IList<>));
            if (interf != null)
            {
                _type2ElementType[propertyType] = interf.GetGenericArguments()[0];
                return true;
            }
            return false;
        }
    }
}