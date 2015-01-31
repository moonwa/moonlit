namespace Moonlit.ObjectConverts.ObjectConverters
{
    internal class DefaultObjectConverter : IObjectConverter
    { 
        public bool TryConvert(ConvertArgs args)
        {
            if (args.DestinationType.IsClass)
            {
                args.ConvertedObject = null;
                return true;
            }
            args.ConvertedObject = null;
            return true;
        } 
    }
}