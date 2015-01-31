using System.Reflection;
using System.Xml.Linq;

namespace Moonlit.Xml
{
    public class ResourceXmlSource : IXmlSource
    {
        private readonly Assembly _assembly;
        private readonly string _baseName;
        private readonly string _extendName;

        public ResourceXmlSource(Assembly assembly, string baseName, string extendName)
        {
            _assembly = assembly;
            _baseName = baseName;
            _extendName = extendName;
        }

        public XElement GetSource(string name)
        {
            name = _baseName + "." + name + _extendName;
            using (var stream = _assembly.GetManifestResourceStream(name))
            {
                return stream == null ? null : XElement.Load(stream);
            }
        }
    }
}