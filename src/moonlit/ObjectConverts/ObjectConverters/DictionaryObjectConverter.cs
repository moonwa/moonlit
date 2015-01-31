using System.Collections;
using System.Collections.Generic;

namespace Moonlit.ObjectConverts.ObjectConverters
{
    public class DictionaryObjectConverter : IObjectConverter
    {
        public bool TryConvert(ConvertArgs args)
        {
            var dictInterface = args.DestinationType.ExtractGenericInterface(typeof(IDictionary<,>));
            if (dictInterface != null)
            {
                if (args.ConvertedObject == null)
                {
                    var dictType = typeof(Dictionary<,>).MakeGenericType(dictInterface.GenericTypeArguments);
                    args.ConvertedObject = args.Converter.CreateInstance(args.Reader, dictType);
                }
                var dict = args.ConvertedObject as IDictionary;
                if (dict == null)
                {
                    return false;
                }
                foreach (var property in args.Reader.Properties)
                {
                    var value = args.Converter.GetValue(args.Reader, property);
                    if (value != null)
                        dict.Add(property, args.Converter.MapObject(value, value.GetType()));
                    else
                        dict.Add(property, null);
                }
                return true;
            }
            return false;
        }
    }
}