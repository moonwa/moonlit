using System;
using System.Collections.Generic;
using System.Threading;

namespace Moonlit.I18n
{
    public class Resource
    {
        private readonly IDictionary<string, CulturedResource> _culturedResources;
        private readonly IResourceSource _resourceSource;
        private readonly object _resourcesLocker = new object();
        private StringComparer _resKeyComparer;
        public Resource(IResourceSource resourceSource, StringComparer resKeyComparer = null)
        {
            _resourceSource = resourceSource;
            _resKeyComparer = resKeyComparer ?? StringComparer.OrdinalIgnoreCase;
            _culturedResources = new Dictionary<string, CulturedResource>(_resKeyComparer);
        }

        public string Get(string key,   string cultureName)
        {
            // get from special culture
            //
            CulturedResource culturedResource = GetCulturedResource(cultureName);
            var cultureValue = culturedResource.Get(key);
            if (cultureValue != null)
            {
                return cultureValue;
            }

            // get from default culture
            CulturedResource defaultCultruedResource = GetCulturedResource("default");
            var defaultCultureValue = defaultCultruedResource.Get(key);

            if (defaultCultureValue != null)
            {
                return defaultCultureValue;
            }

            return null;
        }

        public CulturedResource GetCulturedResource(string cultureName)
        {
            if (string.IsNullOrEmpty(cultureName))
            {
                cultureName = Thread.CurrentThread.CurrentUICulture.Name;
            }

            string cultureCategoryName = cultureName;

            CulturedResource culturedResource;
            if (!_culturedResources.TryGetValue(cultureCategoryName, out culturedResource))
            {
                lock (_resourcesLocker)
                {
                    if (!_culturedResources.TryGetValue(cultureCategoryName, out culturedResource))
                    {
                        culturedResource = _resourceSource.GetResource(cultureName, _resKeyComparer);
                        _culturedResources[cultureCategoryName] = culturedResource;
                    }
                }
            }
            return culturedResource;
        }


    }
}