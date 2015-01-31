using System;

namespace Moonlit.ObjectConverts.ObjectConverters
{
    internal class EnumObjectConverter : IObjectConverter
    {
        public bool TryConvert(ConvertArgs args)
        {
            var propertyType = args.DestinationType;
            if (propertyType.IsEnum ||
                   (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>) && propertyType.GetGenericArguments()[0].IsEnum))
            {
                if (args.Reader.Value != null)
                {
                    var s = args.Reader.Value as string;
                    if (!string.IsNullOrWhiteSpace(s))
                    {
                        args.ConvertedObject = Enum.Parse(propertyType, s, true);
                        return true;
                    }
                    args.ConvertedObject = Convert.ToInt32(args.Reader.Value);
                    return true;
                }
            }
            return false;
        }
    }
}