using System;
using System.Collections.Generic;
using System.Linq;

namespace Moonlit.Net.Web
{
    public class WebCallRequest
    {
        public WebCallRequest Clone()
        {
            WebCallRequest webCallRequest = new WebCallRequest();
            webCallRequest.Uri = this.Uri;
            webCallRequest.Parameters = new List<WebParameter>();
            foreach (var webParameter in Parameters)
            {
                webCallRequest.Parameters.Add(new WebParameter() { Name = webParameter.Name, ParameterType = webParameter.ParameterType, Value = webParameter.Value });
            }
            return webCallRequest;
        }

        public WebCallRequest()
        {
            Parameters = new List<WebParameter>();
        }
        public List<WebParameter> Parameters { get; set; }
        public Uri Uri { get; set; }
        public void TrimUrlParameters()
        {
            UriBuilder uri = new UriBuilder(Uri);
            if (Parameters != null)
                foreach (var webParameter in Parameters.ToList().Where(x => x.ParameterType == WebParameterType.Url))
                {
                    uri.AddQuery(webParameter.Name, webParameter.Value);
                    Parameters.Remove(webParameter);
                }
        }
    }
}