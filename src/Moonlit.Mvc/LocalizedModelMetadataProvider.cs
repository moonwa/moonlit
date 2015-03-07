using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web.Mvc;

namespace Moonlit.Mvc
{
    public class LocalizedModelMetadataProvider : ModelMetadataProvider
    {
        private readonly ModelMetadataProvider _provider;
        private readonly ILocalizer _localizer;

        public LocalizedModelMetadataProvider(ModelMetadataProvider provider, ILocalizer localizer)
        {
            _provider = provider;
            _localizer = localizer;
        }

        public override IEnumerable<ModelMetadata> GetMetadataForProperties(object container, Type containerType)
        {
            var metadatas = _provider.GetMetadataForProperties(container, containerType).ToList();

            foreach (var metadata in metadatas)
            {
                yield return WrapMetadata(metadata);
            }
        }

        private ModelMetadata WrapMetadata(ModelMetadata metadata)
        {
            var lang = Thread.CurrentThread.CurrentUICulture.Name;
            metadata.Watermark = _localizer.GetString(metadata.Watermark, metadata.Watermark, lang);
            metadata.DisplayName = _localizer.GetString(metadata.DisplayName, metadata.DisplayName, lang);
            metadata.ShortDisplayName = _localizer.GetString(metadata.ShortDisplayName, metadata.ShortDisplayName, lang);
            return metadata;
        }

        public override ModelMetadata GetMetadataForProperty(Func<object> modelAccessor, Type containerType, string propertyName)
        {
            return WrapMetadata(_provider.GetMetadataForProperty(modelAccessor, containerType, propertyName));
        }

        public override ModelMetadata GetMetadataForType(Func<object> modelAccessor, Type modelType)
        {
            return WrapMetadata(_provider.GetMetadataForType(modelAccessor, modelType));
        }
    }
}