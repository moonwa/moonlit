using System;

namespace Moonlit
{
    public static class DecimalHelper
    {
        public static decimal? Trim(this decimal? value)
        {
            return value == null ? (decimal?) null : value.Value.Trim();
        }

        public static decimal Trim(this decimal value)
        {
            var s = value.ToString();
            if (s.IndexOf(".") < 0)
            {
                return value;
            } 
            return Convert.ToDecimal(value.ToString().TrimEnd('0'));
        }
    }
}