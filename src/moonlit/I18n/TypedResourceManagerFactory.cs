using System;
using System.Collections.Generic;
using System.Reflection;

namespace Moonlit.I18n
{
    public class TypedResourceManagerFactory : IResourceManagerFactory
    {
        IDictionary<string, Resource> _resourceToType = new Dictionary<string, Resource>();
        public void Register(Type resourceType, string name)
        {

            Resource r = new Resource(new TypeResourceSource(resourceType));
            _resourceToType[name] = r;
        }
        public Resource GetResource(string resourceName)
        {
            if (resourceName == null) throw new ArgumentNullException("resourceName");
            return _resourceToType.GetValue(resourceName, (Resource)null);
        }

        public class TypeResourceSource : IResourceSource
        {
            private readonly Type _resourceType;

            public TypeResourceSource(Type resourceType)
            {
                _resourceType = resourceType;
            }

            public CulturedResource GetResource(string culture, StringComparer resKeyComparer)
            {
                CulturedResource culturedResource = new CulturedResource(culture, resKeyComparer);
                foreach (var property in _resourceType.GetProperties(BindingFlags.Static | BindingFlags.Public))
                {
                    if (property.PropertyType == typeof(string))
                    {
                        culturedResource.Add(property.Name, (string)property.GetValue(null));
                    }
                }
                return culturedResource;
            }
        }
    }

}