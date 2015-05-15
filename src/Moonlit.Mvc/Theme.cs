using System;
using System.Collections.Generic;
using System.Web.Routing;

namespace Moonlit.Mvc
{
    public abstract class Theme
    { 
        public override string ToString()
        {
            return Name;
        }
        private Dictionary<Type, string> _control2Templates = new Dictionary<Type, string>();

        public void RegisterControl(Type controlType, string template)
        {
            _control2Templates[controlType] = template;
        }

        public string ResolveControl(Type controlType)
        {
            string template;
            if (_control2Templates.TryGetValue(controlType, out template))
            {
                return template;
            }
            return null;
        }
        protected internal abstract void PreRequest(MoonlitContext context, RequestContext requestContext);
        public abstract string Name { get; }
    }
}