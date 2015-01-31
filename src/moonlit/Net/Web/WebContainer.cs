using System;
using System.Net;
using System.Text;

namespace Moonlit.Net.Web
{
    public class WebContainer : IWebContainer
    {
        public CookieContainer CookieContainer { get; set; }

        public Encoding Encoding { get; set; }

        public IWebProxy Proxy { get; set; }

        public WebContainer()
        {
            CookieContainer = new CookieContainer();
            Encoding = System.Text.Encoding.Default;
        }
    }
}