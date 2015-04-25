using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moonlit.Net.Http
{
    public static class HttpHelper
    {
        public static int FileSize(string url)
        {
            System.Net.WebRequest req = System.Net.HttpWebRequest.Create(url);
            req.Method = "HEAD";
            using (System.Net.WebResponse resp = req.GetResponse())
            {
                int ContentLength;
                if (int.TryParse(resp.Headers.Get("Content-Length"), out ContentLength))
                {
                    return ContentLength;
                }
            }
            return 0;
        }
    }
}
