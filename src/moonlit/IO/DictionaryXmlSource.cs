using System.IO;
using System.Xml.Linq;
namespace Moonlit.IO
{
    public class DictionaryXmlSource : IXmlSource 
    {
        private readonly string _path;
        private readonly string _extension;

        public DictionaryXmlSource(string path, string extension = ".xml")
        {
            _path = path;
            _extension = extension;
        }

        public XElement GetSource(string name)
        {
            try
            {
                return XElement.Load(Path.Combine(_path, name + _extension));
            }
            catch (System.IO.FileNotFoundException)
            {
                return null;
            }
            catch (System.IO.DirectoryNotFoundException)
            {
                return null;
            }
        }
    }
}