using Moonlit.Collections;

namespace Moonlit
{
    public class DefaultFormatter : IFormatter
    {
        #region Implementation of IFormatter

        public string Format(object value, object[] args)
        {
            if (value == null)
            {
                return string.Empty;
            }
            if (args.IsNullOrEmpty())
            {
                return value.ToString();
            }
            return string.Format(value.ToString(), args);
        }

        #endregion
    }
}