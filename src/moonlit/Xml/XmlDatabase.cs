using System.Collections.Generic;
using System.Linq;
using Moonlit.Reflection;

namespace Moonlit.Data
{
    public class XmlDatabase
    {
        private readonly IXmlSource _xmlSource;
        public XmlDatabase(IXmlSource xmlSource)
        {
            _xmlSource = xmlSource;
        }

        public IEnumerable<T> Query<T>()
        {
            var xele = _xmlSource.GetSource(typeof(T).FullName);
            if (xele == null)
            {
                xele = _xmlSource.GetSource(typeof(T).Name);
            }
            if (xele == null)
                return Enumerable.Empty<T>();

            return xele.Elements().Select(XmlObjectTranslater.Translate<T>);
        }
    }
}