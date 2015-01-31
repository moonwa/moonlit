namespace Moonlit.ObjectConverts.ObjectConverters
{
    public abstract class StructObjectConverter<T> : IObjectConverter
        where T : struct
    {

        public bool TryConvert(ConvertArgs args)
        {
            if (args.DestinationType != typeof(T))
            {
                return false;
            }
            if (args.Reader.Value == null)
            {
                args.ConvertedObject = default(T);
                return true;
            }

            args.ConvertedObject = ConvertCore(args.Reader.Value);
            return true;
        }

        protected abstract T ConvertCore(object value);
    }
}