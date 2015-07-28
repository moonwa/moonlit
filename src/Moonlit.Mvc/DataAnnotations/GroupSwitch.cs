using System;
using System.Web.Mvc;

namespace Moonlit.Mvc
{
    public class GroupSwitchAttribute : Attribute, IMetadataAware
    {
        public void OnMetadataCreated(ModelMetadata metadata)
        {
            metadata.TemplateHint = "groupswitch";
        }
    }
}