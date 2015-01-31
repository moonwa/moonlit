using System;
using System.Net;

namespace Moonlit.Net.Web
{
    public class HttpWebClient : WebClient
    {
        private readonly CookieContainer _container;
        public DecompressionMethods AutomaticDecompression { get; set; }
        public HttpWebClient(CookieContainer container)
        {
            _container = container;
            AutomaticDecompression = DecompressionMethods.None;
        }
        protected override WebRequest GetWebRequest(Uri address)
        {
            var request = base.GetWebRequest(address) as HttpWebRequest;
            if (request != null)
            {
                request.CookieContainer = this._container;
                request.AutomaticDecompression = AutomaticDecompression;
            }
            return request;
        }
    }
}