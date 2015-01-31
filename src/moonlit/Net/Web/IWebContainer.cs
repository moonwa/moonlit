using System;
using System.Net;
using System.Text;

namespace Moonlit.Net.Web
{
    public interface IWebContainer
    {
        CookieContainer CookieContainer { get; }
        Encoding Encoding { get; }
        IWebProxy Proxy { get; }
    }

    public class NullWebContainer : IWebContainer
    {
        public CookieContainer CookieContainer
        {
            get { return new CookieContainer(); }
        }

        public Encoding Encoding
        {
            get { return Encoding.Default; }
        }

        public IWebProxy Proxy
        {
            get { return null; }
        }
    }
}