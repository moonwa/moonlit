using System.Collections.Generic;
using System.Text;

namespace Moonlit.Net.Web
{
    public interface IWebMessageFormatter
    {
        string CreatePostData(IEnumerable<WebParameter> parameters);
        T Deserialize<T>(byte[] bytes, Encoding encoding);
    }
}