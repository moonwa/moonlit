using System;

namespace Moonlit.Weixin
{
    public class WeixinException : Exception
    {
        public int Code { get; set; }

        public WeixinException(int code, string message) : base(message)
        {
            Code = code;
        }
    }
}