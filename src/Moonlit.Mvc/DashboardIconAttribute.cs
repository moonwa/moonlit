using System;
using System.Web.Mvc;

namespace Moonlit.Mvc
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Assembly, AllowMultiple = true, Inherited = true)]
    public class DashboardIconAttribute : ActionFilterAttribute
    {
        private string _text;
        public string Icon { get; set; }

        public string Text
        {
            get { throw new Exception("Please get Text via GetText"); }
            set { _text = value; }
        }

        public string GetText()
        {
            if (string.IsNullOrEmpty(_text))
            {
                return null;
            }
            if (ResourceType != null)
            {
                return EntityAccessor.GetAccessor(ResourceType).GetProperty(null, _text) as string;
            }
            return _text;
        }

        public Type ResourceType { get; set; }
        public DashboardIconAttribute()
        {
            Order = 2;
        }
    }
}