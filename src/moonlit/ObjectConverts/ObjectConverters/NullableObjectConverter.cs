namespace Moonlit.ObjectConverts.ObjectConverters
{
    public abstract class NullableObjectConverter<T> : IObjectConverter
        where T : struct
    {
        protected abstract T ConvertCore(object value);
        public bool TryConvert(ConvertArgs args)
        {
            if (args.DestinationType != typeof(T?))
            {
                return false;
            }
            if (args.Reader.Value == null)
            {
                args.ConvertedObject = null;
                return true;
            }
            args.ConvertedObject = ConvertCore(args.Reader.Value);
            return true;
        }
    }
}