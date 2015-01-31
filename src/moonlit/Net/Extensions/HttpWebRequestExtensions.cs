using System.Text;
using System.IO;

namespace Moonlit.Net
{
    public static class HttpWebRequestExtensions
    {
        public static string GetText(this System.Net.HttpWebRequest req)
        {
            var rsp = req.GetResponse();
            var encoding = Encoding.Default;

            if (!string.IsNullOrEmpty(rsp.Headers[System.Net.HttpResponseHeader.ContentEncoding]))
            {
                encoding = Encoding.GetEncoding(rsp.Headers[System.Net.HttpResponseHeader.ContentEncoding]);
            }
            var stream = rsp.GetResponseStream();
            StreamReader reader = new StreamReader(stream, encoding);
            return reader.ReadToEnd();
        }
    }
}
