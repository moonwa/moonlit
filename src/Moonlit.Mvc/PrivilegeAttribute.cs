using System;

namespace Moonlit.Mvc
{
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    public class PrivilegeAttribute : Attribute
    {
        private string _text;
        private string _group;
        private string _description;
        public Type ResourceType { get; set; }
        public string Name { get; set; }

        public string Text
        {
            get { throw new Exception("Please access text via GetText"); }
            set { _text = value; }
        }

        public string Description
        {
            get { throw new Exception("Please access Description via GetDescription"); }
            set { _description = value; }
        }

        public string Group
        {
            get { throw new Exception("Please access group via GetGroup"); }
            set { _group = value; }
        }

        public string GetText()
        {
            if (string.IsNullOrEmpty(_text))
            {
                return null;
            }
            if (ResourceType != null)
            {
                return EntityAccessor.GetAccessor(this.ResourceType).GetProperty(null, _text) as string;
            }
            return _text;
        }
        public string GetGroup()
        {
            if (string.IsNullOrEmpty(_group))
            {
                return null;
            }
            if (ResourceType != null)
            {
                return EntityAccessor.GetAccessor(this.ResourceType).GetProperty(null, _group) as string;
            }
            return _group;
        }
        public string GetDescription()
        {
            if (string.IsNullOrEmpty(_description))
            {
                return null;
            }
            if (ResourceType != null)
            {
                return EntityAccessor.GetAccessor(this.ResourceType).GetProperty(null, _description) as string;
            }
            return _description;
        }
    }
}