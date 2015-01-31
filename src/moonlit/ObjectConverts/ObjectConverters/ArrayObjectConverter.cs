using System;
using System.Collections;

namespace Moonlit.ObjectConverts.ObjectConverters
{
    public class ArrayObjectConverter : IObjectConverter
    {
        public bool TryConvert(ConvertArgs args)
        {
            if (!args.DestinationType.IsArray)
            {
                return false;
            }
            if (args.Reader.Value == null)
            {
                args.ConvertedObject = null;
                return true;
            }

            ArrayList arrayList = new ArrayList();
            var elementType = args.DestinationType.GetElementType();
            var enumerableItems = args.Reader.Value as IEnumerable;
            if (enumerableItems == null)
            {
                return true;
            }
            foreach (var obj in enumerableItems)
            {
                Type toElementType = args.Converter.GetDestinationType(obj, elementType);
                var dstObject = args.Converter.MapObject(obj, toElementType);
                arrayList.Add(dstObject);
            }
            var array = (Array)Activator.CreateInstance(args.DestinationType, new object[] { arrayList.Count });
            arrayList.CopyTo(array);
            args.ConvertedObject = array;
            return true;
        }
    }
}