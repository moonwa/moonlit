using System.Xml.Linq;

namespace Moonlit.Weixin
{
    public interface IRequestMessage
    {
        void FromXml(XElement element);
    }
    public interface IResponseMessage
    {
        XElement ToXml();
    }
}