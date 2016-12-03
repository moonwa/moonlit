using System.Net.Http;
using System.Threading.Tasks;
using log4net;

namespace Moonlit.Weixin
{
    public class WebClientWeixinProxy : IWeixinProxy
    {
        private readonly string _root;
        private static ILog _logger = LogManager.GetLogger(typeof(WebClientWeixinProxy));

        public WebClientWeixinProxy(string root)
        {
            _root = root;
        }

        public async Task<string> GetAsync(string url)
        {
            HttpClient client = new HttpClient();
            string requestUri = $"{_root}/{url}";
            _logger.Debug($"Get {requestUri}");
            var response = await client.GetAsync(requestUri);
            var s = await response.Content.ReadAsStringAsync();
            _logger.Debug($"Get {requestUri} done, response is {s}");
            return s;
        }
        public async Task<byte[]> GetBytesAsync(string url)
        {
            HttpClient client = new HttpClient();
            string requestUri = $"{_root}/{url}";
            _logger.Debug($"Get {requestUri}");
            var response = await client.GetAsync(requestUri);
            var s = await response.Content.ReadAsByteArrayAsync();
            _logger.Debug($"Get {requestUri} done");
            return s;
        }

        public async Task<string> PostAsync(string url, string data)
        {
            HttpClient client = new HttpClient();
            HttpContent content = new StringContent(data);
            string requestUri = $"{_root}/{url}";
            _logger.Debug($"Posting {requestUri} :{data}");
            var response = await client.PostAsync(requestUri, content);
            var s = await response.Content.ReadAsStringAsync();

            _logger.Debug($"Posted {requestUri}, statusCode: {response.StatusCode}, response: {s}");
            return s;
        }
    }

    public interface IWeixinProxy
    {
        Task<byte[]> GetBytesAsync(string url);
        Task<string> GetAsync(string url);
        Task<string> PostAsync(string url, string data);
    }
}