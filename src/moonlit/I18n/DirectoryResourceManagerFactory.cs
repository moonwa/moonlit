using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Moonlit.I18n
{
    public class DirectoryResourceManagerFactory : IResourceManagerFactory 
    {
        static readonly Dictionary<string, Resource> Containers = new Dictionary<string, Resource>(StringComparer.OrdinalIgnoreCase);
        private ResourcePathResolver _resourcePathResolver;
        public string ResourcePath { get; set; }

        public DirectoryResourceManagerFactory(string rootPath)
        {
            _resourcePathResolver = (x, y) => DefaultPathResolver(rootPath, x, y);
        }
        public DirectoryResourceManagerFactory(ResourcePathResolver resourcePathResolver = null)
        {
            _resourcePathResolver = resourcePathResolver ?? ((x, y) => DefaultPathResolver(".", x, y));
        }

        public static IEnumerable<string> DefaultPathResolver(string rootPath, string name, string cultureName)
        {
            cultureName = cultureName.EqualsIgnoreCase("default") ? "" : cultureName;
            cultureName = string.IsNullOrEmpty(cultureName) ? cultureName : ("." + cultureName);
            var path = Path.Combine(rootPath, name + cultureName);
            if (Directory.Exists(path))
                return Directory.GetFiles(path, "*.json", SearchOption.TopDirectoryOnly);
            return Enumerable.Empty<string>();
        }

        Resource IResourceManagerFactory.GetResource(string resourceName)
        {
            if (resourceName == null) throw new ArgumentNullException("resourceName");

            Resource resource;
            if (!Containers.TryGetValue(resourceName, out resource))
            {
                lock (Containers)
                {
                    if (!Containers.TryGetValue(resourceName, out resource))
                    {
                        var source = new DirectoryResourceSource(resourceName, _resourcePathResolver, Encoding.UTF8);
                        resource = new Resource(source);

                        Containers[resourceName] = resource;
                    }
                }
            }
            return resource;
        } 
    }
}