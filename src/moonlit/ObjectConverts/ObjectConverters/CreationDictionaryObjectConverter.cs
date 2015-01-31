using System;
using System.Collections.Generic;

namespace Moonlit.ObjectConverts.ObjectConverters
{
    public class CreationDictionaryObjectConverter : IObjectConverter
    {
        private readonly string _typeName;
        private readonly ITypeResolvor _typeResolvor;

        public CreationDictionaryObjectConverter(string typeName, ITypeResolvor typeResolvor)
        {
            _typeName = typeName;
            _typeResolvor = typeResolvor;
        }


        public bool TryConvert(ConvertArgs args)
        {
            var di = args.Reader.Value as IDictionary<string, object>;

            Type diType = args.DestinationType;
            if (di != null)
            {
                string xtype = di.GetValue(_typeName, (string)null);
                if (xtype != null)
                {
                    diType = _typeResolvor.ResolveType(xtype, true);

                    if (diType == null)
                    {
                        diType = args.DestinationType;
                    }
                    else
                    {
                        if (diType != args.DestinationType && !args.DestinationType.IsAssignableFrom(diType))
                        {
                            return false;
                        }
                    }
                    args.ConvertedObject = args.Converter.CreateInstance(args.Reader, diType);
                }
                return false;
            }
            return false;
        }

        public bool IsSupport(Type propertyType)
        {
            return typeof(IDictionary<string, object>).IsAssignableFrom(propertyType);
        }
    }
}