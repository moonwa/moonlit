using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moonlit
{
    public interface IFormatter
    {
        string Format(object value, object[] args);
    }

    public class EnumFormatter : IFormatter
    {
        #region Implementation of IFormatter

        public string Format(object value, object[] args)
        {
            if (value == null)
            {
                return string.Empty;
            }
            return EnumHelper.GetEnumDescription((Enum)value);
        }

        #endregion
    }
    public class BooleanFormatter : IFormatter
    {
        private readonly Func<string> _yes;
        private readonly Func<string> _no;

        public BooleanFormatter(Func<string> yes, Func<string>  no)
        {
            _yes = yes;
            _no = no;
        }

        #region Implementation of IFormatter

        public string Format(object value, object[] args)
        {
            if (value == null)
            {
                return string.Empty;
            }
            return (bool)value == true ? _yes() : _no();
        }

        #endregion
    }
    public class DateFormatter : IFormatter
    {
        public DateFormatter()
        {
        }

        #region Implementation of IFormatter

        public string Format(object value, object[] args)
        {
            if (value == null)
            {
                return string.Empty;
            }
            return ((DateTime)value).ToLongDateString();
        }

        #endregion
    }
}
