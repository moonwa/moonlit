using System.Xml.Linq;

namespace Moonlit
{
    public interface IXmlSource
    {
        XElement GetSource(string name);
    }
}