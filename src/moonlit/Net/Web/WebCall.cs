using System;
using System.Linq;
using System.Net;
using System.Text;

namespace Moonlit.Net.Web
{
    public class WebCall<T> : CallOperation<T>
    {
        private readonly WebCallRequest _webCallRequest;
        private HttpWebClient _webClient;
        public WebCall(WebCallRequest webCallRequest, Callback<T> completed, IWebMessageFormatter protocal = null)
            : base(completed)
        {
            _webCallRequest = webCallRequest.Clone();
            _webCallRequest.TrimUrlParameters();
            _webCallRequest.Uri = AddTimeStamp(_webCallRequest.Uri);
            if (protocal == null)
                protocal = new JsonWebMessageFormatter();
            this.MessageFormatter = protocal;
            this.WebContainer = new NullWebContainer();
        }

        public IWebContainer WebContainer { get; set; }
        private Uri AddTimeStamp(Uri url)
        {
            UriBuilder builder = new UriBuilder(url);
            return builder.AddQuery("tm", Guid.NewGuid().ToString("N")).Uri;
        }

        public CallOperation<T> Invoke()
        {
            if (_webClient != null) throw new InvalidOperationException("just invoke one time");

            _webClient = new HttpWebClient(WebContainer.CookieContainer);
            _webClient.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (Windows; U; Windows NT 6.1; zh-CN; rv:1.9.2.10) Gecko/20100914 Firefox/3.6.10 ( .NET CLR 3.5.30729; .NET4.0E)");
            _webClient.Proxy = WebContainer.Proxy;
            _webClient.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            _webClient.UploadDataCompleted += new UploadDataCompletedEventHandler(WebClientUploadDataCompleted);
            _webClient.DownloadDataCompleted += new DownloadDataCompletedEventHandler(WebClientDownloadDataCompleted);
            bool isPost = _webCallRequest.Parameters.Any(x => x.ParameterType == WebParameterType.PostData);
            if (isPost)
            {
                var body = this.MessageFormatter.CreatePostData(this._webCallRequest.Parameters);
                Encoding encoding = _webClient.Encoding;
                var bytes = encoding.GetBytes(body);
                _webClient.UploadDataAsync(_webCallRequest.Uri, bytes);
            }
            else
            {
                _webClient.DownloadDataAsync(_webCallRequest.Uri, null);
            }

            return this;
        }

        void WebClientDownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e)
        {
            if (e.Error != null)
                this.AddError("", e.Error.Message);
            else
                this.Data = this.MessageFormatter.Deserialize<T>(e.Result, _webClient.Encoding);
            OnCompleted();
        }

        void WebClientUploadDataCompleted(object sender, UploadDataCompletedEventArgs e)
        {
            if (e.Error != null)
                this.AddError("", e.Error.Message);
            else
                this.Data = this.MessageFormatter.Deserialize<T>(e.Result, _webClient.Encoding);
            OnCompleted();
        }

        public IWebMessageFormatter MessageFormatter { get; set; }
    }
}
