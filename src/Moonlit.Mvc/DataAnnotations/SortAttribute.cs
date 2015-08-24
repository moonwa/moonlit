using System;
using System.Web.Mvc;

namespace Moonlit.Mvc
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class SortAttribute : Attribute, IMetadataAware
    {
        internal const string MetadataAdditionalKey = "ecard_sort";
        public string Sort { get; private set; }
        public SortAttribute(string sort)
        {
            Sort = sort;
        }

        public void OnMetadataCreated(ModelMetadata metadata)
        {
            metadata.AdditionalValues[MetadataAdditionalKey] = Sort;
        }
    }

    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public abstract class LinkProviderAttribute : Attribute, IMetadataAware
    {
        internal const string MetadataAdditionalKey = "ecard_LinkProvider";
        public void OnMetadataCreated(ModelMetadata metadata)
        {
            metadata.AdditionalValues[MetadataAdditionalKey] = this;
        }

        public abstract string MakeUrl(ModelMetadata metadata, ControllerContext controllerContext);
    }

    public class ActionLinkAttribute : LinkProviderAttribute
    {
        public string Action { get; set; }
        public string Controller { get; set; }

        public ActionLinkAttribute(string action, string controller)
        {
            Action = action;
            Controller = controller;
        }

        #region Overrides of LinkProviderAttribute

        public override string MakeUrl(ModelMetadata metadata, ControllerContext controllerContext)
        {
            var url = new UrlHelper(controllerContext.RequestContext);
            var keyObject = metadata.Model as IKeyObject;
            if (keyObject != null)
            {
                return url.Action(Action, Controller, keyObject.Key);
            }
            return string.Empty;
        }

        #endregion
    }

    public interface IKeyObject
    {
        string Key { get; }
    }
}