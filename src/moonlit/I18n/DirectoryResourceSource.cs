using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Moonlit.Runtime.Serialization;

namespace Moonlit.I18n
{
    /// <summary>
    /// </summary>
    /// <remark>
    /// directory structure:
    /// {root}\
    /// {root}.{language}\
    /// </remark>
    public class DirectoryResourceSource : IResourceSource
    {
        private readonly string _name;
        private readonly ResourcePathResolver _resourcePathResolver;
        private readonly Encoding _encoding;

        public DirectoryResourceSource(string name, ResourcePathResolver resourcePathResolver, Encoding encoding)
        {
            _name = name;
            _resourcePathResolver = resourcePathResolver;
            _encoding = encoding;
        }
        private readonly object _locker = new object();
        public CulturedResource GetResource(string culture, StringComparer resKeyComparer)
        {
            lock (_locker)
            {
                CulturedResource culturedResource = new CulturedResource(culture, resKeyComparer);

                foreach (var fileInfo in _resourcePathResolver(_name, culture))
                {
                    using (var stream = new FileStream(fileInfo, FileMode.Open, FileAccess.Read))
                    {
                        StreamReader reader = new StreamReader(stream, _encoding);
                        var text = reader.ReadToEnd();
                        var json = text.DeserializeAsJsonObject() as IDictionary<string, object>;
                        if (json != null)
                        {
                            foreach (var item in json)
                            {
                                culturedResource.Add(item.Key, item.Value == null ? null : item.Value.ToString());
                            }
                        }
                    }
                }

                return culturedResource;
            }
        }
    }
}