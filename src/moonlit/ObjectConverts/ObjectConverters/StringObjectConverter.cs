using System;

namespace Moonlit.ObjectConverts.ObjectConverters
{
    public class StringObjectConverter : IObjectConverter
    {

        public bool TryConvert(ConvertArgs args)
        {
            if (args.DestinationType != typeof(string))
            {
                return false;
            }
            if (args.Reader.Value == null)
            {
                args.ConvertedObject = null;
                return true;
            }
            args.ConvertedObject = Convert.ToString(args.Reader.Value);
            return true;
        }
    } 
}